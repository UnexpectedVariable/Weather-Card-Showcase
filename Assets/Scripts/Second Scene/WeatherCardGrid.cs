﻿using Assets.Scripts.Util;
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

        private void Start()
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
        public async void Serialize()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("[");
            string[] cardJsons = new string[_weatherCards.Length];
            for(int i = 0; i < _weatherCards.Length; i++)
            {
                cardJsons[i] = JsonConvert.SerializeObject(_weatherCards[i].Serialize(), Formatting.Indented);
            }
            json.AppendJoin(",\n", cardJsons);
            json.AppendLine("\n]");
            Debug.Log(json.ToString());
            await SaveService.SaveAsync(json.ToString());
        }

        [ContextMenu("Deserialize")]
        public async void Deserialize()
        {
            var task = SaveService.LoadAsync();
            if(task == null) return;

            var json = await task;
            var s_Cards = JsonConvert.DeserializeObject<SerializableWeatherCard[]>(json);
            var zippedCards = _weatherCards.Zip(s_Cards, (m_card, s_card) => (m_card, s_card));
            foreach(var pair in zippedCards)
            {
                pair.m_card.Copy(pair.s_card);
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