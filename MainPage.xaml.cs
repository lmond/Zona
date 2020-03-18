using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Diagnostics;
using Zona.Models;
using System.Collections.ObjectModel;
using Zona.Client;
using Zona.Helpers;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Zona.Data;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Media.Core;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.System;
using Windows.UI.ViewManagement;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Generic;

namespace Zona
{
    public sealed partial class MainPage : Page
    {
        private int PageIndex { get; set; } = 1;
        private string Website { get; set; } = Consts.ZonaSvrAddr;
        private ObservableCollection<Movies> Movies { get; set; }
        private ObservableCollection<Movies> Favorites { get; set; }
        private ObservableCollection<Movies> WatchLater { get; set; }
        private Movie CurrentMovie { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            MainNavigation.SelectedItem = MainNavigation.MenuItems[1];
            Favorites = await UserData.GetFavorites();
            WatchLater = await UserData.GetWatchLater();
        }

        /*Navigation View*/
        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string Selection = (args.SelectedItemContainer as NavigationViewItem).Tag.ToString();
            switch (Selection)
            {
                case "home":
                    {
                        Website = Consts.ZonaSvrAddr + "movies";
                        break;
                    }
                case "favorites":
                    {
                        Website = "favorites";
                        break;
                    }
                case "watchlater":
                    {
                        Website = "watchlater";
                        break;
                    }
                case "popularfilms":
                    {
                        Website = Consts.ZonaSvrAddr + "movies/filter/year-2019";
                        break;
                    }
                case "popularserials":
                    {
                        Website = Consts.ZonaSvrAddr + "tvseries";
                        break;
                    }
                case "russianfilms":
                    {
                        Website = Consts.ZonaSvrAddr + "movies/filter/country-rossiia";
                        break;
                    }
                case "recentlyadded":
                    {
                        Website = Consts.ZonaSvrAddr + "updates/movies";
                        break;
                    }
                case "newseries":
                    {
                        Website = Consts.ZonaSvrAddr + "updates/tvseries";
                        break;
                    }
                default:
                    {
                        Website = Consts.ZonaSvrAddr + "movies/filter/genre-" + (args.SelectedItemContainer as NavigationViewItem).Tag.ToString();
                        break;
                    }
            }
            PageIndex = 1;
            GetMoviesList(Website);
        }
        private async void BottomItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var Selection = (sender as NavigationViewItem).Tag.ToString();
            switch (Selection)
            {
                case "theme":
                    {
                        if (MainGrid.RequestedTheme == ElementTheme.Default)
                            MainGrid.RequestedTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;

                        MainGrid.RequestedTheme = MainGrid.RequestedTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                        ApplicationView.GetForCurrentView().TitleBar.ButtonForegroundColor = MainGrid.RequestedTheme == ElementTheme.Dark ? Colors.White : Colors.Black;
                        break;
                    }
                case "contact":
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("https://www.lmond.com/?utm_source=Zona&utm_medium=UWPV3#contact")));
                        break;
                    }
                case "feedback":
                    {
                        var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
                        await launcher.LaunchAsync();
                        break;
                    }
                case "rate":
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Windows.ApplicationModel.Package.Current.Id.FamilyName)));
                        break;
                    }
            }
        }
        /*Navigation View*/

        /*Favorites and Watch Later*/
        private async void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            Movie _Sender = (sender as Button).Tag as Movie;
            Movies Movie = new Movies() { Title = _Sender.Title, Year = _Sender.Year, Image = _Sender.Image, Link = _Sender.Link, Rating = _Sender.Rating, Star = _Sender.Star };
            var IsFavorited = await UserData.ChnageFavorites(Movie);
            if (IsFavorited == true)
            {
                Favorites.Add(Movie);
                LikeButton.Background = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0));
            }
            else
            {
                Favorites.Remove(Favorites.Where(x => x.Link == Movie.Link).FirstOrDefault());
                LikeButton.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            }
        }
        private async void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {
            Movie _Sender = (sender as Button).Tag as Movie;
            Movies Movie = new Movies() { Title = _Sender.Title, Year = _Sender.Year, Image = _Sender.Image, Link = _Sender.Link, Rating = _Sender.Rating, Star = _Sender.Star };
            var IsFavorited = await UserData.ChnageWatchLater(Movie);
            if (IsFavorited == true)
            {
                WatchLater.Add(Movie);
                WatchLaterButton.Background = new SolidColorBrush(Color.FromArgb(200, 211, 141, 0));
            }
            else
            {
                WatchLater.Remove(WatchLater.Where(x => x.Link == Movie.Link).FirstOrDefault());
                WatchLaterButton.Background = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
            }
        }
        /*Favorites and Watch Later*/

        /*Search and Filters*/
        private async void MoviesSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender.Text.Length > 2)
            {
                var result = await MoviesList.Parsing(Consts.ZonaSvrAddr + "search/" + sender.Text);
                sender.ItemsSource = result;
            }
        }
        private void MoviesSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null)
            {
                Website = (Consts.ZonaSvrAddr + "search/" + Uri.EscapeDataString(sender.Text));
                PageIndex = 1;
                GetMoviesList(Website);
            }
            else {
                string Link = (args.ChosenSuggestion as Movies).Link;
                if (!string.IsNullOrEmpty(Link))
                    GetSingleMovie(Link);
            }
        }
        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            string Link = Consts.ZonaSvrAddr;
            bool GotSelection = false;

            if (TypesFilters.SelectedIndex != -1)
            {
                var type = (TypesFilters.SelectedItem as FilterModel).Value;
                Link += type + "/filter/";
            }
            if (GenresFilters.SelectedIndex != -1)
            {
                var genre = (GenresFilters.SelectedItem as FilterModel).Value;
                Link += genre + "/";
                GotSelection = true;
            }
            if (YearsFilters.SelectedIndex != -1)
            {
                var year = (YearsFilters.SelectedItem as FilterModel).Value;
                Link += year + "/";
                GotSelection = true;
            }
            if (CountriesFilters.SelectedIndex != -1)
            {
                var country = (CountriesFilters.SelectedItem as FilterModel).Value;
                Link += country + "/";
                GotSelection = true;
            }
            if (RatingsFilters.SelectedIndex != -1)
            {
                var rating = (RatingsFilters.SelectedItem as FilterModel).Value;
                Link += rating + "/";
                GotSelection = true;
            }
            if (GotSelection == true)
            {
                PageIndex = 1;
                Website = Link;
                GetMoviesList(Website);
            }
        }
        private void ResetFilter_Click(object sender, RoutedEventArgs e) {
            GenresFilters.SelectedIndex = YearsFilters.SelectedIndex = CountriesFilters.SelectedIndex = RatingsFilters.SelectedIndex = -1;
        }
        /*Search and Filters*/

        /*Get Movies*/
        private void MoviesView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string Link = (e.ClickedItem as Movies).Link;
            if (!string.IsNullOrEmpty(Link))
                GetSingleMovie(Link);
        }
        public async void GetMoviesList(string Link)
        {
            MainLoader.IsLoading = true;
            switch (Link)
            {
                case "favorites":
                    Movies = Favorites;
                    break;
                case "watchlater":
                    Movies = WatchLater;
                    break;
                default:
                    Movies = await MoviesList.Parsing(Link + "?page=" + PageIndex);
                    /*string httpResponseBody = await Browser.GetResponse(Link + "?page=" + PageIndex).ConfigureAwait(false) ?? null;
                    

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

                       
                    }*/
                    break;
            }
            PaggingGrid.Visibility = Movies == null ? Visibility.Collapsed : Visibility.Visible;
            NoContentStack.Visibility = Movies != null ? Visibility.Collapsed : Visibility.Visible;
            MoviesView.ItemsSource = Movies;
            if (Movies != null)
            {
                NextPage.IsEnabled = Movies.Count < 60 ? false : true;
                PreviousPage.IsEnabled = PageIndex > 1 ? true : false;

                MoviesView.Width = Movies.Count < 8 ? Movies.Count * 220 : Double.NaN;
            }
            MainLoader.IsLoading = false;
        }
        public async void GetSingleMovie(string Link) {
            MainLoader.IsLoading = true;
            MovieDetails.ChangeView(0, 0, MovieDetails.ZoomFactor);

            var path1 = Link.Substring(Link.IndexOf('/') + 1);
            var path = path1.Substring(0, path1.LastIndexOf('/'));

            if (path == "movies")
            {
                CurrentMovie = await SingleMovie.GetMovie(Consts.ZonaSvrAddr + Link);
                SerieDataBlock.Height = 0;
            }
            else {
                CurrentMovie = await SingleMovie.GetSerie(Consts.ZonaSvrAddr + Link);
                SerieDataBlock.Height = CurrentMovie.Seasons == null && CurrentMovie.EpisodeItem == null ? 0 : Double.NaN;
            }

            if (CurrentMovie != null)
            {
                CurrentMovie.Link = Link;
                MovieDetailsView.DataContext = CurrentMovie;
                MovieDetailsView.Visibility = Visibility.Visible;

                LikeButton.Background = Favorites.Where(x => x.Link == Link).Count() > 0 ?
                    new SolidColorBrush(Color.FromArgb(200, 255, 0, 0)) : new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
                WatchLaterButton.Background = WatchLater.Where(x => x.Link == Link).Count() > 0 ?
                    new SolidColorBrush(Color.FromArgb(200, 211, 141, 0)) : new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));

                PlayButton.Visibility = CurrentMovie.MovieFile != null ? Visibility.Visible : Visibility.Collapsed;
                TrailerButton.Visibility = CurrentMovie.Trailer != null ? Visibility.Visible : Visibility.Collapsed;

                SeasonsList.SelectedIndex = CurrentMovie.Seasons != null && CurrentMovie.Seasons.Count > 0 ? 0 : -1;
                EpisodeItems.SelectedIndex = CurrentMovie.EpisodeItem != null && CurrentMovie.EpisodeItem.Count > 0 ? 0 : -1;
            }
            MainLoader.IsLoading = false;
        }
        /*Get Movies*/

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string _fileUrl = (sender as Button).Tag != null ? (sender as Button).Tag.ToString() : null;
            if(_fileUrl != null) PlayVideo(_fileUrl);
        }
        private async void SeasonsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayerLoader.IsLoading = EpisodesLoader.IsLoading = true;
            var SeasonLink = (e.ClickedItem as Season).Link;
            var result = await SingleMovie.GetEpisodesList(SeasonLink);
            if (result != null)
            {
                PlayButton.Tag = result.MovieFile;
                PostersFlip.ItemsSource = result.Posters;
                EpisodeItems.ItemsSource = result.EpisodeItem;
                EpisodeItems.SelectedIndex = result.EpisodeItem != null ? 0 : -1;
            }
            PlayerLoader.IsLoading = EpisodesLoader.IsLoading = false;
        }
        private async void EpisodeItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayerLoader.IsLoading = true;
            var EpisodeId = (e.ClickedItem as Episode).Link;
            var Episode = await SingleMovie.GetEpisode(EpisodeId);
            if (Episode != null)
            {
                PostersFlip.ItemsSource = Episode.images;
                PlayButton.Tag = Episode.url;
                PlayVideo(Episode.url);
            }
            PlayerLoader.IsLoading = false;
        }
        public void PlayVideo(string source) {            
            if (source != null)
            {
                VideoPlayerPanel.Visibility = Visibility.Visible;
                MovieNameOnVideoPlayer.Text = InnerPageTitle.Text.ToString();
                MainMPE.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(source));
                MainMPE.MediaPlayer.Play();
            }
        }

        private void CloseVideo_Click(object sender, RoutedEventArgs e)
        {
            MainMPE.MediaPlayer.Source = null;
            VideoPlayerPanel.Visibility = Visibility.Collapsed;
        }
        private void CloseMovieDetails_Click(object sender, RoutedEventArgs e)
        {
            MovieDetailsView.Visibility = Visibility.Collapsed;
        }
        private void ClosePopupHelper_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            MovieDetailsView.Visibility = Visibility.Collapsed;
        }

        /*private async void ShowDeveloperMessage()
        {
            MessageModel message = await Message.GetMessageAsync();

            if (message != null)
            {
                var localObjectStorageHelper = new LocalObjectStorageHelper();
                if (localObjectStorageHelper.KeyExists("message_" + message.Id))
                {
                    int count = localObjectStorageHelper.Read<int>("message_" + message.Id);
                    if (count < message.Count)
                    {
                        DeveloperMessage.Content = message.Message;
                        DeveloperMessage.Show();
                        localObjectStorageHelper.Save("message_" + message.Id, count + 1);
                    }
                }
                else
                {
                    localObjectStorageHelper.Save("message_" + message.Id, 0);
                }

            }
        }*/

        private void ChangePage_Click(object sender, RoutedEventArgs e)
        {
            var action = (sender as Button).Tag.ToString();
            if (action == "prev")
            {
                if (PageIndex == 1) return;
                PageIndex -= 1;
            }
            else {
                PageIndex += 1;
            }
            GetMoviesList(Website);
        }
    }
}