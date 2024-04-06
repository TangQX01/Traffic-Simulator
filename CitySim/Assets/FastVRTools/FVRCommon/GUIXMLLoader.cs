using System;
using System.Collections.Generic;
using System.IO;
//using System.Xml;
using UnityEngine;
using UnityEditor;

namespace SIP.Common
{
    public class GUIXMLLoader
    {
        //public static XmlDocument m_xmlDoc = new XmlDocument();

        public GUIXMLLoader()
        {

        }

        // public static void LoadGUIXML(string xmlPath, string xmlTag, ref List<XMLEle> xmlEle)
        // {
        //     if (File.Exists(xmlPath))
        //     {
        //         m_xmlDoc.Load(xmlPath);

        //         XmlNode root = m_xmlDoc.SelectSingleNode(xmlTag);
        //         LoadXMLNode(root, false, ref xmlEle);
        //     }
        // }

//         public static void LoadXMLNode(XmlNode xn, bool editorGUIlayout, ref List<XMLEle> xmlEle)
//         {
//             XmlNodeList rootList = xn.ChildNodes;
//             foreach (XmlNode xnItem in rootList)
//             {
//                 XmlElement xeItem = (XmlElement)xnItem;
//                 string type = xeItem.GetAttribute("Type");
//                 if (type == "EditorGUILayout")
//                 {
//                     bool value = Convert.ToBoolean(xeItem.GetAttribute("Value"));
//                     LoadXMLNode(xnItem, value, ref xmlEle);
//                 }
//                 else
//                 {
//                     XMLEle xe = new XMLEle();
//                     xe.m_layout = editorGUIlayout;
//                     xe.m_type = type;
// 
//                     string posX = xeItem.GetAttribute("PosX");
//                     string posY = xeItem.GetAttribute("PosY");
//                     string width = xeItem.GetAttribute("Width");
//                     string height = xeItem.GetAttribute("Height");
//                     if (posX != "" || posY != "" || width != "" || height != "")
//                     {
//                         xe.m_position = new Rect(Convert.ToInt32(posX), Convert.ToInt32(posY), Convert.ToInt32(width), Convert.ToInt32(height));
//                     }
// 
//                     string label = xeItem.GetAttribute("Label");
//                     if (label != null)
//                     {
//                         xe.m_content = new GUIContent();
//                         xe.m_content.text = label;
//                     }
// 
//                     xe.m_variableName = xeItem.GetAttribute("VariableName");
// 
//                     switch (type)
//                     {
//                         #region Common Field
// 
//                         case "BoundsField":
//                             {
// //                                 string centerX = xeItem.GetAttribute("CenterX");
// //                                 string centerY = xeItem.GetAttribute("CenterY");
// //                                 string centerZ = xeItem.GetAttribute("CenterZ");
// //                                 string sizeX = xeItem.GetAttribute("SizeX");
// //                                 string sizeY = xeItem.GetAttribute("SizeY");
// //                                 string sizeZ = xeItem.GetAttribute("SizeZ");
// //                                 Vector3 center = new Vector3(float.Parse(centerX), float.Parse(centerY), float.Parse(centerZ));
// //                                 Vector3 size = new Vector3(float.Parse(sizeX), float.Parse(sizeY), float.Parse(sizeZ));
// //                                 xe.m_bounds = new Bounds(center, size);
//                                 break;
//                             }
//                         case "ColorField":
//                             {
// //                                 string[] color = xeItem.GetAttribute("Color").Split(',');
// //                                 if (color.Length == 3)
// //                                 {
// //                                     xe.m_color = new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]));
// //                                 }
// //                                 else if (color.Length == 4)
// //                                 {
// //                                     xe.m_color = new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]), float.Parse(color[3]));
// //                                 }
//                                 break;
//                             }
//                         case "CurveField":
//                             {
// //                                 string[] color = xeItem.GetAttribute("Color").Split(',');
// //                                 if (color.Length == 3)
// //                                 {
// //                                     xe.m_color = new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]));
// //                                 }
// //                                 else if (color.Length == 4)
// //                                 {
// //                                     xe.m_color = new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]), float.Parse(color[3]));
// //                                 }
//                                 break;
//                             }
//                         case "EnumMaskField":
//                             {
//                                 break;
//                             }
//                         case "EnumPopup":
//                             {
//                                 break;
//                             }
//                         case "FloatField":
//                             {
//                                 break;
//                             }
//                         case "Foldout":
//                             {
// //                                 string value = xeItem.GetAttribute("Value");
// //                                 xe.m_value = Convert.ToBoolean(value);
//                                 break;
//                             }
//                         case "HelpBox":
//                             {
//                                 xe.m_value = xeItem.GetAttribute("Value");
//                                 break;
//                             }
//                         case "InspectorTitlebar":
//                             {
//                                 break;
//                             }
//                         case "IntField":
//                             {
//                                 break;
//                             }
//                         case "IntPopup":
//                             {
//                                 break;
//                             }
//                         case "IntSlider":
//                             {
//                                 break;
//                             }
//                         case "LabelField":
//                             {
//                                 break;
//                             }
//                         case "LayerField":
//                             {
//                                 break;
//                             }
//                         case "MaskField":
//                             {
//                                 break;
//                             }
//                         case "MinMaxSlider":
//                             {
//                                 break;
//                             }
//                         case "ObjectField":
//                             {
//                                 xe.m_allowSceneObjects = Convert.ToBoolean(xeItem.GetAttribute("AllowSceneObjects"));
//                                 break;
//                             }
//                         case "PasswordField":
//                             {
//                                 xe.m_value = xeItem.GetAttribute("Value");
//                                 break;
//                             }
//                         case "Popup":
//                             {
//                                 break;
//                             }
//                         case "PrefixLabel":
//                             {
//                                 break;
//                             }
//                         case "PropertyField":
//                             {
//                                 break;
//                             }
//                         case "RectField":
//                             {
//                                 break;
//                             }
//                         case "SelectableLabel":
//                             {
//                                 break;
//                             }
//                         case "Slider":
//                             {
//                                 break;
//                             }
//                         case "TagField":
//                             {
//                                 break;
//                             }
//                         case "TextArea":
//                             {
//                                 break;
//                             }
//                         case "TextField":
//                             {
//                                 xe.m_value = xeItem.GetAttribute("Value");
//                                 break;
//                             }
//                         case "Toggle":
//                             {
//                                 break;
//                             }
//                         case "Vector2Field":
//                             {
//                                 break;
//                             }
//                         case "Vector3Field":
//                             {
//                                 break;
//                             }
//                         case "Vector4Field":
//                             {
//                                 break;
//                             }
// 
//                         #endregion
// 
//                         #region EditorGUI Field
// 
//                         case "BeginChangeCheck":
//                             {
//                                 break;
//                             }
//                         case "BeginDisabledGroup":
//                             {
//                                 break;
//                             }
//                         case "BeginProperty":
//                             {
//                                 break;
//                             }
//                         case "DoPasswordField":
//                             {
//                                 break;
//                             }
//                         case "DrawPreviewTexture":
//                             {
//                                 break;
//                             }
//                         case "DrawTextureAlpha":
//                             {
//                                 break;
//                             }
//                         case "DropShadowLabel":
//                             {
//                                 break;
//                             }
//                         case "EndChangeCheck":
//                             {
//                                 break;
//                             }
//                         case "EndDisabledGroup":
//                             {
//                                 break;
//                             }
//                         case "EndProperty":
//                             {
//                                 EditorGUI.EndProperty();
//                                 break;
//                             }
//                         case "GetPropertyHeight":
//                             {
//                                 break;
//                             }
//                         case "IndentedRect":
//                             {
//                                 break;
//                             }
//                         case "ProgressBar":
//                             {
//                                 break;
//                             }
// 
//                         #endregion
// 
//                         #region EditorGUiLayout Field
// 
//                         case "BeginHorizontal":
//                             {
//                                 break;
//                             }
//                         case "BeginScrollView":
//                             {
//                                 break;
//                             }
//                         case "BeginToggleGroup":
//                             {
//                                 break;
//                             }
//                         case "BeginVertical":
//                             {
//                                 break;
//                             }
//                         case "EndHorizontal":
//                             {
//                                 break;
//                             }
//                         case "EndScrollView":
//                             {
//                                 break;
//                             }
//                         case "EndToggleGroup":
//                             {
//                                 break;
//                             }
//                         case "EndVertical":
//                             {
//                                 break;
//                             }
//                         case "Separator":
//                             {
//                                 break;
//                             }
// 
//                         #endregion
// 
//                     }
// 
//                     xmlEle.Add(xe);
//                 }
//                 
//             }
//         }

