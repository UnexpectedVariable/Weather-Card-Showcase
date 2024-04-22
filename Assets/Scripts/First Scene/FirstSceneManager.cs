using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.FirstScene
{
    internal class FirstSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Button _loadSceneButton = null;
        [SerializeField]
        private RadialProgressBar _radialProgressBar = null;

        private void Start()
        {
            _loadSceneButton.onClick.AddListener(() => 
            {
                AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("SecondScene");
            });
        }
    }
}
