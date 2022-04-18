/****************************************************
  文件：AutoBindAttribute.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/12/4 20:53:13
  功能：
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    //执行时机：由用户自己决定（建议放在OnInit内）
    //目前支持Text, TextMeshProUGUI, Slider, Toggle, Image
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UIAutoBindAttribute : Attribute 
    {
        public string Key; //绑定的数据Key
        public string DefaultValue; //绑定的数据初始值

        public UIAutoBindAttribute(string Key, string DefaultValue = "") {
            this.Key = Key;
            this.DefaultValue = DefaultValue;
        }
    }
    
    //必填项
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UIRequiredAttribute : Attribute
    {
        public string Info { get; set; }

        public UIRequiredAttribute(string Info = "")
        {
            this.Info = Info;
        }
    }

    //正则表达式验证
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UIRegexAttribute : Attribute
    {
        public string Regex { get; set; }
        public string Info { get; set; }
        public UIRegexAttribute(string Regex, string Info = "")
        {
            this.Regex = Regex;
            this.Info = Info;
        }
    }
}