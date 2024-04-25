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
    internal class WeatherCard : MonoBehaviour, IObserved<WeatherCard>, Util.Observer.IObserver<WeatherCard>, ISerializable<SerializableWeatherCard>
    {
        protected List<Util.Observer.IObserver<WeatherCard>> _observers = new();

        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Toggle _favoriteToggle = null;
        [SerializeField]
        private TMP_InputField _location = null;
        [SerializeField]
        private Image _weatherIcon = null;
        [SerializeField]
        private TextMeshProUGUI _temperature = null;

        private Client _weatherClient = null;

        public string Location => _location.text;
        public string Temperature => _temperature.text;
        public bool Favored => _observers.Count > 0;
        public Sprite Icon => _weatherIcon.sprite;

        public Toggle FavoriteToggle => _favoriteToggle;

        private void Awake()
        {
            InitializeButtons();
            if (_favoriteToggle == null) _favoriteToggle = GetComponentInChildren<Toggle>();
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
            var buttons = GetComponentsInChildren<Button>();

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
        }

        private void OnDisable()
        {
            _favoriteToggle?.onValueChanged.Invoke(false);
        }

        #region Observer
        public void Attach(Util.Observer.IObserver<WeatherCard> observer)
        {
            _observers.Add(observer);

            _favoriteToggle.SetIsOnWithoutNotify(true);
        }

        public void Attach(ICollection<Util.Observer.IObserver<WeatherCard>> observers)
        {
            throw new NotImplementedException();
        }

        public void Detach(Util.Observer.IObserver<WeatherCard> observer)
        {
            _observers.Remove(observer);

            _favoriteToggle.SetIsOnWithoutNotify(false);
        }

        public void Detach(ICollection<Util.Observer.IObserver<WeatherCard>> observers)
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            foreach(var observer in _observers)
            {
                observer.Handle(this);
            }
        }

        public bool Contains(Util.Observer.IObserver<WeatherCard> observer)
        {
            if (_observers.Contains(observer)) return true;
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

        public class SerializableWeatherCard : ISerialization
        {
            public string Location;
            public bool Active;
            public bool Favored;

            public SerializableWeatherCard() { }
            public SerializableWeatherCard(WeatherCard weatherCard)
            {
                Location = weatherCard.Location;
                Active = weatherCard.isActiveAndEnabled;
                Favored = weatherCard.Favored;
            }
        }
        #endregion
    }
}
