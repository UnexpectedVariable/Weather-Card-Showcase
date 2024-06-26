﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    internal struct s_RectTransform
    {
        public UnityEngine.Vector2 SizeDelta { get; set; }
        public UnityEngine.Vector3 Position { get; set; }

        public s_RectTransform(RectTransform rectTransform)
        {
            SizeDelta = rectTransform.sizeDelta;
            Position = rectTransform.position;
        }

        public void Copy(RectTransform source)
        {
            SizeDelta = source.sizeDelta;
            Position = source.position;
        }

        public void Copy(s_RectTransform source)
        {
            SizeDelta = source.SizeDelta;
            Position = source.Position;
        }
    }
}
