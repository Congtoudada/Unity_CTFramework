/****************************************************
  文件：PanelProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 10:19:17
  功能：
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [CreateAssetMenu(menuName = "CT/UIProfile", fileName = "UIProfile", order = 30)]
    public class UIProfile : ScriptableObject
    {
        [Title("启用单缓存技术；否则启用缓存池技术")]
        public bool isSingleCache = false;

        [DisableIf("isSingleCache")]
        [Title("UI面板缓存池的缓存上限")]
        public int maxCache = 3;

        [Title("是否启用AssetBundle加载面板配置文件")]
        [Tooltip("若开启该项，请确保资源系统的AB包加载也一同开启")]
        public bool isABLoad = false;

        [DisableIf("isABLoad")]
        [TabGroup("Resources")]
        [Title("UI面板配置文件存放目录(Resources)")]
        [FolderPath]
        [OnValueChanged("ModifyPath")]
        public string profileDir = "CTFramework/Example/UISys/";

        [EnableIf("isABLoad")]
        [TabGroup("AssetBundle")]
        [Title("UI面板配置文件存放的ab包")]
        public string abName = "";

        private void ModifyPath()
        {
            if (profileDir.Contains("Resources"))
            {
                profileDir = profileDir.Substring(profileDir.IndexOf("Resources/") + 10); //提取Resources加载路径
            }
            else
            {
                profileDir = "";
            }
        }
    }
}