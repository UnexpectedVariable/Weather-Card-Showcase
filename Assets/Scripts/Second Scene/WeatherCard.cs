using Assets.Scripts.OpenWeather;
using Assets.Scripts.Util;
using Assets.Scripts.Util.JSON;
using Assets.Scripts.Util.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Second_Scene.WeatherCard;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.Button;
using static UnityEngine.UI.Toggle;

namespace Assets.Scripts.Second_Scene
{
    internal partial class WeatherCard : MonoBehaviour, Util.Observer.IObserver<WeatherCard>, ISerializable<SerializableWeatherCard>
    {
        private List<Util.Observer.IObserver<WeatherCard>> _cardObservers = new();
        private List<Util.Observer.IObserver<WeatherCard>> _saveObservers = new();


        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Toggle _favoriteToggle = null;
        [SerializeField]
        private TMP_InputField _location = null;
        [SerializeField]
        private Image _weatherIcon = null;
        [SerializeField]
        private Image _background = null;
        [SerializeField]
        private TextMeshProUGUI _temperature = null;

        private Client _weatherClient = null;

        public string Location => _location.text;
        public string Temperature => _temperature.text;
        public bool Favored => _cardObservers.Count > 0;
        public Sprite Icon => _weatherIcon.sprite;
        public Color BackgroundColor
        {
            get => _background.color;
            set => _background.color = value;
        }

        public Toggle FavoriteToggle => _favoriteToggle;

        private void Awake()
        {
            InitializeButtons();
            if (_favoriteToggle == null) _favoriteToggle = GetComponentInChildren<Toggle>();
            if (_background == null) _background = GetComponent<Image>();
            InitializeForecastLogic();
        }

        private void InitializeForecastLogic()
        {
            _weatherClient = new Client();

            if (_location == null) _location = GetComponentInChildren<TMP_InputField>();

            _location.onSubmit.AddListener((value) =>
            {
                StartCoroutine(GetWeatherData(value));
            });
        }

        private void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>(true);

            if (_closeButton == null) _closeButton = Array.Find(buttons,
                (button) => button.name == "CloseButton");

            _closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                Notify();
            });
        }

        private IEnumerator GetWeatherData(string location)
        {
            var data = _weatherClient.GetAsync(location);
            while (!data.IsCompleted)
            {
                Debug.Log($"Query for {location} not complete:\n" +
                    $"Status: {data.Status}");
                yield return new WaitForSeconds(0.2f);
            }
            if (data.IsCompletedSuccessfully)
            {
                _temperature.text = $"{(int)data.Result.main.temp}";
                string path = $"Images/{data.Result.weather[0].icon}@2x";
                _weatherIcon.sprite = Resources.Load<Sprite>(path);
                Notify();
            }
        }

        /*private async void GetWeatherData(string location)
        {
            var data = await _weatherClient.GetAsync(location);
            _temperature.text = $"{data.main.temp}";
        }*/

        public void Copy(WeatherCard source)
        {
            _location.text = source.Location;
            _temperature.text = source.Temperature;
            _weatherIcon.sprite = source.Icon;
        }

        public void Copy(SerializableWeatherCard source)
        {
            _location.text = source.Location;
            _location.onSubmit.Invoke(source.Location);
            _favoriteToggle.onValueChanged.Invoke(source.Favored);
            gameObject.SetActive(source.Active);
        }

        public void ValidateChosenIcon()
        {
            if(!Favored)
            {
                _favoriteToggle.graphic.gameObject.SetActive(false);
            }
            else
            {
                _favoriteToggle.graphic.gameObject.SetActive(true);
            }
        }

        public void Reset()
        {
            _location.text = string.Empty;
            _temperature.text = "0";
            _weatherIcon.sprite = Resources.Load<Sprite>(Data.DEFAULT_WEATHER_URI);
            _favoriteToggle?.onValueChanged.Invoke(false);
            gameObject.SetActive(true);
            Notify();
        }

        private void OnDisable()
        {
            _favoriteToggle?.onValueChanged.Invoke(false);
        }

        #region Observer
        public void AttachCardObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            _cardObservers.Add(observer);
            _favoriteToggle.SetIsOnWithoutNotify(true);
            Notify();
        }

        public void AttachSaveObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            _saveObservers.Add(observer);
        }

        public void DetachCardObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            _cardObservers.Remove(observer);
            _favoriteToggle.SetIsOnWithoutNotify(false);
            Notify();
        }

        public void DetachSaveObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            _saveObservers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _cardObservers)
            {
                observer.Handle(this);
            }
            foreach (var observer in _saveObservers)
            {
                observer.Handle(this);
            }
        }

        public bool ContainsCardObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            if (_cardObservers.Contains(observer)) return true;
            return false;
        }

        public bool ContainsSaveObserver(Util.Observer.IObserver<WeatherCard> observer)
        {
            if (_saveObservers.Contains(observer)) return true;
            return false;
        }

        public void Handle(WeatherCard observed)
        {
            Copy(observed);
        }
        #endregion

        #region Serialization
        public SerializableWeatherCard Serialize()
        {
            return new SerializableWeatherCard(this);
        }
        #endregion
    }
}
