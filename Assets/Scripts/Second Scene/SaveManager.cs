using Assets.Scripts.Util.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Second_Scene
{
    internal class SaveManager : MonoBehaviour, Util.Observer.IObserver<WeatherCard>, Util.Observer.IObserver<SettingsWindow>
    {
        public event EventHandler OnSave = null;

        public void Handle(WeatherCard observed)
        {
            Debug.Log($"WeatherCard {observed.name} invoked save manager handle");
            OnSave?.Invoke(this, EventArgs.Empty);
        }

        public void Handle(SettingsWindow observed)
        {
            Debug.Log($"Settings window invoked save manager handle");
            OnSave?.Invoke(this, EventArgs.Empty);
        }
    }
}
