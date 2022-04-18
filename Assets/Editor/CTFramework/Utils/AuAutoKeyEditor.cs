/****************************************************
  文件：AuAutoKeyEditor.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/25 15:32:00
  功能：根据AuGroupProfile，自动生成Key的脚本
*****************************************************/
using CT.AuSys;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CT.Editor
{
    public class AuAutoKeyEditor
    {
        [Title("音乐组配置文件")]
        [Required]
        public AuGroupProfile profile;

        [Title("脚本输出目录")]
        [FolderPath, Required, EditorCache]
        public string output;

        private EditorDict dict;

        public AuAutoKeyEditor(EditorDict dict)
        {
            this.dict = dict;
            dict.InitFields(this);
        }

        //一键生成所有Key
        [Button("一键生成所有Key", ButtonSizes.Large)]
        public void CreateScript()
        {
            if (EditorVerify.RequiredCheck(this, out string info))
            {
                if (this.CreateScript_Loader())
                    dict.SaveFields(this);//自带 AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning(info);
            }
        }

    }
}