using Assets.Scripts.Second_Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Util
{
    internal static class Extensions
    {
        public static void Copy(this RectTransform target, RectTransform source)
        {
            target.sizeDelta = source.sizeDelta;

            target.position = source.position;
        }

        public static void Copy(this RectTransform target, s_RectTransform source)
        {
            target.sizeDelta = source.SizeDelta;

            target.position = source.Position;
        }
    }
}
