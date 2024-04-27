using Assets.Scripts.Util;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Second_Scene.WeatherCard;

namespace Assets.Scripts.Second_Scene
{
    internal class WeatherCardGrid : MonoBehaviour
    {
        [SerializeField]
        private WeatherCard[] _weatherCards = null;

        public event EventHandler<WeatherCard> CardFavored = null;
        public event EventHandler<WeatherCard> CardUnfavored = null;

        private void Awake()
        {
            if (_weatherCards.Length == 0) _weatherCards = GetComponentsInChildren<WeatherCard>(true);

            foreach(var card in _weatherCards)
            {
                card.FavoriteToggle.onValueChanged.AddListener((value) =>
                {
                    if(value) CardFavored?.Invoke(this, card);
                    else CardUnfavored?.Invoke(this, card);

                    card.ValidateChosenIcon();
                });
            }
        }

        [ContextMenu("Serialize")]
        public string Serialize()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("[");
            string[] cardJsons = new string[_weatherCards.Length];
            for(int i = 0; i < _weatherCards.Length; i++)
            {
                cardJsons[i] = _weatherCards[i]
                    .Serialize()
                    .ToJson();
            }
            json.AppendJoin(",\n", cardJsons);
            json.Append("\n]");
            return json.ToString();
        }

        [ContextMenu("Deserialize")]
        public void Deserialize(string json)
        {
            var s_Cards = JsonConvert.DeserializeObject<SerializableWeatherCard[]>(json);
            var zippedCards = _weatherCards.Zip(s_Cards, (m_card, s_card) => (m_card, s_card));
            foreach(var pair in zippedCards)
            {
                pair.m_card.Copy(pair.s_card);
            }
        }

        public void AttachSaveObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            foreach(var card in _weatherCards)
            {
                card.AttachSaveObserver(observer);
            }
        }

        public void Reset()
        {
            foreach(var card in _weatherCards)
            {
                card.Reset();
            }
        }
    }
}
