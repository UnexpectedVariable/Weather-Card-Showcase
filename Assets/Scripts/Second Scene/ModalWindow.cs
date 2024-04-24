using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SecondScene
{
    internal class ModalWindow : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton = null;
        [SerializeField]
        private GameObject _container = null;

        private void Start()
        {
            if (_closeButton == null) _closeButton = GetComponentInChildren<Button>();
            if (_container == null) _container = gameObject;

            _closeButton.onClick.AddListener(() =>
            {
                _container.SetActive(false);
            });
        }
    }
}