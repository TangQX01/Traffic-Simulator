using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SIP.Common;

namespace SIP.FVRCommonEditor
{
    public class FVRLanguageWindow : EditorWindow
    {
        Lan m_language = Lan.Chinese;
        string[] m_selStringsEn = new string[] { "Chinese", "English" };
        string[] m_selStringsCn = new string[] { "中文", "英文" };
        public static string m_languageMenu = "SIP Tools/Settings/Language";

        public FVRLanguageWindow()
        {

        }

        [MenuItem("SIP Tools/Settings/Language")]
        static void LanguageInit()
        {
            FVRLanguageWindow dw = (FVRLanguageWindow)EditorWindow.GetWindow(typeof(FVRLanguageWindow));
            dw.title = "Language Settings";
            return;
        }

        void OnGUI()
        {
            if (EditorPrefs.HasKey("Language"))
                m_language = (Lan)EditorPrefs.GetInt("Language");
            else
                EditorPrefs.SetInt("Language", (int)m_language);

            switch (m_language)
            {
                case Lan.Chinese:
                    {
                        GUILayout.Label("语言设置", EditorStyles.boldLabel);
                        m_language = (Lan)GUILayout.SelectionGrid((int)m_language, m_selStringsCn, 1);
                        EditorPrefs.SetInt("Language", (int)m_language);
                        //m_languageMenu = "FastVR Tools/设置/语言";
                        break;
                    }
                case Lan.English:
                    {
                        GUILayout.Label("Language Settings", EditorStyles.boldLabel);
                        m_language = (Lan)GUILayout.SelectionGrid((int)m_language, m_selStringsEn, 1);
                        EditorPrefs.SetInt("Language", (int)m_language);
                        //m_languageMenu = "FastVR Tools/Settings/Language";
                        break;
                    }
            }
        }

        void OnFocus()
        {
            if (EditorPrefs.HasKey("Language"))
                m_language = (Lan)EditorPrefs.GetInt("Language");
        }

        void OnLostFocus()
        {
            EditorPrefs.SetInt("Language", (int)m_language);
        }

        void OnDestroy()
        {
            EditorPrefs.SetInt("Language", (int)m_language);
        }
    }
}
