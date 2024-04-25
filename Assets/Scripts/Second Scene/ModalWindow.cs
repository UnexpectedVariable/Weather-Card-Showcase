using Assets.Scripts.Util;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Assets.Scripts.SecondScene
{
    internal class ModalWindow : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton = null;
        [SerializeField] 
        private Button _linkButton = null;
        [SerializeField]
        private GameObject _container = null;
        [SerializeField]
        private CanvasGroup _group = null;

        private Tween _fadeTween = null;

        private void Start()
        {
            if (_container == null) _container = gameObject;
            if (_group == null) _group = GetComponent<CanvasGroup>();

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            var buttons = GetComponentsInChildren<Button>();

            if (_closeButton == null) _closeButton = Array.Find(buttons,
                (button) => button.name == "CloseButton");
            if (_linkButton == null) _linkButton = Array.Find(buttons,
                (button) => button.name == "LinkButton");

            _closeButton.onClick.AddListener(() =>
            {
                Fade(0, 0.5f, () =>
                {
                    gameObject.SetActive(false);
                });
            });
            _linkButton.onClick.AddListener(() =>
            {
                Application.OpenURL(Data.LINKEDIN_URL);
            });
        }

        public void Fade(float target, float duration, TweenCallback onEnd)
        {
            if (_fadeTween != null) _fadeTween.Kill();

            _fadeTween = _group.DOFade(target, duration);
            _fadeTween.onComplete = onEnd;
        }
    }
}