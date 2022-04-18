/****************************************************
  文件：ABUtilityEditor.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 15:52:08
  功能：Nothing
*****************************************************/
using CT.ResSys;
using CT.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace CT.Editor
{
    public class ABUtilityEditor
    {
        [BoxGroup("路径参数")]
        [Title("源路径")]
        [FolderPath, EditorCache, Required]
        public string sourcePath;

        [BoxGroup("路径参数")]
        [Title("原路径和输出路径是否相同")]
        [EditorCache]
        public bool isSamePath = true;

        [HideIf("isSamePath")]
        [BoxGroup("路径参数")]
        [Title("目标路径")]
        [FolderPath, EditorCache]
        public string outputPath;

        private EditorDict dict;

        public ABUtilityEditor(EditorDict dict)
        {
            this.dict = dict;
            dict.InitFields(this);
        }

        //生成资源清单，用于进行AssetBundle的校验
        [Button("生成资源清单")]
        public void CreateVersionList()
        {
            if (EditorVerify.RequiredCheck(this, out string info))
            {
                if (this.CreateVersionList_Loader())
                    dict.SaveFields(this);
            }
            else
            {
                Debug.LogWarning(info);
            }
        }
    }
}
