using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OpenWeather.Models
{
    internal class WeatherData
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public long dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    internal class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    internal class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    internal class Main
    {
        public double temp { get; set; }
        public double feelsLike { get; set; }
        public double tempMin { get; set; }
        public double tempMax { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    internal class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }
    }

    internal class Clouds
    {
        public int all { get; set; }
    }

    internal class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }
}
