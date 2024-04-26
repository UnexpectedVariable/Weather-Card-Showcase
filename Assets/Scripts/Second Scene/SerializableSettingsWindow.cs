using Assets.Scripts.Util.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Second_Scene
{
    internal class SerializableSettingsWindow : ISerialization
    {
        public bool IsMusicOn;
        public bool IsSFXOn;
        public float MusicVolume;

        public SerializableSettingsWindow() { }
        public SerializableSettingsWindow(SettingsWindow source)
        {
            IsMusicOn = source.IsMusicOn;
            IsSFXOn = source.IsSFXOn;
            MusicVolume = source.MusicVolume;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
