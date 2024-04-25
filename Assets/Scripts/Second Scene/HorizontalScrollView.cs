using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Second_Scene
{
    internal class HorizontalScrollView : MonoBehaviour
    {
        [SerializeField]
        private List<RectTransform> _objects = null;
        [SerializeField]
        private (bool initialized, WeatherCard card)[] _chosenCards = null;

        [SerializeField]
        private Button _rightScrollButton = null;
        [SerializeField]
        private Button _leftScrollButton = null;

        private void Start()
        {
            WeatherCard[] cards = GetComponentsInChildren<WeatherCard>();
            _chosenCards = new (bool initialized, WeatherCard card)[cards.Length];
            for(int i = 0; i < cards.Length; i++)
            {
                _chosenCards[i] = (false, cards[i]);
            }

            var buttons = GetComponentsInChildren<Button>();

            if (_rightScrollButton == null) _rightScrollButton = Array.Find(buttons,
                (button) => button.name == "RightScrollButton");
            if (_leftScrollButton == null) _leftScrollButton = Array.Find(buttons,
                (button) => button.name == "LeftScrollButton");

            _rightScrollButton.onClick.AddListener(() =>
            {
                Swipe(1);
            });
            _leftScrollButton.onClick.AddListener(() =>
            {
                Swipe(2);
            });
        }

        private void Swipe(int target)
        {
            target %= _objects.Count;

            var rectBuff = new s_RectTransform(_objects[0]);

            _objects[0].Copy(_objects[target]);
            _objects[target].Copy(_objects[_objects.Count - target]);
            _objects[_objects.Count - target].Copy(rectBuff);
        }

        public bool TryGetSpotIdx(out int idx)
        {
            idx = -1;
            for(int i = 0; i < _chosenCards.Length; i++)
            {
                var pair = _chosenCards[i];
                if (pair.initialized) continue;
                idx = i;
                return true;
            }
            return false;
        }

        public void OnCardFavored(object sender, WeatherCard card)
        {
            if(TryGetSpotIdx(out int idx))
            {
                _chosenCards[idx].card.Copy(card);
                _chosenCards[idx].initialized = true;
                card.Attach(_chosenCards[idx].card);
            }
        }

        public void OnCardUnfavored(object sender, WeatherCard card)
        {
            for (int i = 0; i < _chosenCards.Length; i++)
            {
                if (!card.Contains(_chosenCards[i].card)) continue;
                card.Detach(_chosenCards[i].card);
                _chosenCards[i].initialized = false;
                _chosenCards[i].card.Reset();
                break;
            }
        }
    }
}
