/****************************************************
  文件：UIDataInEditor.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UISys
{
    //自定义枚举（包含支持的4种响应）
    public enum CustomUIType
    {
        String,
        Float,
        Int,
        Bool
    }

    //在PanelProfile的KeyValues使用，用于存储初始化数据
    [Serializable]
    [InlineProperty(LabelWidth = 70)]
    public class UIItem
    {
        [FoldoutGroup("$title")]
        [InfoBox("$verifyInfo")]
        [OnValueChanged("ModifyTitle")]
        [OnValueChanged("VerifyValue")]
        [LabelText("类型")]
        public CustomUIType type = CustomUIType.String; //类型

        [FoldoutGroup("$title")]
        [LabelText("键")]
        [Delayed, Required]
        [OnValueChanged("ModifyTitle")]
        public string key = ""; //键

        [FoldoutGroup("$title")]
        [OnValueChanged("ModifyPath")]
        [OnValueChanged("ModifyTitle")]
        [OnValueChanged("VerifyValue")]
        [LabelText("值")]
        [FolderPath]
        [Tooltip("提供路径支持，但输入未必是路径")]
        public string value = ""; //值

        [FoldoutGroup("$title")]
        [LabelText("首次触发")]
        [Tooltip("调用After监听的目的通常是用来渲染整个页面")]
        public bool isFirstInvoke = true;

        [FoldoutGroup("$title")]
        [LabelText("描述信息")]
        public string info = "无"; //描述信息

        #region 辅助变量和函数
        protected string verifyInfo = "欢迎创建UIItem，我是您的智能向导";
        protected string title = "String: - ";

        protected void VerifyValue()
        {
            //局部函数
            void successInput()
            {
                verifyInfo = "合法输入：您的输入值为: " + value;
            }

            void failInput(string rightType)
            {
                verifyInfo = "Value值不合法，请输入" + rightType;
                value = "";
            }

            switch (type)
            {
                case CustomUIType.String:
                    successInput();
                    break;
                case CustomUIType.Float:
                    float result1;
                    if (!float.TryParse(value, out result1))
                    {
                        failInput("浮点数");
                    }
                    else
                    {
                        successInput();
                    }
                    break;
                case CustomUIType.Int:
                    int result2;
                    if (!int.TryParse(value, out result2))
                    {
                        failInput("整数");
                    }
                    else
                    {
                        successInput();
                    }
                    break;
                case CustomUIType.Bool:
                    bool result3;
                    if (!bool.TryParse(value, out result3))
                    {
                        failInput("布尔值(true或false)");
                    }
                    else successInput();
                    break;
            }
        }

        public void ModifyTitle()
        {
            title = type + ": " + key + "- " + value;

            if (title.Length > 26)
            {
                title = title.Substring(0, 26) + "...";
            }
        }

        protected void ModifyPath()
        {
            if (value.Contains("Resources"))
            {
                value = value.Substring(value.IndexOf("Resources/") + 10); //提取Resources加载路径
            }
        }

        #endregion
    }
}