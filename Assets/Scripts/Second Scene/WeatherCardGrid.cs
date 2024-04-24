using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
    }
}
