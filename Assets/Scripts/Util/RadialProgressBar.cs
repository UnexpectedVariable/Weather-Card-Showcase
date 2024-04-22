using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Util
{
    internal class RadialProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Image _image = null;

        private void Start()
        {
            if(_image == null) _image = GetComponent<Image>();
        }
    }
}
