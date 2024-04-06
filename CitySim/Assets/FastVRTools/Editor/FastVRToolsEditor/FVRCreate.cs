using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using SIP.FastVRTools;

namespace SIP.FastVRToolsEditor
{
    [ExecuteInEditMode]

    public class FVRCreate  // : ScriptableObject
    {
        [MenuItem("SIP Tools/FastVRTool/Create FastVR Root")]
        static void CreateFVRRoot()
        {
            // Create Fast VR Root [3/26/2014 leo]
            GameObject root = new GameObject("FVRRoot");
            root.AddComponent<FVRRoot>();
            root.AddComponent<FVRManagerBase>();
            Transform transform = root.transform;

            // Create Scene Manager [3/26/2014 leo]
            GameObject sceneManager = new GameObject("SceneManager");
            sceneManager.transform.parent = root.transform;

            GameObject uiManager = new GameObject("UIManager");
            uiManager.transform.parent = root.transform;

            GameObject cameraManager = new GameObject("CameraManager");
            cameraManager.transform.parent = root.transform;
            cameraManager.AddComponent<FVRCameraManager>();

//             foreach (Transform transform in transforms)
//             {
//                 GameObject newChild = new GameObject("_Child");
//                 newChild.transform.parent = transform;
//             }
        }  
    }
}
