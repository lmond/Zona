using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Zona.Models;

namespace Zona.Helpers
{
    class UserData
    {
        public static async Task<ObservableCollection<Movies>> GetFavorites()
        {
            var result = new ObservableCollection<Movies>();

            string rootPath = ApplicationData.Current.RoamingFolder.Path;
            string filePath = Path.Combine(rootPath, "Favorites.json");
            if (File.Exists(filePath))
            {
                var file = await ApplicationData.Current.RoamingFolder.GetFileAsync("Favorites.json");
                string content = await FileIO.ReadTextAsync(file);
                result = JsonConvert.DeserializeObject<ObservableCollection<Movies>>(content);
            }
            return result;
        }
        public static async Task<bool> ChnageFavorites(Movies Movie)
        {
            bool IsFavorited = true;
            try
            {
                var favorites = await GetFavorites();
                if (favorites == null)
                {
                    favorites = new ObservableCollection<Movies>();
                }
                else
                {
                    foreach (var item in favorites.ToArray())
                    {
                        if (item.Link == Movie.Link)
                        {
                            favorites.Remove(item);
                            IsFavorited = false;
                        }
                    }
                }

                if (IsFavorited == true)
                    favorites.Add(Movie);

                StorageFile file = await ApplicationData.Current.RoamingFolder.CreateFileAsync("Favorites.json", CreationCollisionOption.ReplaceExisting);
                string data = JsonConvert.SerializeObject(favorites);
                await FileIO.WriteTextAsync(file, data);
                return IsFavorited;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<ObservableCollection<Movies>> GetWatchLater()
        {
            var result = new ObservableCollection<Movies>();
            string rootPath = ApplicationData.Current.RoamingFolder.Path;
            string filePath = Path.Combine(rootPath, "WatchLater.json");
            if (File.Exists(filePath))
            {
                var file = await ApplicationData.Current.RoamingFolder.GetFileAsync("WatchLater.json");
                string content = await FileIO.ReadTextAsync(file);
                result = JsonConvert.DeserializeObject<ObservableCollection<Movies>>(content);
            }
            return result;
        }
        public static async Task<bool> ChnageWatchLater(Movies Movie)
        {
            bool IsFavorited = true;
            try
            {
                var favorites = await GetWatchLater();
                if (favorites == null)
                {
                    favorites = new ObservableCollection<Movies>();
                }
                else
                {
                    foreach (var item in favorites.ToArray())
                    {
                        if (item.Link == Movie.Link)
                        {
                            favorites.Remove(item);
                            IsFavorited = false;
                        }
                    }
                }

                if (IsFavorited == true)
                    favorites.Add(Movie);

                StorageFile file = await ApplicationData.Current.RoamingFolder.CreateFileAsync("WatchLater.json", CreationCollisionOption.ReplaceExisting);
                string data = JsonConvert.SerializeObject(favorites);
                await FileIO.WriteTextAsync(file, data);
                return IsFavorited;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
