using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Second_Scene
{
    internal class HorizontalScrollView : MonoBehaviour
    {
        [SerializeField]
        private List<RectTransform> _cardRects = null;
        [SerializeField]
        private (bool initialized, WeatherCard card)[] _chosenCards = null;
        [SerializeField]
        private SwipeDetector _swipeDetector = null;

        private s_RectTransform[] _cardRectAnchors = null;
        private s_RectTransform[] _rectAnchorsBuffer = null;
        private Color[] _cardColors = null;

        private int _lastTarget = 0;

        private void Start()
        {
            InitializeSwipeDetector();
            InitializeWeatherCards();
            InitializeStructs();
        }

        private void InitializeSwipeDetector()
        {
            if (_swipeDetector == null) _swipeDetector = GetComponent<SwipeDetector>();
            _swipeDetector.OnSwipe += Swipe;
            _swipeDetector.OnSwipeFinished += FinalizeSwipe;
        }

        private void InitializeStructs()
        {
            _cardRectAnchors = new s_RectTransform[_cardRects.Count];
            _rectAnchorsBuffer = new s_RectTransform[_cardRects.Count];
            _cardColors = new Color[_cardRects.Count];
            for (int i = 0; i < _cardRectAnchors.Length; i++)
            {
                _cardRectAnchors[i].Copy(_cardRects[i]);
                _rectAnchorsBuffer[i].Copy(_cardRects[i]);
                _cardColors[i] = _chosenCards[i].card.BackgroundColor;
            }
        }

        private void InitializeWeatherCards()
        {
            WeatherCard[] cards = GetComponentsInChildren<WeatherCard>();
            _chosenCards = new (bool initialized, WeatherCard card)[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                _chosenCards[i] = (false, cards[i]);
            }

            if (_cardRects.Count == cards.Length) return;
            _cardRects.Capacity = cards.Length;
            for (int i = 0; i < cards.Length; i++)
            {
                _cardRects.Add(cards[i].GetComponent<RectTransform>());
            }
        }

        private void Swipe(object sender, Vector2 direction)
        {
            var target = direction.x > 0 ? 1 : 2;
            var step = 0.05f;
            var threshold = 5;
            ValidateDirection(target);

            for (int i = 0; i < _cardRects.Count; i++)
            {
                var targetIdx = (i + target) % _cardRects.Count;
                _cardRects[i].Lerp(
                    new s_RectTransform(_cardRects[i]),
                    _rectAnchorsBuffer[targetIdx],
                    step);
                _chosenCards[i].card.BackgroundColor = Color.Lerp(
                    _chosenCards[i].card.BackgroundColor,
                    _cardColors[targetIdx],
                    step);
            }

            if ((_cardRects[0].position - _rectAnchorsBuffer[target].Position).sqrMagnitude < Math.Pow(threshold, 2))
            {
                ShiftAnchors(target);
                ShiftColors(target);
            }

            SortCardHierarchy();
        }

        private void SortCardHierarchy()
        {
            var cardsSorted = _cardRects.OrderByDescending((rect) => rect.sizeDelta.sqrMagnitude).ToList();
            for (int i = 0; i < cardsSorted.Count; i++)
            {
                var card = cardsSorted[i];
                card.SetSiblingIndex(i);
            }
        }

        private void ValidateDirection(int target)
        {
            if(_lastTarget == 0)
            {
                _lastTarget = target;
                return;
            }
            if (_lastTarget != target)
            {
                _lastTarget = target;
                ShiftAnchors(target, true);
                ShiftColors(target, true);
            }
        }

        private void ShiftAnchors(int target, bool flip = false)
        {
            Debug.Log("Shifting anchors buffer");
            if(flip) target = target == 1 ? 2 : 1;
            var rectBuff = _rectAnchorsBuffer[0];

            _rectAnchorsBuffer[0].Copy(_rectAnchorsBuffer[target]);
            _rectAnchorsBuffer[target].Copy(_rectAnchorsBuffer[_rectAnchorsBuffer.Length - target]);
            _rectAnchorsBuffer[_rectAnchorsBuffer.Length - target].Copy(rectBuff);
        }

        private void ShiftColors(int target, bool flip = false)
        {
            Debug.Log("Shifting colors");
            if (flip) target = target == 1 ? 2 : 1;
            var colorBuff = _cardColors[0];

            _cardColors[0] = _cardColors[target];
            _cardColors[target] = _cardColors[_cardColors.Length - target];
            _cardColors[_cardColors.Length - target] = colorBuff;
        }

        private async void FinalizeSwipe(object sender, EventArgs e)
        {
            //TODO: make it lerp to closes color instead of last target
            var step = 0.05f;
            var accumulator = step;
            var delay = 0.01f;

            var startingPositions = new s_RectTransform[]
            {
                new s_RectTransform(_cardRects[0]),
                new s_RectTransform(_cardRects[1]),
                new s_RectTransform(_cardRects[2])
            };
            var finishingPositions = new s_RectTransform[_cardRects.Count];
            var finishingColors = new Color[_cardRects.Count];

            for(int i = 0; i < finishingPositions.Length; i++) 
            {
                finishingPositions[i] = _cardRects[i].Min(new s_RectTransform[]
                {
                    _rectAnchorsBuffer[i],
                    _rectAnchorsBuffer[(i + _lastTarget) % _cardRects.Count]
                });
                finishingColors[i] = _chosenCards[i].card.BackgroundColor.ClosestByRGBDistSqr(_cardColors);
            }

            while(accumulator < 1)
            {
                for(int i = 0; i < _cardRects.Count; i++)
                {
                    _cardRects[i].Lerp(
                        startingPositions[i],
                        finishingPositions[i],
                        accumulator);
                    _chosenCards[i].card.BackgroundColor = Color.Lerp(
                        _chosenCards[i].card.BackgroundColor,
                        finishingColors[i],
                        accumulator);
                }
                accumulator += step;
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
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
                card.AttachCardObserver(_chosenCards[idx].card);
            }
        }

        public void OnCardUnfavored(object sender, WeatherCard card)
        {
            for (int i = 0; i < _chosenCards.Length; i++)
            {
                if (!card.ContainsCardObserver(_chosenCards[i].card)) continue;
                card.DetachCardObserver(_chosenCards[i].card);
                _chosenCards[i].initialized = false;
                _chosenCards[i].card.Reset();
                break;
            }
        }

        public void ToggleSwipe()
        {
            _swipeDetector.enabled = !_swipeDetector.enabled;
        }
    }
}
