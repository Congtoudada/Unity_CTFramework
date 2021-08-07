/****************************************************
  文件：UICoreProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 9:52:41
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [CreateAssetMenu(menuName = "CT/UISystem/UICoreProfile", fileName = "UICoreProfile")]
    public class UICoreProfile : SerializedScriptableObject
    {
        [Header("UI预制体默认Resources加载路径")]
        public string prefabPath = CTConstant.UI_PREFAB_DIR_PATH;

        [Header("ModelView默认Resources加载路径")]
        public string modelviewPath = CTConstant.UI_MODELVIEW_DIR_PATH;

#if UNITY_EDITOR
        [BoxGroup("编辑器拓展")]
        [Header("ModelPanel模板加载路径")]
        public string modelPath = "Assets/CTFramework/4.UISystem/UIEditor/ModelPanelTemplate.txt";
        [BoxGroup("编辑器拓展")]
        [Header("ViewComponent模板加载路径")]
        public string viewPath = "Assets/CTFramework/4.UISystem/UIEditor/ViewComponentTemplate.txt";
#endif
    }
}
