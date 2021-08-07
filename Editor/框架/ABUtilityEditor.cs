/****************************************************
  文件：ABUtilityEditor.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 15:52:08
  功能：Nothing
*****************************************************/
using CT.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace CT.ResourceSys
{
    public class ABUtilityEditor
    {
        [BoxGroup("路径参数")]
        [Title("源路径")]
        [FolderPath]
        public string sourcePath;

        [BoxGroup("group")]
        [Title("目标路径")]
        [FolderPath]
        public string outputPath;

        private EditorDict dict;

        public ABUtilityEditor(EditorDict dict)
        {
            this.dict = dict;
            if (dict.editorDict != null)
            {
                sourcePath = dict.editorDict[GetType() + "_sourcePath"];
                outputPath = dict.editorDict[GetType() + "_outputPath"];
            }
        }

        [Button("生成资源清单")]
        public void CreateVersionList()
        {
            if(!string.IsNullOrEmpty(sourcePath))
            {
                //资源准备
                VersionList versionList = new VersionList();
                DirectoryInfo dir = new DirectoryInfo(sourcePath);
                FileInfo[] files = dir.GetFiles();
                MD5 md5 = MD5.Create();

                //生成清单数据
                foreach (FileInfo file in files)
                {
                    //Debug.Log(file.Name);
                    VersionItem item = new VersionItem();
                    item.fileName = file.Name;

                    string path = Path.Combine(sourcePath, file.Name);
                    string fileMd5 = string.Empty;
                    using (FileStream fs = File.OpenRead(path))
                    {
                        byte[] fileMd5Bytes = md5.ComputeHash(fs);
                        fileMd5 = System.BitConverter.ToString(fileMd5Bytes).Replace("-", "").ToLower();
                    }
                    item.md5 = fileMd5;
                    versionList.updateList.Add(item);
                }

                //转化Json存储
                string json = JsonUtility.ToJson(versionList, true);

                //写到目标路径
                using (StreamWriter sw = new StreamWriter(Path.Combine(outputPath, CTConstant.AB_VERSION_LIST)))
                {
                    sw.Write(json);
                }

                Debug.Log("versionList生成成功: " + outputPath);

                //保存源路径和输出路径，下次方便使用
                if(!string.IsNullOrEmpty(sourcePath) && !string.IsNullOrEmpty(outputPath))
                {
                    dict.editorDict[GetType() + "_sourcePath"] = sourcePath;
                    dict.editorDict[GetType() + "_outputPath"] = outputPath;
                    dict.WriteData();
                }
            }
            else
            {
                Debug.LogWarning("请选择源路径!");
            }
        }
    }
}
