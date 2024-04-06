using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SIP.Common
{
    public class GUILine
    {
        Texture2D line_tex = new Texture2D(1, 2, TextureFormat.ARGB32, true);
        public GUILine()
        {
            line_tex.SetPixel(0, 0, new Color(1, 1, 1, 0));
            line_tex.Apply();
        }

        public void SetLineColor(Color color)
        {
            line_tex.SetPixel(0, 0, color);
            line_tex.Apply();
        }

        public void Drawline(Rect rect)
        {
            GUI.DrawTexture(rect, line_tex);
        }
    }
}
