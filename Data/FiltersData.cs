using System.Collections.Generic;
using Zona.Models;

namespace Zona.Data
{
    class FiltersData
    {
        public static List<FilterModel> Typeslist { get { return GetTypesFilter(); } }
        public static List<FilterModel> Genreslist { get { return GetGenresFilter(); } }
        public static List<FilterModel> Yearslist { get { return GetYearsFilter(); } }
        public static List<FilterModel> Countrieslist { get { return GetCountriesFilter(); } }
        public static List<FilterModel> Ratingslist { get { return GetRatingsFilter(); } }
        public static List<FilterModel> Sortlist { get { return GetSortFilter(); } }

        private static List<FilterModel> GetTypesFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "movies", Member = "Фильмы" });
            items.Add(new FilterModel() { Value = "tvseries", Member = "Сериалы" });
            return items;
        }
        private static List<FilterModel> GetGenresFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "genre-drama", Member = "Драма" });
            items.Add(new FilterModel() { Value = "genre-komediia", Member = "Комедия" });
            items.Add(new FilterModel() { Value = "genre-triller", Member = "Триллер" });
            items.Add(new FilterModel() { Value = "genre-melodrama", Member = "Мелодрама" });
            items.Add(new FilterModel() { Value = "genre-boevik", Member = "Боевик" });
            items.Add(new FilterModel() { Value = "genre-kriminal", Member = "Криминал" });
            items.Add(new FilterModel() { Value = "genre-uzhasy", Member = "Ужасы" });
            items.Add(new FilterModel() { Value = "genre-prikliucheniia", Member = "Приключения" });
            items.Add(new FilterModel() { Value = "genre-fantastika", Member = "Фантастика" });
            items.Add(new FilterModel() { Value = "genre-detektiv", Member = "Детектив" });
            items.Add(new FilterModel() { Value = "genre-fentezi", Member = "Фэнтези" });
            items.Add(new FilterModel() { Value = "genre-semeinyi", Member = "Семейный" });
            items.Add(new FilterModel() { Value = "genre-voennyi", Member = "Военный" });
            items.Add(new FilterModel() { Value = "genre-multfilm", Member = "Мультфильм" });
            items.Add(new FilterModel() { Value = "genre-istoriia", Member = "История" });
            items.Add(new FilterModel() { Value = "genre-biografiia", Member = "Биография" });
            items.Add(new FilterModel() { Value = "genre-miuzikl", Member = "Мюзикл" });
            items.Add(new FilterModel() { Value = "genre-vestern", Member = "Вестерн" });
            items.Add(new FilterModel() { Value = "genre-muzyka", Member = "Музыка" });
            items.Add(new FilterModel() { Value = "genre-sport", Member = "Спорт" });
            items.Add(new FilterModel() { Value = "genre-dokumentalnyi", Member = "Документальный" });
            items.Add(new FilterModel() { Value = "genre-korotkometrazhka", Member = "Короткометражка" });
            items.Add(new FilterModel() { Value = "genre-anime", Member = "Аниме" });
            items.Add(new FilterModel() { Value = "genre-film-nuar", Member = "Фильм-нуар" });
            return items;
        }
        private static List<FilterModel> GetYearsFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "year-2019", Member = "2019" });
            items.Add(new FilterModel() { Value = "year-2018", Member = "2018" });
            items.Add(new FilterModel() { Value = "year-2017", Member = "2017" });
            items.Add(new FilterModel() { Value = "year-2016", Member = "2016" });
            items.Add(new FilterModel() { Value = "year-2015", Member = "2015" });
            items.Add(new FilterModel() { Value = "year-2014", Member = "2014" });
            items.Add(new FilterModel() { Value = "year-2013", Member = "2013" });
            items.Add(new FilterModel() { Value = "year-2012", Member = "2012" });
            items.Add(new FilterModel() { Value = "year-2011", Member = "2011" });
            items.Add(new FilterModel() { Value = "year-2010", Member = "2010" });
            items.Add(new FilterModel() { Value = "year-2000s", Member = "2000-е" });
            items.Add(new FilterModel() { Value = "year-90s", Member = "90-е" });
            items.Add(new FilterModel() { Value = "year-80s", Member = "80-е" });
            items.Add(new FilterModel() { Value = "year-70s", Member = "70-е" });
            items.Add(new FilterModel() { Value = "year-60s", Member = "60-е" });
            items.Add(new FilterModel() { Value = "year-50s", Member = "50-е" });
            items.Add(new FilterModel() { Value = "year-40s", Member = "40-е" });
            items.Add(new FilterModel() { Value = "year-old", Member = "до 40-х" });
            return items;
        }
        private static List<FilterModel> GetCountriesFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "country-ssha", Member = "США" });
            items.Add(new FilterModel() { Value = "country-rossiia", Member = "Россия" });
            items.Add(new FilterModel() { Value = "country-frantciia", Member = "Франция" });
            items.Add(new FilterModel() { Value = "country-velikobritaniia", Member = "Великобритания" });
            items.Add(new FilterModel() { Value = "country-italiia", Member = "Италия" });
            items.Add(new FilterModel() { Value = "country-germaniia", Member = "Германия" });
            items.Add(new FilterModel() { Value = "country-indiia", Member = "Индия" });
            items.Add(new FilterModel() { Value = "country-kanada", Member = "Канада" });
            items.Add(new FilterModel() { Value = "country-iaponiia", Member = "Япония" });
            items.Add(new FilterModel() { Value = "country-ispaniia", Member = "Испания" });
            items.Add(new FilterModel() { Value = "country-gonkong", Member = "Гонконг" });
            items.Add(new FilterModel() { Value = "country-koreia-iuzhnaia", Member = "Корея Южная" });
            items.Add(new FilterModel() { Value = "country-avstraliia", Member = "Австралия" });
            items.Add(new FilterModel() { Value = "country-kitai", Member = "Китай" });
            items.Add(new FilterModel() { Value = "country-belgiia", Member = "Бельгия" });
            items.Add(new FilterModel() { Value = "country-shvetciia", Member = "Швеция" });
            items.Add(new FilterModel() { Value = "country-daniia", Member = "Дания" });
            items.Add(new FilterModel() { Value = "country-polsha", Member = "Польша" });
            items.Add(new FilterModel() { Value = "country-chekhiia", Member = "Чехия" });
            items.Add(new FilterModel() { Value = "country-niderlandy", Member = "Нидерланды" });
            items.Add(new FilterModel() { Value = "country-ukraina", Member = "Украина" });
            items.Add(new FilterModel() { Value = "country-irlandiia", Member = "Ирландия" });
            items.Add(new FilterModel() { Value = "country-shveitcariia", Member = "Швейцария" });
            items.Add(new FilterModel() { Value = "country-norvegiia", Member = "Норвегия" });
            items.Add(new FilterModel() { Value = "country-meksika", Member = "Мексика" });
            items.Add(new FilterModel() { Value = "country-serbiia", Member = "Сербия" });
            items.Add(new FilterModel() { Value = "country-argentina", Member = "Аргентина" });
            items.Add(new FilterModel() { Value = "country-vengriia", Member = "Венгрия" });
            items.Add(new FilterModel() { Value = "country-tailand", Member = "Таиланд" });
            items.Add(new FilterModel() { Value = "country-avstriia", Member = "Австрия" });
            return items;
        }
        private static List<FilterModel> GetRatingsFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "rating-9", Member = "от 9" });
            items.Add(new FilterModel() { Value = "rating-8", Member = "от 8" });
            items.Add(new FilterModel() { Value = "rating-7", Member = "от 7" });
            items.Add(new FilterModel() { Value = "rating-6", Member = "от 6" });
            items.Add(new FilterModel() { Value = "rating-5", Member = "от 5" });
            items.Add(new FilterModel() { Value = "rating-4", Member = "от 4" });
            items.Add(new FilterModel() { Value = "rating-3", Member = "от 3" });
            items.Add(new FilterModel() { Value = "rating-2", Member = "от 2" });
            items.Add(new FilterModel() { Value = "rating-1", Member = "от 1" });
            return items;
        }
        private static List<FilterModel> GetSortFilter()
        {
            var items = new List<FilterModel>();
            items.Add(new FilterModel() { Value = "-", Member = "По популярности" });
            items.Add(new FilterModel() { Value = "sort-rating", Member = "По рейтингу" });
            items.Add(new FilterModel() { Value = "sort-date", Member = "По дате выхода" });
            return items;
        }
    }
}