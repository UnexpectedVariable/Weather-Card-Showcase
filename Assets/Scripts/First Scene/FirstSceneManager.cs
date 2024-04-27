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
                StartCoroutine(LoadSceneCoroutine());
                _loadSceneButton.gameObject.SetActive(false);
            });
        }

        IEnumerator LoadSceneCoroutine()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("SecondScene");
            loadSceneAsync.allowSceneActivation = false;

            while (!loadSceneAsync.isDone)
            {
                _radialProgressBar.SetFill(loadSceneAsync.progress);
                if(loadSceneAsync.progress >= 0.9f)
                {
                    _radialProgressBar.SetFill(1f);
                    yield return new WaitForFixedUpdate();
                    loadSceneAsync.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
