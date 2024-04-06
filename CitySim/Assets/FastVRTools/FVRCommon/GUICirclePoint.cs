using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SIP.Common
{
    class GUICirclePoint
    {
        Texture2D circle_tex = new Texture2D(1, 2, TextureFormat.ARGB32, true);

        public GUICirclePoint()
        {
            circle_tex.SetPixel(0, 0, new Color(1, 1, 1, 0));
            circle_tex.Apply();
        }

        public void SetCircleColor(Color color)
        {
            circle_tex.SetPixel(0, 0, color);
            circle_tex.Apply();
        }

        public void DrawCicle(int center_x, int center_y, int radian)
        {
            int x = 0;
            int y = radian;
            float d = 1.25f - radian;
            CirclePoint(center_x, center_y, x, y);

            while (x <= y)
            {
                if (d < 0)
                {
                    d += 2 * x + 3;
                }
                else
                {
                    d += 2 * (x - y) + 5;
                    y--;
                }
                x++;
                CirclePoint(center_x, center_y, x, y);
            }
        }

        public void DrawCenterCicle(int center_x, int center_y, int radian)
        {
            GUI.DrawTexture(new Rect(center_x, center_y, 1, 1), circle_tex);
            DrawCicle(center_x, center_y, radian);
        }

        private void CirclePoint(int center_x, int center_y, int x, int y)
        {
            Rect rect;
            rect = new Rect(center_x + x, center_y + y, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x + y, center_y + x, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x + x, center_y - y, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x + y, center_y - x, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x - x, center_y - y, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x - y, center_y - x, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x - x, center_y + y, 1, 1);
            GUI.DrawTexture(rect, circle_tex);

            rect = new Rect(center_x - y, center_y + x, 1, 1);
            GUI.DrawTexture(rect, circle_tex);
        }
    }
}
