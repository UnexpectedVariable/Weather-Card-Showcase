using Assets.Scripts.SecondScene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Second_Scene
{
    internal class SettingsWindow : MonoBehaviour
    {
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

        private void Start()
        {
            InitializeButtons();
            InitializeToggles();
            InitiazlieSliders();
        }

        private void InitiazlieSliders()
        {
            if (_musicVolumeSlider == null) _musicVolumeSlider = GetComponentInChildren<Slider>();

            _musicVolumeSlider.onValueChanged.AddListener((value) =>
            {
                StartCoroutine(TransitionSound(value * 0.01f));
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
            });
            _sfxToggle.onValueChanged.AddListener((value) =>
            {
                _clickSFXAudioSource.mute = !value;
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

        private IEnumerator TransitionSound(float target)
        {
            float delta = 0.01f;
            float lerp = 0.1f;
            while (Math.Abs(_bgMusicAudioSource.volume - target) > delta)
            {
                _bgMusicAudioSource.volume = Mathf.Lerp(_bgMusicAudioSource.volume, target, lerp);
                yield return new WaitForSeconds(delta);
            }
            _bgMusicAudioSource.volume = target;
        }
    }
}
