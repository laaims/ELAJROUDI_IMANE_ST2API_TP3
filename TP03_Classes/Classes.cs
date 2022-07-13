using System;
using System.Collections.Generic;

namespace TP03_Classes
{
    // Classes for Serilaization of JSON
    public class City{
        public string name;
        public string latitude;
        public string longitude;
        public City(string n, string lat, string lon)
        {
            name = n;
            latitude = lat;
            longitude = lon;
        }
    }
    public class Current
    {
        public double dt { get; set; }
        public double sunrise { get; set; }
        public double sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double dew_podouble { get; set; }
        public double uvi { get; set; }
        public double clouds { get; set; }
        public double visibility { get; set; }
        public double wind_speed { get; set; }
        public double wind_deg { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Daily
    {
        public double dt { get; set; }
        public double sunrise { get; set; }
        public double sunset { get; set; }
        public double moonrise { get; set; }
        public double moonset { get; set; }
        public double moon_phase { get; set; }
        public Temp temp { get; set; }
        public FeelsLike feels_like { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double dew_podouble { get; set; }
        public double wind_speed { get; set; }
        public double wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<Weather> weather { get; set; }
        public double clouds { get; set; }
        public double pop { get; set; }
        public double rain { get; set; }
        public double uvi { get; set; }
    }

    public class FeelsLike
    {
        public double day { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Root
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public double timezone_offset { get; set; }
        public Current current { get; set; }
        public List<Daily> daily { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Weather
    {
        public double id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }


}
