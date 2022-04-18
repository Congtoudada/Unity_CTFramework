/****************************************************
/****************************************************
  文件：CTProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 15:59:35
  功能：负责全局配置
*****************************************************/

using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT
{
    public enum ProfileEnum
    {
        Unknown,
        Res, //持有: ABLoader
        UI, //持有: UIFactory
        Au, //持有: AuFactory, AuLoader
    }
    
    [CreateAssetMenu(menuName = "CT/CTConfig", fileName = "CTConfig", order = 0)]
    public class CTConfig : ScriptableObject
    {
        [Header("全局配置文件重定向路径")]
        [Tooltip("空代表不使用路径重定向")]
        public string OverridePath = "";

        [Header("配置文件Resources加载路径")]
        [FolderPath]
        [OnValueChanged("OnResourceLoadDirChanged")]
        public string ResourceLoadDir = "";

        [Header("是否启用框架DEBUG输出")] public bool isDebug = true;

        [LabelText("框架核心模块")]
        public List<CTItem> list = new List<CTItem>();

        private void OnResourceLoadDirChanged()
        {
            ResourceLoadDir = EditorHelper.GetResourcesPath(ResourceLoadDir);
        }

        [FoldoutGroup("框架辅助模块"), Header("玩家数据")] public bool isPlayerData = true;
        [FoldoutGroup("框架辅助模块"), Header("场景管理类")] public bool isSceneMgr = true;
        [FoldoutGroup("框架辅助模块"), Header("定时回调管理类")] public bool isTimerMgr = true;
    }

    [Serializable]
    public class CTItem
    {
        [FoldoutGroup("$moduleName")]
        [Header("配置文件类型")]
        public ProfileEnum type = ProfileEnum.Unknown;

        [FoldoutGroup("$moduleName")]
        [Header("配置文件的模块名称（用于加载）")]
        [FilePath]
        [OnValueChanged("OnModuleNameChanged")]
        public string moduleName = "";

        [FoldoutGroup("$moduleName")]
        [Header("是否启用全局缓存")]
        public bool isCacheProfile = true;

        private void OnModuleNameChanged()
        {
            moduleName = Path.GetFileNameWithoutExtension(moduleName);
        }

    }
}