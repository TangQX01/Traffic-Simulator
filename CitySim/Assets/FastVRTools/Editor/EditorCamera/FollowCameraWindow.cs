using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SIP.FastVRTools.Cameras
{
    #if UNITY_EDITOR
        class FollowCameraWindow : Editor
        {
            Editor proxy;
            //Lan m_language = Lan.Chinese;
            //public FollowCameraWindow(Editor proxy)
            //{
            //    this.proxy = proxy;
            //}
            public override void OnInspectorGUI()
            {
                GUILayout.Label("相机属性");
            }
        }
    #endif
}
