/****************************************************
  文件：UIProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/15 21:13:21
  功能：
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.UISys
{
    [CreateAssetMenu(menuName = "CT/PanelProfile", fileName = "PanelProfile", order = 31)]
    public class PanelProfile : SerializedScriptableObject
    {
        [Title("面板游戏对象的预制体名称")]
        public string panelName = "";

        [Title("UI面板是否缓存")]
        [Tooltip("缓存开启后会多次调用OnEnter,OnExit来显示与隐藏，请小心数据的初始化问题")]
        public bool isCache = true;

        [Title("UI面板是否预加载进缓存池，Push前必须先调用AddToCacheTool！")]
        [EnableIf("isCache")]
        public bool isPreLoad = false;

        [Title("是否启用AB包加载面板游戏对象")]
        public bool isAssetBundle;

        [HideIf("isAssetBundle")]
        [Title("UI面板预制体所在目录，勿加资源名(Resources)")]
        [FolderPath]
        [OnValueChanged("ModifyPath")]
        public string resourcesDir = "CTFramework/Example/UISys/";

        [ShowIf("isAssetBundle")]
        [Title("UI面板预制体所在主包(AssetBundle)")]
        public string abName;

        [Title("是否启用预定义KeyValues")]
        [Tooltip("如果启用，将缓存面板配置文件专门用于初始化KeyValues")]
        public bool isCacheProfile = false;

        [ShowIf("isCacheProfile")]
        [Title("ModelView键值对")]
        public List<UIItem> keyValues = new List<UIItem>();


        public string GetResourcesPath()
        {
            return Path.Combine(resourcesDir, panelName);
        }

        private void ModifyPath()
        {
            if (resourcesDir.Contains("Resources"))
            {
                resourcesDir = resourcesDir.Substring(resourcesDir.IndexOf("Resources/") + 10); //提取Resources加载路径
            }
            else
            {
                resourcesDir = "";
            }
        }

        [Button("刷新Key-Values", ButtonSizes.Large)]
        private void RefreshKeyValues()
        {
            foreach(var item in keyValues)
            {
                item.ModifyTitle();
            }
        }

    }
}