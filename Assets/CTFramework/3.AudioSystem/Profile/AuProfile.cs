/****************************************************
  文件：AuProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 21:24:32
  功能：
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AuSys
{
    public enum VolumeType
    {
        BG, //背景音
        Env, //环境音
        Effect //音效音
    }

    [CreateAssetMenu(menuName = "CT/AuProfile", fileName = "AuProfile", order = 45)]
    public class AuProfile : ScriptableObject
    {
        [Title("背景音量")]
        [Range(0, 1f)]
        public float bgVolume = 1.0f;
        
        [Title("环境音量")]
        [Range(0, 1f)]
        public float envVolume = 1.0f;

        [Title("音效音量")]
        [Range(0, 1f)]
        public float effectVolume = 1.0f;

        [Title("是否启用AssetBundle加载面板配置文件")]
        public bool isABLoad = false;

        [DisableIf("isABLoad")]
        [TabGroup("Resources")]
        [Title("AuGroup配置文件存放目录(Resources)")]
        [FolderPath]
        [OnValueChanged("ModifyPath")]
        public string profileDir = "CTFramework/Example/AuSys/";

        [EnableIf("isABLoad")]
        [TabGroup("AssetBundle")]
        [Title("AuGroup配置文件存放的ab包")]
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