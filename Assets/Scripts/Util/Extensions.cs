using Assets.Scripts.Second_Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Util
{
    internal static class Extensions
    {
        #region RectTransform
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

        public static void Lerp(this RectTransform target, s_RectTransform start, s_RectTransform end, float step)
        {
            target.sizeDelta = Vector2.Lerp(start.SizeDelta, end.SizeDelta, step);
            target.position = Vector3.Lerp(start.Position, end.Position, step);
        }

        public static s_RectTransform Min(this RectTransform target, ICollection<s_RectTransform> transforms)
        {
            var min = transforms.ElementAt(0);
            var minDistance = (target.position - min.Position).sqrMagnitude;

            foreach (var transform in transforms)
            {
                var distance = (target.position - transform.Position).sqrMagnitude;

                if (distance > minDistance) continue;
                min = transform;
                minDistance = distance;
            }

            return min;
        }
        #endregion

        public static Color ClosestByRGBDistSqr(this Color target, ICollection<Color> colors)
        {
            var distancesSqr = colors.Select((color) =>
            {
                return Math.Pow(color.r - target.r, 2) + Math.Pow(color.g - target.g, 2) + Math.Pow(color.b - target.b, 2);
            });
            var minIdx = 0;
            var counter = 0;
            var min = distancesSqr.ElementAt(0);
            foreach(var dist in distancesSqr) 
            {
                if (min > dist)
                {
                    min = dist;
                    minIdx = counter;
                }
                counter++;
            }
            return colors.ElementAt(minIdx);
        }

        public static float GetBrightness(this Color target)
        {
            return (target.r * 0.299f + target.g * 0.587f + target.b * 0.114f) * 0.00390625f;
        }


    }
}
