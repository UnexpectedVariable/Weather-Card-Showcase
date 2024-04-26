using Assets.Scripts.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Second_Scene.WeatherCard;

namespace Assets.Scripts.Second_Scene
{
    internal class MainContent : MonoBehaviour
    {
        [SerializeField]
        private SettingsWindow _settingsView = null;

        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Button _settingsButton = null;
        [SerializeField]
        private Button _resetButton = null;

        [SerializeField]
        private HorizontalScrollView _horizontalScrollView = null;
        [SerializeField]
        private WeatherCardGrid _weatherCardGrid = null;

        [SerializeField]
        private AudioSource _clickSFX = null;

        private void Start()
        {
            InitializeButtons();
            InitializeViews();
        }

        private void InitializeViews()
        {
            if (_horizontalScrollView == null) _horizontalScrollView = GetComponentInChildren<HorizontalScrollView>();
            if (_weatherCardGrid == null) _weatherCardGrid = GetComponentInChildren<WeatherCardGrid>();
            if(_settingsView == null) _settingsView = GetComponentInChildren<SettingsWindow>();

            _weatherCardGrid.CardFavored += _horizontalScrollView.OnCardFavored;
            _weatherCardGrid.CardUnfavored += _horizontalScrollView.OnCardUnfavored;

            _settingsView.Initialize();
        }

        private void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>();

            foreach (var button in buttons)
            {
                button.onClick.AddListener(() =>
                {
                    _clickSFX.Play();
                });
            }

            if (_closeButton == null) _closeButton = Array.Find(buttons,
                (button) => button.name == "CloseButton");
            if (_settingsButton == null) _settingsButton = Array.Find(buttons,
                (button) => button.name == "SettingsButton");
            if (_resetButton == null) _resetButton = Array.Find(buttons,
                (button) => button.name == "ResetButton");

            _closeButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            _settingsButton.onClick.AddListener(() =>
            {
                _settingsView.gameObject.SetActive(true);
            });
            _resetButton.onClick.AddListener(() =>
            {
                _weatherCardGrid.Reset();
            });
        }

        [ContextMenu("Serialize")]
        public async void Serialize()
        {
            string weatherJson = _weatherCardGrid.Serialize();
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"\"WeatherCards\":\n{weatherJson},");
            json.AppendLine($"\"Settings\":\n{_settingsView.Serialize().ToJson()}");
            json.AppendLine("}");
            Debug.Log(json.ToString());
            await SaveService.SaveAsync(json.ToString());
        }

        [ContextMenu("Deserialize")]
        public async void Deserialize()
        {
            var task = SaveService.LoadAsync();
            if (task == null) return;

            var json = await task;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            _weatherCardGrid.Deserialize(dict["WeatherCards"].ToString());
            var settingsSerialization = JsonConvert.DeserializeObject<SerializableSettingsWindow>(dict["Settings"].ToString());
            _settingsView.Copy(settingsSerialization);
        }
    }
}
