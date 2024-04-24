using Assets.Scripts.OpenWeather.Models;
using Assets.Scripts.OpenWeather.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.Scripts.OpenWeather
{
    internal class Client
    {
        private readonly HttpClient _httpClient;

        public Client()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData> GetAsync(string country)
        {
            WeatherData data = new();
            HttpRequestMessage request = new(HttpMethod.Get, $"{Data.BASE_URI}weather?q={country}&units=metric&appid={Data.API_KEY}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<WeatherData>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new HttpRequestException($"Request failed with code {response.StatusCode}");
            }
            return data;
        }
    }
}
