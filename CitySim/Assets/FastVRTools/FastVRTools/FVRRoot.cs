using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SIP.FastVRTools
{
    [AddComponentMenu("SIP/FastVRTool/SIP Root")]
    [RequireComponent(typeof(GameObject))]

    public class FVRRoot : MonoBehaviour
    {
        static public List<FVRRoot> s_rootList = new List<FVRRoot>();

        protected virtual void OnEnable() { s_rootList.Add(this); }
        protected virtual void OnDisable() { s_rootList.Remove(this); }

        protected virtual void Start()
        {
        }

        void Update()
        {

        }

        public void BasicKeyShortCut()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        static public void Broadcast(string funcName, object param)
        {
            if (param == null)
            {
                Debug.LogError("FastVRMsg: You are try to broadcast an null object!");
            }
            else
            {
                for (int i = 0, imax = s_rootList.Count; i < imax; ++i)
                {
                    FVRRoot root = s_rootList[i];
                    if (root != null) root.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        static public void Broadcast(string funcName)
        {
            for (int i = 0, imax = s_rootList.Count; i < imax; ++i)
            {
                FVRRoot root = s_rootList[i];
                if (root != null) root.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
            }
        }

    }
}
