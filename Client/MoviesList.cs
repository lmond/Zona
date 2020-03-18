using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Zona.Models;
using Zona.Helpers;
using System.Diagnostics;

namespace Zona.Client
{
    class MoviesList
    {
        public static async Task<ObservableCollection<Movies>> Parsing(string website)
        {
            ObservableCollection<Movies> MoviesList = new ObservableCollection<Movies>();
            try
            {
                string httpResponseBody = await Browser.GetResponse(website).ConfigureAwait(false) ?? null;
                if (httpResponseBody == null)
                    return MoviesList = null;

                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(WebUtility.HtmlDecode(httpResponseBody));

                List<HtmlNode> toftitle = resultat.DocumentNode.Descendants("ul").Where(x => x.HasClass("results")).ToList();
                var li = toftitle[0].Descendants("li").Where(o => o.HasClass("results-item-wrap")).ToList();

                foreach (var item in li)
                {
                    var img = item.Descendants("meta").Any(o => o.Attributes["itemprop"].Value == "image") == true ?
                        item.Descendants("meta").Where(o => o.Attributes["itemprop"].Value == "image").FirstOrDefault().GetAttributeValue("content", null) : string.Empty;
                    string title = item.Descendants("div").Any(o => o.Attributes["class"].Value == "results-item-title") == true ?
                        item.Descendants("div").Where(o => o.Attributes["class"].Value == "results-item-title").FirstOrDefault().InnerText : string.Empty;
                    string link = item.Descendants("a").Any() == true ?
                        item.Descendants("a").ToList()[0].GetAttributeValue("href", null) : string.Empty;
                    var year = item.Descendants("span").Any(o => o.HasClass("results-item-year")) == true ?
                        item.Descendants("span").Where(o => o.HasClass("results-item-year")).FirstOrDefault().InnerText : string.Empty;
                    double rating = item.Descendants("span").Any(o => o.HasClass("results-item-rating")) == true ? 
                        double.Parse(item.Descendants("span").Where(o => o.HasClass("results-item-rating")).FirstOrDefault().Descendants("span").FirstOrDefault().InnerText, System.Globalization.CultureInfo.InvariantCulture) : 0;
                    double star = rating == 0 ? -1 : rating / 2;

                    MoviesList.Add(new Movies()
                    {
                        Image = img,
                        Title = title,
                        Link = link,
                        Year = year,
                        Rating = rating,
                        Star = star,
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MoviesList = null;
            }
            return MoviesList;
        }
    }
}
