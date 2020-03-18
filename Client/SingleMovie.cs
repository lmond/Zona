using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zona.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Zona.Data;
using Zona.Helpers;

namespace Zona.Client
{
    class SingleMovie
    {
        public static async Task<Movie> ParseMainInfo(string httpResponseBody)
        {
            Movie CurrentMovie = new Movie();
            try
            {
                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(WebUtility.HtmlDecode(httpResponseBody));
                List<HtmlNode> toftitle = resultat.DocumentNode.Descendants("main").Where(x => x.HasClass("l-content")).ToList();

                /*Main Items*/
                CurrentMovie.Image = toftitle[0].Descendants("meta").Any(o => o.Attributes["itemprop"].Value == "image") == true ?
                    toftitle[0].Descendants("meta").Where(o => o.Attributes["itemprop"].Value == "image").FirstOrDefault().GetAttributeValue("content", null) : string.Empty; 

                CurrentMovie.Title = toftitle[0].Descendants("span").Any(o => o.Attributes["class"].Value == "js-title") == true ?
                    toftitle[0].Descendants("span").Where(o => o.Attributes["class"].Value == "js-title").FirstOrDefault().InnerText : string.Empty;

                CurrentMovie.Year = toftitle[0].Descendants("dt").Any(o => (o.Name == "dt" && o.InnerText == "Год")) == true ?
                    toftitle[0].Descendants("dt").Where(o => (o.Name == "dt" && o.InnerText == "Год")).FirstOrDefault().ParentNode.Descendants("dd").FirstOrDefault().InnerText : string.Empty;

                CurrentMovie.Duration = toftitle[0].Descendants("time").Any() == true ?
                    Regex.Replace(toftitle[0].Descendants("time").FirstOrDefault().InnerText, @"\s+\s+", "") : string.Empty;

                CurrentMovie.Description = toftitle[0].Descendants("div").Any(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("description"))) == true ?
                    toftitle[0].Descendants("div").Where(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("description"))).FirstOrDefault().InnerText : string.Empty;
                /*Main Items*/

                /*Ratings*/
                CurrentMovie.Rating = toftitle[0].Descendants("span").Any(o => o.HasClass("entity-rating-mobi")) == true ?
                    double.Parse(toftitle[0].Descendants("span").Where(o => o.HasClass("entity-rating-mobi")).FirstOrDefault().InnerText, System.Globalization.CultureInfo.InvariantCulture) : 0;

                CurrentMovie.Star = CurrentMovie.Rating == 0 ? -1 : CurrentMovie.Rating / 2;
                /*Ratings*/

                /*Get Lists*/
                CurrentMovie.Genres = toftitle[0].Descendants("dd").Any(o => (o.HasClass("js-genres"))) == true ?
                    String.Join(", ", toftitle[0].Descendants("dd").Where(o => (o.HasClass("js-genres"))).FirstOrDefault().Descendants("span").ToList().Select(p => p.InnerText).ToArray()) : string.Empty;

                CurrentMovie.Countries = toftitle[0].Descendants("dd").Any(o => (o.Attributes["class"] != null && o.Attributes["class"].Value.Contains("js-countries"))) == true ?
                    String.Join(", ", toftitle[0].Descendants("dd").Where(o => (o.Attributes["class"] != null && o.Attributes["class"].Value.Contains("js-countries"))).FirstOrDefault().Descendants("span").ToList().Select(p => p.InnerText).ToArray()) : string.Empty;

