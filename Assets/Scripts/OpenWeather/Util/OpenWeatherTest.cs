using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OpenWeather.Util
{
    internal class OpenWeatherTest : MonoBehaviour
    {
        private Client client;

        private void Start()
        {
            client = new Client();
        }

        [ContextMenu("Test")]
        public void Test()
        {
            StartCoroutine(TestAsync());
        }

        IEnumerator TestAsync()
        {
            var data = client.GetAsync("Ukraine");
            while (!data.IsCompleted)
            {
                Debug.Log($"Test task not complete:\n" +
                    $"Status: {data.Status}");
                yield return new WaitForSeconds(1f);
            }
            Debug.Log($"Test task complete:\n" +
                $"Result : {data.Result}");
        }
    }
}
