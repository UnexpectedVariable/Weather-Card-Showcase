using System;
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
        private Button _closeButton = null;
        [SerializeField]
        private Button _infoButton = null;

        private void Start()
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
            });
        }
    }
}