        // According GUI draw stuffs, draw gui. [4/4/2014 leo]
        // public static void XMLDraw(XMLEle element)
        // {
        //     if (element == null)
        //         return;

        //     if (element.m_layout == false)
        //     {
        //         #region EditorGUI Options

        //         if (element.m_position == null)
        //         {
        //             Debug.Log("XMLDraw: Element Position is null! Please set element position or using EditorGUI Layout option!");
        //             return;
        //         }
        //         switch (element.m_type)
        //         {
        //             case "BeginChangeCheck":
        //                 {
        //                     EditorGUI.BeginChangeCheck();
        //                     break;
        //                 }
        //             case "BeginDisabledGroup":
        //                 {
        //                     EditorGUI.BeginDisabledGroup((bool)element.m_value);
        //                     break;
        //                 }
        //             case "BeginProperty":
        //                 {
        //                     element.m_content = EditorGUI.BeginProperty(element.m_position, element.m_content, element.m_seProperty);
        //                     break;
        //                 }
        //             case "BoundsField":
        //                 {
        //                     if (element.m_content == null)
        //                     {
        //                         element.m_bounds = EditorGUI.BoundsField(element.m_position, element.m_bounds);
        //                     }
        //                     else
        //                     {
        //                         element.m_bounds = EditorGUI.BoundsField(element.m_position, element.m_content, element.m_bounds);
        //                     }
        //                     break;
        //                 }
        //             case "ColorField":
        //                 {
        //                     if (element.m_content == null)
        //                     {
        //                         element.m_color = EditorGUI.ColorField(element.m_position, element.m_color);
        //                     }
        //                     else
        //                     {
        //                         element.m_color = EditorGUI.ColorField(element.m_position, element.m_content, element.m_color);
        //                     }
        //                     break;
        //                 }
        //             case "CurveField":
        //                 {
        //                     if (element.m_curve == null && element.m_seProperty!= null)
        //                     {
        //                         EditorGUI.CurveField(element.m_position, element.m_seProperty, element.m_color, element.m_ranges);
        //                     }
        //                     else if (element.m_color != null && element.m_ranges != null & element.m_content != null)
        //                     {
        //                         element.m_curve = EditorGUI.CurveField(element.m_position, element.m_content, element.m_curve, element.m_color, element.m_ranges);
        //                     }
        //                     else if (element.m_content != null)
        //                     {
        //                         element.m_curve = EditorGUI.CurveField(element.m_position, element.m_content, element.m_curve);
        //                     }
        //                     else
        //                     {
        //                         element.m_curve = EditorGUI.CurveField(element.m_position, element.m_curve);
        //                     }
        //                     break;
        //                 }
        //             case "DoPasswordField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.DoPasswordField(element.m_passId, element.m_position, element.m_content, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.DoPasswordField(element.m_passId, element.m_position, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     break;
        //                 }
        //             case "DrawPreviewTexture":
        //                 {
        //                     if (element.m_mat != null && element.m_scaleMode != ScaleMode.ScaleToFit && element.m_imageAspect != null)
        //                     {
        //                         EditorGUI.DrawPreviewTexture(element.m_position, element.m_image, element.m_mat, element.m_scaleMode, (float)element.m_imageAspect);
        //                     }
        //                     else if (element.m_mat != null && element.m_scaleMode != ScaleMode.ScaleToFit)
        //                     {
        //                         EditorGUI.DrawPreviewTexture(element.m_position, element.m_image, element.m_mat, element.m_scaleMode);
        //                     }
        //                     else if (element.m_mat != null)
        //                     {
        //                         EditorGUI.DrawPreviewTexture(element.m_position, element.m_image, element.m_mat);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.DrawPreviewTexture(element.m_position, element.m_image);
        //                     }
        //                     break;
        //                 }
        //             case "DrawTextureAlpha":
        //                 {
        //                     if (element.m_scaleMode != ScaleMode.ScaleToFit && element.m_imageAspect != null)
        //                     {
        //                         EditorGUI.DrawTextureAlpha(element.m_position, element.m_image, element.m_scaleMode, (float)element.m_imageAspect);
        //                     }
        //                     else if (element.m_scaleMode != ScaleMode.ScaleToFit)
        //                     {
        //                         EditorGUI.DrawTextureAlpha(element.m_position, element.m_image, element.m_scaleMode);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.DrawTextureAlpha(element.m_position, element.m_image);
        //                     }
        //                     break;
        //                 }
        //             case "DropShadowLabel":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         EditorGUI.DropShadowLabel(element.m_position, element.m_content, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.DropShadowLabel(element.m_position, element.m_content);
        //                     }
        //                     break;
        //                 }
        //             case "EndChangeCheck":
        //                 {
        //                     element.m_value = EditorGUI.EndChangeCheck();
        //                     break;
        //                 }
        //             case "EndDisabledGroup":
        //                 {
        //                     EditorGUI.EndDisabledGroup();
        //                     break;
        //                 }
        //             case "EndProperty":
        //                 {
        //                     EditorGUI.EndProperty();
        //                     break;
        //                 }
        //             case "EnumMaskField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumMaskField(element.m_position, element.m_content, element.m_enumValue, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumMaskField(element.m_position, element.m_content, element.m_enumValue);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumMaskField(element.m_position, element.m_enumValue, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumMaskField(element.m_position, element.m_enumValue);
        //                     }
        //                     break;
        //                 }
        //             case "EnumPopup":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumPopup(element.m_position, element.m_content, element.m_enumValue, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumPopup(element.m_position, element.m_content, element.m_enumValue);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumPopup(element.m_position, element.m_enumValue, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_enumValue = EditorGUI.EnumPopup(element.m_position, element.m_enumValue);
        //                     }
        //                     break;
        //                 }
        //             case "FloatField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.FloatField(element.m_position, element.m_content, (float)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.FloatField(element.m_position, element.m_content, (float)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.FloatField(element.m_position, (float)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.FloatField(element.m_position, (float)element.m_value);
        //                     }
        //                     break;
        //                 }
        //             case "Foldout":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null && element.m_toglleOnLabelClick != null)
        //                     {
        //                         element.m_value = EditorGUI.Foldout(element.m_position, (bool)element.m_value, element.m_content, (bool)element.m_toglleOnLabelClick, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle != null && element.m_toglleOnLabelClick == null)
        //                     {
        //                         element.m_value = EditorGUI.Foldout(element.m_position, (bool)element.m_value, element.m_content, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null && element.m_toglleOnLabelClick != null)
        //                     {
        //                         element.m_value = EditorGUI.Foldout(element.m_position, (bool)element.m_value, element.m_content, (bool)element.m_toglleOnLabelClick);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null && element.m_toglleOnLabelClick == null)
        //                     {
        //                         element.m_value = EditorGUI.Foldout(element.m_position, (bool)element.m_value, element.m_content);
        //                     }
        //                     break;
        //                 }
        //             case "GetPropertyHeight":
        //                 {
        //                     if (element.m_content != null && element.m_includeChildren != null)
        //                     {
        //                         element.m_value = EditorGUI.GetPropertyHeight(element.m_seProperty, element.m_content, (bool)element.m_includeChildren);
        //                     }
        //                     else if (element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.GetPropertyHeight(element.m_seProperty, element.m_content);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.GetPropertyHeight(element.m_seProperty);
        //                     }
        //                     break;
        //                 }
        //             case "HelpBox":
        //                 {
        //                     EditorGUI.HelpBox(element.m_position, (string)element.m_value, element.m_messageType);
        //                     break;
        //                 }
        //             case "IndentedRect":
        //                 {
        //                     element.m_position = EditorGUI.IndentedRect(element.m_position);
        //                     break;
        //                 }
        //             case "InspectorTitlebar":
        //                 {
        //                     if(element.m_targetObjs.Length == 1)
        //                     {
        //                         element.m_value = EditorGUI.InspectorTitlebar(element.m_position, (bool)element.m_value, element.m_targetObjs[0]);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.InspectorTitlebar(element.m_position, (bool)element.m_value, element.m_targetObjs);
        //                     }
        //                     break;
        //                 }
        //             case "IntField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.IntField(element.m_position, element.m_content, (int)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.IntField(element.m_position, element.m_content, (int)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.IntField(element.m_position, (int)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.IntField(element.m_position, (int)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "IntPopup":
        //                 {
        //                     if (element.m_seProperty != null && element.m_content != null)
        //                     {
        //                         EditorGUI.IntPopup(element.m_position, element.m_seProperty, element.m_displayedOptions, element.m_optionValues, element.m_content);
        //                     }
        //                     else if (element.m_seProperty != null && element.m_content == null)
        //                     {
        //                         EditorGUI.IntPopup(element.m_position, element.m_seProperty, element.m_displayedOptions, element.m_optionValues);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.IntPopup(element.m_position, element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_guiStyle);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_content == null)
        //                     {
        //                         element.m_value = EditorGUI.IntPopup(element.m_position, (int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_guiStyle);
        //                     }
        //                     else if (element.m_guiStyle == null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.IntPopup(element.m_position, element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_optionValues);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.IntPopup(element.m_position, (int)element.m_value, element.m_displayedOptions, element.m_optionValues);
        //                     }
        //                     break;
        //                 }
        //             case "IntSlider":
        //                 {
        //                     if (element.m_seProperty != null && element.m_content != null)
        //                     {
        //                         EditorGUI.IntSlider(element.m_position, element.m_seProperty, (int)element.m_leftValue, (int)element.m_rightValue, element.m_content);
        //                     }
        //                     else if (element.m_seProperty != null && element.m_content == null)
        //                     {
        //                         EditorGUI.IntSlider(element.m_position, element.m_seProperty, (int)element.m_leftValue, (int)element.m_rightValue);
        //                     }
        //                     else if (element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.IntSlider(element.m_position, element.m_content, (int)element.m_value, (int)element.m_leftValue, (int)element.m_rightValue);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.IntSlider(element.m_position, (int)element.m_value, (int)element.m_leftValue, (int)element.m_rightValue);
        //                     }
        //                     break;
        //                 }
        //             case "LabelField":
        //                 {
        //                     if (element.m_guiStyle != null && element.m_label2 != null)
        //                     {
        //                         EditorGUI.LabelField(element.m_position, element.m_content, element.m_label2, element.m_guiStyle);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_label2 == null)
        //                     {
        //                         EditorGUI.LabelField(element.m_position, element.m_content, element.m_guiStyle);
        //                     }
        //                     else if (element.m_guiStyle == null && element.m_label2 != null)
        //                     {
        //                         EditorGUI.LabelField(element.m_position, element.m_content, element.m_label2);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.LabelField(element.m_position, element.m_content, element.m_guiStyle);
        //                     }
        //                     break;
        //                 }
        //             case "LayerField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.LayerField(element.m_position, element.m_content, (int)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.LayerField(element.m_position, element.m_content, (int)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.LayerField(element.m_position, (int)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.LayerField(element.m_position, (int)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "MaskField":
        //                 {
        //                     string[] displayedOptions;
        //                     if (element.m_displayedOptions != null)
        //                     {
        //                         displayedOptions = new string[element.m_displayedOptions.Length];
        //                         for (int i = 0; i < element.m_displayedOptions.Length; i++)
        //                         {
        //                             displayedOptions[i] = element.m_displayedOptions[i].text;
        //                         }
        //                     }
        //                     else
        //                     {
        //                         Debug.Log("XMLDraw: Displayed Options is null!");
        //                         return;
        //                     }

        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.MaskField(element.m_position, element.m_content, (int)element.m_value, displayedOptions, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.MaskField(element.m_position, element.m_content, (int)element.m_value, displayedOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.MaskField(element.m_position, (int)element.m_value, displayedOptions, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.MaskField(element.m_position, (int)element.m_value, displayedOptions);
        //                     } 

        //                     break;
        //                 }
        //             case "MinMaxSlider":
        //                 {
        //                     if (element.m_content != null)
        //                     {
        //                         EditorGUI.MinMaxSlider(element.m_content, element.m_position, ref element.m_minValue, ref element.m_maxValue, element.m_minLimit, element.m_maxLimit);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.MinMaxSlider(element.m_position, ref element.m_minValue, ref element.m_maxValue, element.m_minLimit, element.m_maxLimit);
        //                     }
        //                     break;
        //                 }
        //             case "ObjectField":
        //                 {
        //                     if (element.m_content != null && element.m_allowSceneObjects != null)
        //                     {
        //                         element.m_obj = EditorGUI.ObjectField(element.m_position, element.m_content, element.m_obj, element.m_objType, (bool)element.m_allowSceneObjects);
        //                     }
        //                     else if (element.m_content != null && element.m_allowSceneObjects == null)
        //                     {
        //                         //element.m_obj = EditorGUI.ObjectField(element.m_position, element.m_content, element.m_obj, element.m_objType);
        //                     }
        //                     else if (element.m_content == null && element.m_allowSceneObjects != null)
        //                     {
        //                         element.m_obj = EditorGUI.ObjectField(element.m_position, element.m_obj, element.m_objType, (bool)element.m_allowSceneObjects);
        //                     }
        //                     else
        //                     {
        //                         //element.m_obj = EditorGUI.ObjectField(element.m_position, element.m_obj, element.m_objType);
        //                     }
        //                     break;
        //                 }
        //             case "PasswordField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.PasswordField(element.m_position, element.m_content, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.PasswordField(element.m_position, element.m_content, (string)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.PasswordField(element.m_position, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.PasswordField(element.m_position, (string)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "Popup":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.Popup(element.m_position, element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.Popup(element.m_position, element.m_content, (int)element.m_value, element.m_displayedOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.Popup(element.m_position, (int)element.m_value, element.m_displayedOptions, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.Popup(element.m_position, (int)element.m_value, element.m_displayedOptions);
        //                     } 
        //                     break;
        //                 }
        //             case "PrefixLabel":
        //                 {
        //                     element.m_position = EditorGUI.PrefixLabel(element.m_position, element.m_prefixLabelId, element.m_content);
        //                     break;
        //                 }
        //             case "ProgressBar":
        //                 {
        //                     EditorGUI.ProgressBar(element.m_position, (float)element.m_value, element.m_content.text);
        //                     break;
        //                 }
        //             case "PropertyField":
        //                 {
        //                     if (element.m_includeChildren != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.PropertyField(element.m_position, element.m_seProperty, element.m_content, (bool)element.m_includeChildren);
        //                     }
        //                     else if (element.m_includeChildren != null && element.m_content == null)
        //                     {
        //                         element.m_value = EditorGUI.PropertyField(element.m_position, element.m_seProperty, (bool)element.m_includeChildren);
        //                     }
        //                     else if (element.m_includeChildren != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.PropertyField(element.m_position, element.m_seProperty, element.m_content);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.PropertyField(element.m_position, element.m_seProperty);
        //                     }
        //                     break;
        //                 }
        //             case "RectField":
        //                 {
        //                     if (element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUI.RectField(element.m_position, element.m_content, (Rect)element.m_value);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.RectField(element.m_position, (Rect)element.m_value);
        //                     }
        //                     break;
        //                 }
        //             case "SelectableLabel":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         EditorGUI.SelectableLabel(element.m_position, element.m_content.text, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         EditorGUI.SelectableLabel(element.m_position, element.m_content.text);
        //                     }
        //                     break;
        //                 }
        //             case "Slider":
        //                 {
        //                     if (element.m_content != null && element.m_seProperty != null)
        //                     {
        //                         EditorGUI.Slider(element.m_position, element.m_seProperty, element.m_leftValue, element.m_rightValue, element.m_content);
        //                     }
        //                     else if (element.m_content != null && element.m_seProperty == null)
        //                     {
        //                         element.m_value = EditorGUI.Slider(element.m_position, element.m_content, (float)element.m_value, element.m_leftValue, element.m_rightValue);
        //                     }
        //                     else if (element.m_content == null && element.m_seProperty != null)
        //                     {
        //                         EditorGUI.Slider(element.m_position, element.m_seProperty, element.m_leftValue, element.m_rightValue);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.Slider(element.m_position, (float)element.m_value, element.m_leftValue, element.m_rightValue);
        //                     }
        //                     break;
        //                 }
        //             case "TagField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.TagField(element.m_position, element.m_content, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.TagField(element.m_position, element.m_content, (string)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.TagField(element.m_position, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.TagField(element.m_position, (string)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "TextArea":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.TextArea(element.m_position, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.TextArea(element.m_position, (string)element.m_value);
        //                     }
        //                     break;
        //                 }
        //             case "TextField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.TextField(element.m_position, element.m_content, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.TextField(element.m_position, element.m_content, (string)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.TextField(element.m_position, (string)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.TextField(element.m_position, (string)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "Toggle":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.Toggle(element.m_position, element.m_content, (bool)element.m_value, element.m_guiStyle);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUI.Toggle(element.m_position, element.m_content, (bool)element.m_value);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUI.Toggle(element.m_position, (bool)element.m_value, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUI.Toggle(element.m_position, (bool)element.m_value);
        //                     } 
        //                     break;
        //                 }
        //             case "Vector2Field":
        //                 {
        //                     element.m_value = EditorGUI.Vector2Field(element.m_position, element.m_content.text, (Vector2)element.m_value);
        //                     break;
        //                 }
        //             case "Vector3Field":
        //                 {
        //                     element.m_value = EditorGUI.Vector3Field(element.m_position, element.m_content.text, (Vector3)element.m_value);
        //                     break;
        //                 }
        //             case "Vector4Field":
        //                 {
        //                     element.m_value = EditorGUI.Vector4Field(element.m_position, element.m_content.text, (Vector4)element.m_value);
        //                     break;
        //                 }
        //         }
        //         #endregion
        //     }
        //     else
        //     {
        //         #region EditorGUILayout Options
        //         switch (element.m_type)
        //         {
        //             case "BeginHorizontal":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                         element.m_value = EditorGUILayout.BeginHorizontal(element.m_guiStyle, element.m_layoutOptions);
        //                     else
        //                         element.m_value = EditorGUILayout.BeginHorizontal(element.m_layoutOptions);
        //                     break;
        //                 }
        //             case "BeginScrollView":
        //                 {
        //                     if(element.m_alwaysShowHorizontal != null && element.m_alwaysShowVertical != null && element.m_verticalScrollbar != null && element.m_horizontalScrollbar != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.BeginScrollView((Vector2)element.m_value, (bool)element.m_alwaysShowHorizontal, (bool)element.m_alwaysShowVertical, element.m_horizontalScrollbar, element.m_verticalScrollbar, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_alwaysShowHorizontal != null && element.m_alwaysShowVertical != null && element.m_verticalScrollbar == null && element.m_horizontalScrollbar == null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.BeginScrollView((Vector2)element.m_value, (bool)element.m_alwaysShowHorizontal, (bool)element.m_alwaysShowVertical, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_alwaysShowHorizontal == null && element.m_alwaysShowVertical == null && element.m_verticalScrollbar != null && element.m_horizontalScrollbar != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.BeginScrollView((Vector2)element.m_value, element.m_horizontalScrollbar, element.m_verticalScrollbar, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_alwaysShowHorizontal == null && element.m_alwaysShowVertical == null && element.m_verticalScrollbar == null && element.m_horizontalScrollbar == null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.BeginScrollView((Vector2)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.BeginScrollView((Vector2)element.m_value, element.m_guiStyle);
        //                     }
        //                     break;
        //                 }
        //             case "BeginToggleGroup":
        //                 {
        //                     element.m_value = EditorGUILayout.BeginToggleGroup(element.m_content, (bool)element.m_value);
        //                     break;
        //                 }
        //             case "BeginVertical":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.BeginVertical(element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.BeginVertical(element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "BoundsField":
        //                 {
        //                     if (element.m_content == null)
        //                     {
        //                         element.m_bounds = EditorGUILayout.BoundsField(element.m_bounds, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_bounds = EditorGUILayout.BoundsField(element.m_content, element.m_bounds, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "ColorField":
        //                 {
        //                     if (element.m_content == null)
        //                     {
        //                         element.m_color = EditorGUI.ColorField(element.m_position, element.m_color);
        //                     }
        //                     else
        //                     {
        //                         element.m_color = EditorGUI.ColorField(element.m_position, element.m_content, element.m_color);
        //                     }
        //                     break;
        //                 }
        //             case "CurveField":
        //                 {
        //                     if (element.m_content != null && element.m_color != null && element.m_ranges != null)
        //                     {
        //                         element.m_curve = EditorGUILayout.CurveField(element.m_content, element.m_curve, element.m_color, element.m_ranges, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_color != null && element.m_ranges != null)
        //                     {
        //                         element.m_curve = EditorGUILayout.CurveField(element.m_curve, element.m_color, element.m_ranges, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_color == null && element.m_ranges == null)
        //                     {
        //                         element.m_curve = EditorGUILayout.CurveField(element.m_content, element.m_curve, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_curve = EditorGUILayout.CurveField(element.m_curve, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "EndHorizontal":
        //                 {
        //                     EditorGUILayout.EndHorizontal();
        //                     break;
        //                 }
        //             case "EndScrollView":
        //                 {
        //                     EditorGUILayout.EndScrollView();
        //                     break;
        //                 }
        //             case "EndToggleGroup":
        //                 {
        //                     EditorGUILayout.EndToggleGroup();
        //                     break;
        //                 }
        //             case "EndVertical":
        //                 {
        //                     EditorGUILayout.EndVertical();
        //                     break;
        //                 }
        //             case "EnumMaskField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumMaskField(element.m_content, element.m_enumValue, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumMaskField(element.m_content, element.m_enumValue, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumMaskField(element.m_enumValue, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumMaskField(element.m_enumValue, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "EnumPopup":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumPopup(element.m_content, element.m_enumValue, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumPopup(element.m_content, element.m_enumValue, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumPopup(element.m_enumValue, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_enumValue = EditorGUILayout.EnumPopup(element.m_enumValue, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "FloatField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.FloatField(element.m_content, (float)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.FloatField(element.m_content, (float)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.FloatField((float)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.FloatField((float)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "Foldout":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.Foldout((bool)element.m_value, element.m_content, element.m_guiStyle);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.Foldout((bool)element.m_value, element.m_content);
        //                     }
        //                     break;
        //                 }
        //             case "HelpBox":
        //                 {
        //                     EditorGUILayout.HelpBox((string)element.m_value, element.m_messageType);
        //                     break;
        //                 }
        //             case "InspectorTitlebar":
        //                 {
        //                     if (element.m_targetObjs.Length == 1)
        //                     {
        //                         element.m_value = EditorGUILayout.InspectorTitlebar((bool)element.m_value, element.m_targetObjs[0]);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.InspectorTitlebar((bool)element.m_value, element.m_targetObjs);
        //                     }
        //                     break;
        //                 }
        //             case "IntField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntField(element.m_content, (int)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntField(element.m_content, (int)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntField((int)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.IntField((int)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "IntPopup":
        //                 {
        //                     if (element.m_seProperty != null && element.m_content != null)
        //                     {
        //                         EditorGUILayout.IntPopup(element.m_seProperty, element.m_displayedOptions, element.m_optionValues, element.m_content, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_seProperty != null && element.m_content == null)
        //                     {
        //                         EditorGUILayout.IntPopup(element.m_seProperty, element.m_displayedOptions, element.m_optionValues, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntPopup(element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_content == null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntPopup((int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_guiStyle == null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntPopup((int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.IntPopup((int)element.m_value, element.m_displayedOptions, element.m_optionValues, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "IntSlider":
        //                 {
        //                     if (element.m_seProperty != null && element.m_content != null)
        //                     {
        //                         EditorGUILayout.IntSlider(element.m_seProperty, (int)element.m_leftValue, (int)element.m_rightValue, element.m_content, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_seProperty != null && element.m_content == null)
        //                     {
        //                         EditorGUILayout.IntSlider(element.m_seProperty, (int)element.m_leftValue, (int)element.m_rightValue, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.IntSlider(element.m_content, (int)element.m_value, (int)element.m_leftValue, (int)element.m_rightValue, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.IntSlider((int)element.m_value, (int)element.m_leftValue, (int)element.m_rightValue, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "LabelField":
        //                 {
        //                     if (element.m_guiStyle != null && element.m_label2 != null)
        //                     {
        //                         EditorGUILayout.LabelField(element.m_content, element.m_label2, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_guiStyle != null && element.m_label2 == null)
        //                     {
        //                         EditorGUILayout.LabelField(element.m_content, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_guiStyle == null && element.m_label2 != null)
        //                     {
        //                         EditorGUILayout.LabelField(element.m_content, element.m_label2, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
                                
        //                         EditorGUILayout.LabelField(element.m_content, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "LayerField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.LayerField(element.m_content, (int)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.LayerField(element.m_content, (int)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.LayerField((int)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.LayerField((int)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "MaskField":
        //                 {
        //                     string[] displayedOptions;
        //                     if (element.m_displayedOptions != null)
        //                     {
        //                         displayedOptions = new string[element.m_displayedOptions.Length];
        //                         for (int i = 0; i < element.m_displayedOptions.Length; i++)
        //                         {
        //                             displayedOptions[i] = element.m_displayedOptions[i].text;
        //                         }
        //                     }
        //                     else
        //                     {
        //                         Debug.Log("XMLDraw: Displayed Options is null!");
        //                         return;
        //                     }

        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.MaskField(element.m_content, (int)element.m_value, displayedOptions, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.MaskField(element.m_content, (int)element.m_value, displayedOptions, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.MaskField((int)element.m_value, displayedOptions, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.MaskField((int)element.m_value, displayedOptions, element.m_layoutOptions);
        //                     }

        //                     break;
        //                 }
        //             case "MinMaxSlider":
        //                 {
        //                     if (element.m_content != null)
        //                     {
        //                         EditorGUILayout.MinMaxSlider(element.m_content, ref element.m_minValue, ref element.m_maxValue, element.m_minLimit, element.m_maxLimit, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         EditorGUILayout.MinMaxSlider(ref element.m_minValue, ref element.m_maxValue, element.m_minLimit, element.m_maxLimit, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "ObjectField":
        //                 {
        //                     if (element.m_content != null && element.m_allowSceneObjects != null)
        //                     {
        //                         element.m_obj = EditorGUILayout.ObjectField(element.m_content, element.m_obj, element.m_objType, (bool)element.m_allowSceneObjects, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_allowSceneObjects == null)
        //                     {
        //                         element.m_obj = EditorGUILayout.ObjectField(element.m_content, element.m_obj, element.m_objType, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_allowSceneObjects != null)
        //                     {
        //                         element.m_obj = EditorGUILayout.ObjectField(element.m_obj, element.m_objType, (bool)element.m_allowSceneObjects, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_obj = EditorGUILayout.ObjectField(element.m_obj, element.m_objType, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "PasswordField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.PasswordField(element.m_content, (string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.PasswordField(element.m_content, (string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.PasswordField((string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.PasswordField((string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "Popup":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.Popup(element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.Popup(element.m_content, (int)element.m_value, element.m_displayedOptions, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.Popup((int)element.m_value, element.m_displayedOptions, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.Popup((int)element.m_value, element.m_displayedOptions, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "PrefixLabel":
        //                 {
        //                     if(element.m_guiStyle != null && element.m_followingStyle != null)
        //                     {
        //                         EditorGUILayout.PrefixLabel(element.m_content, element.m_followingStyle, element.m_guiStyle);
        //                     }
        //                     else if(element.m_guiStyle == null && element.m_followingStyle != null)
        //                     {
        //                         EditorGUILayout.PrefixLabel(element.m_content, element.m_followingStyle);
        //                     }
        //                     else
        //                     {
        //                         EditorGUILayout.PrefixLabel(element.m_content);
        //                     }
                            
        //                     break;
        //                 }
        //             case "PropertyField":
        //                 {
        //                     if (element.m_includeChildren != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.PropertyField(element.m_seProperty, element.m_content, (bool)element.m_includeChildren, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_includeChildren != null && element.m_content == null)
        //                     {
        //                         element.m_value = EditorGUILayout.PropertyField(element.m_seProperty, (bool)element.m_includeChildren, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_includeChildren != null && element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.PropertyField(element.m_seProperty, element.m_content, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.PropertyField(element.m_seProperty, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "RectField":
        //                 {
        //                     if (element.m_content != null)
        //                     {
        //                         element.m_value = EditorGUILayout.RectField(element.m_content, (Rect)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.RectField((Rect)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "SelectableLabel":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         EditorGUILayout.SelectableLabel(element.m_content.text, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         EditorGUILayout.SelectableLabel(element.m_content.text, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "Separator":
        //                 {
        //                     EditorGUILayout.Separator();
        //                     break;
        //                 }
        //             case "Slider":
        //                 {
        //                     if (element.m_content != null && element.m_seProperty != null)
        //                     {
        //                         EditorGUILayout.Slider(element.m_seProperty, element.m_leftValue, element.m_rightValue, element.m_content, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_seProperty == null)
        //                     {
        //                         element.m_value = EditorGUILayout.Slider(element.m_content, (float)element.m_value, element.m_leftValue, element.m_rightValue, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_seProperty != null)
        //                     {
        //                         EditorGUILayout.Slider(element.m_seProperty, element.m_leftValue, element.m_rightValue, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.Slider((float)element.m_value, element.m_leftValue, element.m_rightValue);
        //                     }
        //                     break;
        //                 }
        //             case "Space":
        //                 {
        //                     EditorGUILayout.Space();
        //                     break;
        //                 }
        //             case "TagField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.TagField(element.m_content, (string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.TagField(element.m_content, (string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.TagField((string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.TagField((string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "TextArea":
        //                 {
        //                     if (element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.TextArea((string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.TextArea((string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "TextField":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.TextField(element.m_content, (string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.TextField(element.m_content, (string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.TextField((string)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.TextField((string)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "Toggle":
        //                 {
        //                     if (element.m_content != null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.Toggle(element.m_content, (bool)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content != null && element.m_guiStyle == null)
        //                     {
        //                         element.m_value = EditorGUILayout.Toggle(element.m_content, (bool)element.m_value, element.m_layoutOptions);
        //                     }
        //                     else if (element.m_content == null && element.m_guiStyle != null)
        //                     {
        //                         element.m_value = EditorGUILayout.Toggle((bool)element.m_value, element.m_guiStyle, element.m_layoutOptions);
        //                     }
        //                     else
        //                     {
        //                         element.m_value = EditorGUILayout.Toggle((bool)element.m_value, element.m_layoutOptions);
        //                     }
        //                     break;
        //                 }
        //             case "Vector2Field":
        //                 {
        //                     element.m_value = EditorGUILayout.Vector2Field(element.m_content.text, (Vector2)element.m_value);
        //                     break;
        //                 }
        //             case "Vector3Field":
        //                 {
        //                     element.m_value = EditorGUILayout.Vector3Field(element.m_content.text, (Vector3)element.m_value);
        //                     break;
        //                 }
        //             case "Vector4Field":
        //                 {
        //                     element.m_value = EditorGUILayout.Vector4Field(element.m_content.text, (Vector4)element.m_value);
        //                     break;
        //                 }
        //         }
        //         #endregion
        //     }
        // }
    }
}
