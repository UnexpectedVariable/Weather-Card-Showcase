using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Second_Scene
{
    internal class MainContent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _settingsContainer = null;

        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Button _settingsButton = null;

        [SerializeField]
        private HorizontalScrollView _horizontalScrollView = null;
        [SerializeField]
        private WeatherCardGrid _weatherCardGrid = null;

        private void Start()
        {
            InitializeButtons();
            InitializeViews();
        }

        private void InitializeViews()
        {
            if (_horizontalScrollView == null) _horizontalScrollView = GetComponentInChildren<HorizontalScrollView>();
            if (_weatherCardGrid == null) _weatherCardGrid = GetComponentInChildren<WeatherCardGrid>();

            _weatherCardGrid.CardFavored += _horizontalScrollView.OnCardFavored;
            _weatherCardGrid.CardUnfavored += _horizontalScrollView.OnCardUnfavored;
        }

        private void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>();

            if (_closeButton == null) _closeButton = Array.Find(buttons,
                (button) => button.name == "CloseButton");
            if (_settingsButton == null) _settingsButton = Array.Find(buttons,
                (button) => button.name == "SettingsButton");

            _closeButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            _settingsButton.onClick.AddListener(() =>
            {
                _settingsContainer.SetActive(true);
            });
        }
    }
}
