using System;
using System.Collections.Generic;
using SIP.FastVRTools;
using SIP.FVRCommonEditor;
using UnityEditor;
using UnityEngine;

namespace SIP.FastVRToolsEditor
{
    public class FVRCamMWindow : Editor
    {
        Editor m_proxy;
        Lan m_language = Lan.Chinese;
        public FVRCameraManager m_fvrCameraManager;
        List<XMLEle> m_xmlElement = new List<XMLEle>();
        public bool m_isLoaded = false;

//         // Window texts [4/4/2014 leo]
// #region Window Texts
//         string m_txtTitle;
//         string m_txtStartCam;
//         string m_txtManualSwitch;
//         string m_txtSwitchTime;
//         string m_txtAutoSwitchNum;
//         string m_txtAutoSwitchCam;
//         string m_txtAutoSwitchTime;
//         string m_txtAutoSwitchDes;
//         string m_txtAutoSwitchPause;
//         string m_txtAutoSwitchReverse;
// #endregion

        //public FVRCamMWindow(Editor proxy)
        //{
        //    this.m_proxy = proxy;
        //}

        public void OnEnable()
        {

        }

        public void OnDisable()
        {
            m_isLoaded = false;
        }

        public override void OnInspectorGUI()
        {
            //Repaint();
//             Event e = Event.current;
//             if (e.control && e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
//             {
//                 e.Use();
//             }
//             if (e.control && e.type == EventType.KeyDown && e.keyCode == KeyCode.Y)
//             {
//                 e.Use();
//             }

            if (EditorPrefs.HasKey("Language"))
            {
                Lan lan = (Lan)EditorPrefs.GetInt("Language");
                if (m_language != lan)
                {
                    m_language = lan;
                    m_isLoaded = false;
                    LoadXML();
                }
            }

            if (!m_isLoaded)
            {
                LoadXML();
                m_isLoaded = true;
            }

            for(int i = 0; i < m_xmlElement.Count; i++)
            {
                if (m_xmlElement[i] != null)
                {
                    switch (m_xmlElement[i].m_variableName)
                    {
                        case "StartCam":
                            {
                                m_xmlElement[i].m_obj = m_fvrCameraManager.transform.GetChild((int)m_xmlElement[i].m_value).GetComponent<Camera>();
                                m_fvrCameraManager.m_startCamera = (Camera)m_xmlElement[i].m_obj;
                                //m_fvrCameraManager.m_currentCamNum = i;
                                //EditorGUILayout.ObjectField(m_xmlElement[i].m_content, m_fvrCameraManager.m_activeCamera, typeof(Camera), true, m_xmlElement[i].m_layoutOptions);
                                break;
                            }
                        case "MaualSwitch":
                            {
                                m_fvrCameraManager.m_enableMaualSwitch = (bool)m_xmlElement[i].m_value;
                                break;
                            }
                        case "SwitchTime":
                            {
                                m_fvrCameraManager.m_switchTime = (float)m_xmlElement[i].m_value;
                                break;
                            }
                    }
                    GUIXMLLoader.XMLDraw(m_xmlElement[i]);
                }
            }



            //EditorGUILayout.FloatField(m_fvrCameraManager.m_switchTime);
//             GUILine line = new GUILine();
//             Rect last_rect = new Rect(0, 0, position.width, 0);
//             last_rect.x = 0;
//             last_rect.width = position.width;
//             last_rect.y = GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height;
//             last_rect.height = 1;
//             Color color = new Color(0.5f, 0.5f, 0.5f, 0);
//             line.SetLineColor(color);
//             line.Drawline(last_rect);
        }

        public void LoadXML()
        {
            m_xmlElement.Clear();
            if (m_language == Lan.Chinese)
            {
                string path = Application.streamingAssetsPath + "/guixml_cn.xml";
                GUIXMLLoader.LoadGUIXML(path, "CameraManager", ref m_xmlElement);
            }
            else if (m_language == Lan.English)
            {
                string path = Application.streamingAssetsPath + "/guixml_cn.xml";
                GUIXMLLoader.LoadGUIXML(path, "CameraManager", ref m_xmlElement);
            }
            m_fvrCameraManager = (FVRCameraManager)m_proxy.target;

            if (m_xmlElement.Count > 0)
            {
                for (int i = 0; i < m_xmlElement.Count; i++)
                {
                    if (m_xmlElement[i] != null)
                    {
                        switch (m_xmlElement[i].m_variableName)
                        {
                            case "StartCam":
                                {
                                    int num = m_fvrCameraManager.transform.childCount;
                                    m_xmlElement[i].m_obj = m_fvrCameraManager.m_startCamera;
                                    m_xmlElement[i].m_optionValues = new int[num];
                                    m_xmlElement[i].m_displayedOptions = new GUIContent[num];
                                    
                                    for (int j = 0; j < num; j++)
                                    {
                                        string name = m_fvrCameraManager.transform.GetChild(j).name;
                                        m_xmlElement[i].m_displayedOptions[j] = new GUIContent();
                                        m_xmlElement[i].m_displayedOptions[j].text = name;
                                        m_xmlElement[i].m_optionValues[j] = j;
                                        if (m_fvrCameraManager.transform.GetChild(j).GetComponent<Camera>() == m_fvrCameraManager.m_startCamera)
                                        {
                                            m_xmlElement[i].m_value = j;
                                        }
                                    }
                                    if (m_fvrCameraManager.m_startCamera == null)
                                    {
                                        m_xmlElement[i].m_value = 0;
                                        m_fvrCameraManager.m_startCamera = m_fvrCameraManager.transform.GetChild(0).GetComponent<Camera>();
                                    }
//                                     m_xmlElement[i].m_objType = typeof(Camera);
//                                     m_xmlElement[i].m_obj = m_fvrCameraManager.m_activeCamera;

                                    break;
                                }
                            case "MaualSwitch":
                                {
                                    m_xmlElement[i].m_value = m_fvrCameraManager.m_enableMaualSwitch;
                                    break;
                                }
                            case "SwitchTime":
                                {
                                    m_xmlElement[i].m_value = m_fvrCameraManager.m_switchTime;
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }
}
