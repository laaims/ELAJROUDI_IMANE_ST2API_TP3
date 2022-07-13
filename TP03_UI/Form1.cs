using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using TP03_Classes;
using System.Device.Location;

namespace TP03_UI
{
    public partial class Form1 : Form
    {
        // API key
        private const string APIKey = "d09d85ca6f9120e8d0c2857a93e05f9b";
        private List<City> cities;
        public Form1()
        {
            InitializeComponent();
            InitCities();
        }

        private void InitCities()
        {
            //New York
            //Moscow
            //Berlin
            //Paris
            //Kiev
            //Oslo
            //Morocow
            //New Jersey
            //Tokyo
            cities = new List<City>();
            cities.Add(new City("New York","40.6943","73.9249"));
            cities.Add(new City("Helsinki", "60.1699", "24.9384"));
            cities.Add(new City("Berlin","52.5200","13.4050"));
            cities.Add(new City("Paris","48.8566","2.3522"));
            cities.Add(new City("Kiev","50.4501","30.5234"));
            cities.Add(new City("Oslo","59.9139","10.7522"));
            cities.Add(new City("Cairo", "30.0444", "31.2357"));
            cities.Add(new City("New Jersey","40.0583","74.4057"));
            cities.Add(new City("Tokyo","35.6762","139.6503"));
            cities.Add(new City("Mexico City","19.4333","-99.1333"));
        }

        private City SearchCity(string city)
        {
            for(int i = 0; i < cities.Count; i++)
            {
                if(cities[i].name == city)
                {
                    return cities[i];
                }
            }
            return null;
        }
        public static async Task<string> SendRequestAsync(string lat, string lon, string part)
        {
            string query = $@"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude={part}&units=metric&appid={APIKey}";
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(query)
            };
            using (var response = await client.SendAsync(request))
            {
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LoadIcon(string icon_url, PictureBox pictureBox)
        {
            var request = WebRequest.Create($@"http://openweathermap.org/img/wn/{icon_url}@2x.png");

            using (var res = request.GetResponse())
            using (var stream = res.GetResponseStream())
            {
                pictureBox.Image = Bitmap.FromStream(stream);
            }
        }
        private void SetupWeather(Root weather_report)
        {
            // Filling data in today weather panel
            timezoneLbl.Text = weather_report.timezone;
            curTempLbl.Text = Convert.ToInt32(weather_report.current.temp).ToString() + "°C";
            conditionLbl.Text = weather_report.current.weather[0].main;
            humidityLbl.Text = weather_report.current.humidity.ToString();
            windLbl.Text = weather_report.current.wind_speed.ToString();
            sunriseLbl.Text = UnixTimeStampToDateTime(weather_report.current.sunrise).ToString();
            sunsetLbl.Text = UnixTimeStampToDateTime(weather_report.current.sunset).ToString();
            LoadIcon(weather_report.current.weather[0].icon, curWeatherIco);

            if (weather_report.daily.Count < 5) {
                panel1.Show();
                defLabel.Hide();
                return;
            }

            // Filling data in 5 days forcast panel

            // For Day 1
            List<Daily> forecast = weather_report.daily;
            day1Lbl.Text = UnixTimeStampToDateTime(forecast[1].dt).ToString("ddd");
            d1condLbl.Text = forecast[1].weather[0].main;
            d1tempLbl.Text = Convert.ToInt32(forecast[1].temp.day).ToString() + "°C";
            LoadIcon(forecast[1].weather[0].icon, d1WeatherIco);

            // For Day 2
            day2Lbl.Text = UnixTimeStampToDateTime(forecast[2].dt).ToString("ddd");
            d2condLbl.Text = forecast[2].weather[0].main;
            d2TempLbl.Text = Convert.ToInt32(forecast[2].temp.day).ToString() + "°C";
            LoadIcon(forecast[2].weather[0].icon, d2WeatherIco);

            // For Day 3
            day3Lbl.Text = UnixTimeStampToDateTime(forecast[3].dt).ToString("ddd");
            d3condLbl.Text = forecast[3].weather[0].main;
            d3TempLbl.Text = Convert.ToInt32(forecast[3].temp.day).ToString() + "°C";
            LoadIcon(forecast[3].weather[0].icon, d3WeatherIco);

            // For Day 4
            day4Lbl.Text = UnixTimeStampToDateTime(forecast[4].dt).ToString("ddd");
            d4condLbl.Text = forecast[4].weather[0].main;
            d4TempLbl.Text = Convert.ToInt32(forecast[4].temp.day).ToString() + "°C";
            LoadIcon(forecast[4].weather[0].icon, d4WeatherIco);

            // For Day 5
            day5Lbl.Text = UnixTimeStampToDateTime(forecast[5].dt).ToString("ddd");
            d5condLbl.Text = forecast[5].weather[0].main;
            d5TempLbl.Text = Convert.ToInt32(forecast[5].temp.day).ToString() + "°C";
            LoadIcon(forecast[5].weather[0].icon, d5WeatherIco);

            // updating last updated time
            updatedTimeLbl.Text = DateTime.Now.ToString();
            panel1.Show();
            panel2.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel2.Hide();
            string response;
            Root weather_report;
            string lon = longitudeTxt.Text;
            string lat = latitudeTxt.Text;
            if (lat == "" || lon == "")
                return;
            defLabel.Text = "Loading...";
            response = await Task.Run(() => SendRequestAsync(lat, lon, "hourly,minutely").Result);
            weather_report = JsonSerializer.Deserialize<Root>(response);
            SetupWeather(weather_report);
            defLabel.Text = "No Weather Data";
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start(); //started watcher
            GeoCoordinate coord = watcher.Position.Location;
            if (!watcher.Position.Location.IsUnknown)
            {
                double lat = coord.Latitude; //latitude
                double lon = coord.Longitude;  //logitude
                string response = await Task.Run(() => SendRequestAsync(lat.ToString(), lon.ToString(), "hourly,minutely").Result);
                Root weather_report = JsonSerializer.Deserialize<Root>(response);
                SetupWeather(weather_report);
            }
            else
            {
                panel1.Hide();
                panel2.Hide();
            }
            InitCitiesComboBox();
        }

        private void InitCitiesComboBox()
        {
            foreach(City city in cities){
                citiesCb.Items.Add(city.name);
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void d3TempLbl_Click(object sender, EventArgs e)
        {

        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (citiesCb.SelectedIndex >= 0)
            {
                City city = SearchCity(citiesCb.SelectedItem.ToString());
                latitudeTxt.Text = city.latitude;
                longitudeTxt.Text = city.longitude;
            }
        }
    }
}
