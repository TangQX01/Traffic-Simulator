using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SIP.FVRCommonEditor
{
    public enum XMLSTYLE
    {
        UNITY_GUI,
        UNITY_GUILayout,
        UNITY_EditorGUI,
        UNITY_EditorGUILayout,
    }
    public sealed class XMLEle
    {
        // Common Field For Each Element [4/4/2014 leo]
        #region Common Field
        public object m_value;
        public string m_variableName;
        public GUIContent m_content;
        public string m_type;
        public Rect m_position;
        public GUIStyle m_guiStyle;
        public Color m_color;
        public bool m_isShow = true;
        //public XMLSTYLE m_xmlStyle = XMLSTYLE.UNITY_EditorGUI;
        public XMLSTYLE m_editorGUILayout;
        #endregion

        #region EditorGUI option field
        // Property Variables [4/4/2014 leo]
        public SerializedProperty m_seProperty;

        // Bounds Field Variables [4/8/2014 leo]
        public Bounds m_bounds;

        // Animation Curve Variables [4/8/2014 leo]
        public AnimationCurve m_curve;
        public Rect m_ranges;

        // Do password field variables [4/8/2014 leo]
        public int m_passId = 0;

        // Draw preview texture variables [4/8/2014 leo]
        public Texture m_image;
        public Material m_mat;
        public ScaleMode m_scaleMode;
        public object m_imageAspect;

        // Enum mask field variables [4/8/2014 leo]
        public Enum m_enumValue;

        // Fold out variables [4/8/2014 leo]
        public object m_toglleOnLabelClick;

        // Get proerty height [4/8/2014 leo]
        public object m_includeChildren;

        // Message type [4/8/2014 leo]
        public MessageType m_messageType;

        // Inspector Titlebar [4/8/2014 leo]
        public UnityEngine.Object[] m_targetObjs;

        // Int popup variables [4/8/2014 leo]
        public GUIContent[] m_displayedOptions;
        public int[] m_optionValues;

        // Int slider variables [4/8/2014 leo]
        public float m_leftValue;
        public float m_rightValue;

        // Lable field variables [4/8/2014 leo]
        public GUIContent m_label2;

        // Min max slider [4/8/2014 leo]
        public float m_minValue;
        public float m_maxValue;
        public float m_minLimit;
        public float m_maxLimit;

        // Prefix label variables [4/8/2014 leo]
        public int m_prefixLabelId;

        // Object field type [4/8/2014 leo]
        public UnityEngine.Object m_obj;
        public Type m_objType;
        public object m_allowSceneObjects;

        #endregion

        #region EditorGUILayout option field
        // Editor GUI Layout Option [4/8/2014 leo]
        public GUILayoutOption[] m_layoutOptions;

        // Scroll view variables [4/8/2014 leo]
        public GUIStyle m_horizontalScrollbar;
        public GUIStyle m_verticalScrollbar;
        public object m_alwaysShowHorizontal;
        public object m_alwaysShowVertical;

        // Prefix label variables [4/8/2014 leo]
        public GUIStyle m_followingStyle;
        #endregion

        public XMLEle()
        {

        }
    }
}
