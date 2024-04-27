using Assets.Scripts.Util.JSON;
using Newtonsoft.Json;

namespace Assets.Scripts.Second_Scene
{
    internal partial class WeatherCard
    {
        internal class SerializableWeatherCard : ISerialization
        {
            public string Location;
            public bool Active;
            public bool Favored;

            public SerializableWeatherCard() { }
            public SerializableWeatherCard(WeatherCard source)
            {
                Location = source.Location;
                Active = source.isActiveAndEnabled;
                Favored = source.Favored;
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}
