using Assets.Scripts.SecondScene;
using Assets.Scripts.Util;
using Assets.Scripts.Util.JSON;
using Assets.Scripts.Util.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Second_Scene.SettingsWindow;

namespace Assets.Scripts.Second_Scene
{
    internal partial class SettingsWindow : MonoBehaviour, ISerializable<SerializableSettingsWindow>, IInitializable
    {
        private List<Util.Observer.IObserver<SettingsWindow>> _saveObservers = new();

        [SerializeField]
        private GameObject _modalWindow = null;

        [SerializeField]
        private Toggle _musicToggle = null;
        [SerializeField]
        private Toggle _sfxToggle = null;

        [SerializeField]
        private Slider _musicVolumeSlider = null;

        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private Button _infoButton = null;

        [SerializeField]
        private AudioSource _bgMusicAudioSource = null;
        [SerializeField]
        private AudioSource _clickSFXAudioSource = null;

        public bool IsMusicOn => _musicToggle.isOn;
        public bool IsSFXOn => _sfxToggle.isOn;
        public float MusicVolume => _musicVolumeSlider.value;

        public bool IsInitialized
        {
            get;
            private set;
        }

        public event EventHandler OnEnabled = null;
        public event EventHandler OnDisabled = null;

        private void Start()
        {
            Initialize();
        }

        private void InitiazlieSliders()
        {
            if (_musicVolumeSlider == null) _musicVolumeSlider = GetComponentInChildren<Slider>();

            _musicVolumeSlider.onValueChanged.AddListener(async (value) =>
            {
                await TransitionSound(value * 0.01f);
                Notify();
            });
        }

        private void InitializeToggles()
        {
            var toggles = GetComponentsInChildren<Toggle>();

            if (_musicToggle == null) _musicToggle = Array.Find(toggles,
                (button) => button.name == "MusicToggle");
            if (_sfxToggle == null) _sfxToggle = Array.Find(toggles,
                (button) => button.name == "SFXToggle");

            _musicToggle.onValueChanged.AddListener((value) =>
            {
                _bgMusicAudioSource.mute = !value;
                Notify();
            });
            _sfxToggle.onValueChanged.AddListener((value) =>
            {
                _clickSFXAudioSource.mute = !value;
                Notify();
            });
        }

        private void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>();

            if (_closeButton == null) _closeButton = Array.Find(buttons,
                (button) => button.name == "CloseButton");
            if (_infoButton == null) _infoButton = Array.Find(buttons,
                (button) => button.name == "InfoButton");

            _closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
            _infoButton.onClick.AddListener(() =>
            {
                _modalWindow.SetActive(true);
                _modalWindow.GetComponent<ModalWindow>().Fade(1, 0.5f, () => { });
            });
        }

        private async Task TransitionSound(float target)
        {
            float delta = 0.01f;
            float lerp = 0.1f;
            while (Math.Abs(_bgMusicAudioSource.volume - target) > delta)
            {
                _bgMusicAudioSource.volume = Mathf.Lerp(_bgMusicAudioSource.volume, target, lerp);
                await Task.Delay(TimeSpan.FromSeconds(delta));
            }
            _bgMusicAudioSource.volume = target;
        }

        public void Copy(SerializableSettingsWindow source)
        {
            _musicToggle.isOn = source.IsMusicOn;
            _sfxToggle.isOn = source.IsSFXOn;
            _musicVolumeSlider.value = source.MusicVolume;
        }

        public SerializableSettingsWindow Serialize()
        {
            return new SerializableSettingsWindow(this);
        }

        public void Initialize()
        {
            if (IsInitialized) return;

            InitializeButtons();
            InitializeToggles();
            InitiazlieSliders();
        }

        public void AttachSaveObserver(Util.Observer.IObserver<SettingsWindow> observer)
        {
            _saveObservers.Add(observer);
        }

        public void DetachSaveObserver(Util.Observer.IObserver<SettingsWindow> observer)
        {
            _saveObservers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _saveObservers)
            {
                observer.Handle(this);
            }
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke(this, EventArgs.Empty);
        }
    }
}