                CurrentMovie.Directors = toftitle[0].Descendants("span").Any(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("director"))) == true ?
                    String.Join(", ", toftitle[0].Descendants("span").Where(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("director"))).ToList().Select(p => p.Descendants("span").FirstOrDefault().InnerText).ToArray()) : string.Empty;

                var _ActorsList = toftitle[0].Descendants("span").Any(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("actor"))) == true ?
                    toftitle[0].Descendants("span").Where(o => (o.Attributes["itemprop"] != null && o.Attributes["itemprop"].Value.Contains("actor"))).ToList().Select(p => p.Descendants("span").FirstOrDefault().InnerText).ToList() : null;

                CurrentMovie.ActorsList = _ActorsList != null ? await GetActorImages(_ActorsList) : null;
                /*Get Lists*/

                /*Get Videos*/
                if (toftitle[0].Descendants("div").Any(o => o.HasClass("entity-player")) == true)
                {
                    var _AjaxVideoId = toftitle[0].Descendants("div").Any(o => o.HasClass("entity-player")) == true ?
                        toftitle[0].Descendants("div").Where(o => o.HasClass("entity-player")).FirstOrDefault().GetAttributeValue("data-id", null) : null;
                    if (_AjaxVideoId != null) {
                        string _AjaxUrl = Consts.ZonaSvrAddr + "/ajax/video/" + _AjaxVideoId;
                        string response = await Browser.GetResponse(_AjaxUrl).ConfigureAwait(false) ?? null;
                        var result = JsonConvert.DeserializeObject<Video>(response);
                        CurrentMovie.Posters = result.images;
                        CurrentMovie.MovieFile = result.url;
                    }
                }
                CurrentMovie.Trailer = toftitle[0].Descendants("a").Any(o => (o.HasClass("trailer-btn"))) == true ?
                    toftitle[0].Descendants("a").Where(o => (o.HasClass("trailer-btn"))).FirstOrDefault().GetAttributeValue("data-link", null) : null;
                /*Get Videos*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "///" + ex.StackTrace);
                CurrentMovie = null;
            }
            return CurrentMovie;
        }
        public static async Task<Movie> GetMovie(string website)
        {
            Movie CurrentMovie = null;
            try
            {
                string httpResponseBody = await Browser.GetResponse(website).ConfigureAwait(false) ?? null;
                if (httpResponseBody == null)
                    return CurrentMovie = null;
                else
                    CurrentMovie = await ParseMainInfo(httpResponseBody);
            }
            catch (Exception)
            {
                
            }
            return CurrentMovie;
        }
        public static async Task<Movie> GetSerie(string website)
        {
            Movie CurrentSerie = null;
            try
            {
                string httpResponseBody = await Browser.GetResponse(website).ConfigureAwait(false) ?? null;
                if (httpResponseBody == null)
                    return CurrentSerie = null;
                else
                    CurrentSerie = await ParseMainInfo(httpResponseBody);

                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(WebUtility.HtmlDecode(httpResponseBody));

                List<HtmlNode> toftitle = resultat.DocumentNode.Descendants("main").Where(x => x.HasClass("l-content")).ToList();

                /*Seasons List*/
                var _SeasonsNode = toftitle[0].Descendants("div").Where(o => (o.Name == "div" && o.HasClass("entity-seasons"))).FirstOrDefault().ChildNodes.Any(o => o.HasClass("entity-season")) == true ?
                    toftitle[0].Descendants("div").Where(o => (o.Name == "div" && o.HasClass("entity-seasons"))).FirstOrDefault().ChildNodes.Where(o => o.HasClass("entity-season")).ToList() : null;
                if (_SeasonsNode != null && _SeasonsNode.Count != 0)
                {
                    List<Season> _Seasons = new List<Season>();
                    foreach (var item in _SeasonsNode)
                    {
                        _Seasons.Add(new Season()
                        {
                            Title = item.InnerText,
                            Link = item.GetAttributeValue("href", null)
                        });
                    }
                    CurrentSerie.Seasons = _Seasons;
                }
                /*Seasons List*/

                /*Episodes List*/
                var _EpisodesList = toftitle[0].Descendants("ul").Any(o => (o.Name == "ul" && o.HasClass("js-episodes"))) == true ?
                    toftitle[0].Descendants("ul").Where(o => (o.Name == "ul" && o.HasClass("js-episodes"))).FirstOrDefault().Descendants("li").ToList() : null;
                if (_EpisodesList != null && _EpisodesList.Count != 0)
                {
                    List<Episode> _Episodes = new List<Episode>();
                    foreach (var item in _EpisodesList)
                    {
                        var _EpImage = item.Descendants("span").Any(x => x.Name == "span" && x.HasClass("entity-episode-img-wrap")) == true ?
                            item.Descendants("span").Where(x => x.Name == "span" && x.HasClass("entity-episode-img-wrap")).FirstOrDefault().InnerHtml : null;
                        if (_EpImage != null) {
                            var res = _EpImage.Substring(_EpImage.LastIndexOf('(') + 1);
                            _EpImage = res.Substring(0, res.LastIndexOf(')'));
                        }
                        string _title = item.Descendants("span").Any(x => x.HasClass("entity-episode-name")) == true ?
                            item.Descendants("span").Where(x => x.HasClass("entity-episode-name")).FirstOrDefault().InnerText.Replace("  ", String.Empty) : string.Empty;
                        string _link = item.Descendants("span").Any(x => x.HasClass("entity-episode-link")) == true ?
                            item.Descendants("span").Where(x => x.HasClass("entity-episode-link")).FirstOrDefault().GetAttributeValue("data-id", null) : string.Empty;
                        _Episodes.Add(new Episode()
                        {
                            Title = _title,
                            Image = _EpImage,
                            Link = _link
                        });
                    }
                    CurrentSerie.EpisodeItem = _Episodes;
                }
                /*Episodes List*/
            }
            catch (Exception)
            {

            }
            return CurrentSerie;
        }
        public static async Task<Video> GetEpisode(string EpisodeId)
        {
            var result = new Video();
            try
            {
                var _AjaxUrl = Consts.ZonaSvrAddr + "ajax/video/" + EpisodeId;
                string response = await Browser.GetResponse(_AjaxUrl).ConfigureAwait(false) ?? null;
                result = JsonConvert.DeserializeObject<Video>(response);
                var _MovieFile = result.url;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        public static async Task<Movie> GetEpisodesList(string SeasonId)
        {
            Movie CurrentSerie = new Movie();
            try
            {
                string website = Consts.ZonaSvrAddr + SeasonId;
                string httpResponseBody = await Browser.GetResponse(website).ConfigureAwait(false) ?? null;
                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(WebUtility.HtmlDecode(httpResponseBody));
                List<HtmlNode> toftitle = resultat.DocumentNode.Descendants("main").Where(x => x.HasClass("l-content")).ToList();

                /*Get Video URL*/
                if (toftitle[0].Descendants("div").Any(o => o.HasClass("entity-player")) == true)
                {
                    var _AjaxVideoId = toftitle[0].Descendants("div").Any(o => o.HasClass("entity-player")) == true ?
                        toftitle[0].Descendants("div").Where(o => o.HasClass("entity-player")).FirstOrDefault().GetAttributeValue("data-id", null) : null;
                    if (_AjaxVideoId != null)
                    {
                        string _AjaxUrl = Consts.ZonaSvrAddr + "/ajax/video/" + _AjaxVideoId;
                        string response = await Browser.GetResponse(_AjaxUrl).ConfigureAwait(false) ?? null;
                        var result = JsonConvert.DeserializeObject<Video>(response);
                        CurrentSerie.Posters = result.images;
                        CurrentSerie.MovieFile = result.url;
                    }
                }
                /*Get Video URL*/

                /*Episodes List*/
                var _EpisodesList = toftitle[0].Descendants("ul").Any(o => (o.Name == "ul" && o.HasClass("js-episodes"))) == true ?
                    toftitle[0].Descendants("ul").Where(o => (o.Name == "ul" && o.HasClass("js-episodes"))).FirstOrDefault().Descendants("li").ToList() : null;
                if (_EpisodesList != null && _EpisodesList.Count != 0)
                {
                    List<Episode> _Episodes = new List<Episode>();
                    foreach (var item in _EpisodesList)
                    {
                        var _EpImage = item.Descendants("span").Any(x => x.Name == "span" && x.HasClass("entity-episode-img-wrap")) == true ?
                            item.Descendants("span").Where(x => x.Name == "span" && x.HasClass("entity-episode-img-wrap")).FirstOrDefault().InnerHtml : null;
                        if (_EpImage != null)
                        {
                            var res = _EpImage.Substring(_EpImage.LastIndexOf('(') + 1);
                            _EpImage = res.Substring(0, res.LastIndexOf(')'));
                        }
                        string _title = item.Descendants("span").Any(x => x.HasClass("entity-episode-name")) == true ?
                            item.Descendants("span").Where(x => x.HasClass("entity-episode-name")).FirstOrDefault().InnerText.Replace("  ", String.Empty) : string.Empty;
                        string _link = item.Descendants("span").Any(x => x.HasClass("entity-episode-link")) == true ?
                            item.Descendants("span").Where(x => x.HasClass("entity-episode-link")).FirstOrDefault().GetAttributeValue("data-id", null) : string.Empty;
                        _Episodes.Add(new Episode()
                        {
                            Title = _title,
                            Image = _EpImage,
                            Link = _link
                        });
                    }
                    CurrentSerie.EpisodeItem = _Episodes;
                }
                /*Episodes List*/
            }
            catch (Exception)
            {
                CurrentSerie = null;
            }
            return CurrentSerie;
        }
        public static async Task<List<ActorsList>> GetActorImages(List<string> actors) {
            List<ActorsList> ActorsList = new List<ActorsList>();
            foreach (var item in actors)
            {
                string actorLat = Translit.translit(item);
                string image = "https://www.lmond.com/muviz/actors/" + actorLat + ".jpg";
                try
                {
                    HttpWebRequest request = WebRequest.Create(image) as HttpWebRequest;
                    request.Method = "HEAD";
                    HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        image = null;
                    }
                }
                catch
                {
                    image = null;
                }
                ActorsList.Add(new ActorsList { Name = item, Image = image });
            }
            return ActorsList;
        }
    }
}