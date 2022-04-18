/****************************************************
  文件：UIValidation.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/12/4 10:10:42
  功能：
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UISys
{
    public static class UIOperation
    {
        #region 表单验证
        //表单验证（目前仅支持对InputField的内容验证）
        //参数①：验证对象
        //参数②：输出信息
        //返回值：结果；输出参数：成功返回"",失败返回特性信息
        public static bool InputFieldValidate(this ViewComponent self, out string info)
        {
            info = "";
            Type type = self.GetType();
            //获取所有字段
            foreach (FieldInfo field in type.GetFields())
            {   
                object value = field.GetValue(self);
                //仅支持InputField表单验证
                if (value.GetType() == typeof(InputField))
                {
                    InputField input = value as InputField;
                    if (!InputFieldValidate(field, input, out info)) //有一个表单验证不通过，则不通过
                    {
                        return false;
                    }
                }
            }
            //所有字段均通过检查，返回true
            return true;
        }

        private static bool InputFieldValidate(FieldInfo field, InputField input, out string info)
        {
            info = "";
            //遍历字段的所有特性
            foreach (Attribute attr in field.GetCustomAttributes(false))
            {
                //非空检查
                UIRequiredAttribute require = attr as UIRequiredAttribute;
                if (require != null)
                {
                    if (!RequiredValidate(input.text))
                    {
                        info = require.Info;
                        return false;
                    }
                }

                //正则检查
                UIRegexAttribute regex = attr as UIRegexAttribute;
                if (regex != null)
                {
                    if (!RegexValidate(input.text, regex.Regex))
                    {
                        info = regex.Info;
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool RequiredValidate(object value)
        {
            if (value == null)
                return false;
            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                    return false;
            }
            return true;
        }

        private static bool RegexValidate(object value, string regex)
        {
            string content = value as string;
            if (content != null)
            {
                return Regex.IsMatch(content, regex);
            }
            return true;
        }
        #endregion
        
        #region 自动绑定Key-Values
        //自动绑定
        //参数②：根据外界调用时机，决定内部工作机制
        //  - 如果为true，则会直接初始化值并绑定监听（建议在OnInit中调用）
        //  - 如果为false，则仅初始化值，初始化过程中关闭触发事件（建议在OnEnter中调用）
        public static void AutoBindUI(this ViewComponent self, bool isOnInit = true)
        {
            foreach(FieldInfo field in self.GetType().GetFields())
            {
                if (field.IsDefined(typeof(UIAutoBindAttribute)))
                {
                    //获取变量字段
                    object obj = field.GetValue(self);
                    //获取变量字段的UIAutoBind特性
                    var attr = field.GetCustomAttribute(typeof(UIAutoBindAttribute)) as UIAutoBindAttribute;
                    //自动绑定组件
                    self.BindComponent(obj, attr.Key, attr.DefaultValue, isOnInit);
                }
            }
        }
        //绑定监听
        private static void BindComponent(this ViewComponent self, object obj, string key, string value, bool isOnInit)
        {
            Type type = obj.GetType();
            //Text类型自动绑定
            if (type == typeof(Text))
            {
                Text comp = obj as Text;
                //绑定Key-Values
                if (isOnInit)
                {
                    self.controller.SetString(key, value);
                    self.controller.AddStringAfterListener(key, v => comp.text = v);
                }
                else
                {
                    self.controller.SetString(key, value, false).isTrigger = true;
                }
            }
            //TextMeshProUGUI类型自动绑定
            else if (type == typeof(TextMeshProUGUI))
            {
                TextMeshProUGUI comp = obj as TextMeshProUGUI;
                //绑定Key-Values
                if (isOnInit)
                {
                    self.controller.SetString(key, value);
                    self.controller.AddStringAfterListener(key, v => comp.text = v);
                }
                else
                {
                    self.controller.SetString(key, value, false).isTrigger = true;
                }
            }
            //Slider类型自动绑定
            else if (type == typeof(Slider))
            {
                Slider comp = obj as Slider;
                //绑定Key-Values
                if (isOnInit)
                {
                    self.controller.SetFloat(key, Convert.ToSingle(value));
                    self.controller.AddFloatAfterListener(key, v => comp.value = v);
                }
                else
                {
                    self.controller.SetString(key, value, false).isTrigger = true;
                }
            }
            //Toggle类型自动绑定
            else if (type == typeof(Toggle))
            {
                Toggle comp = obj as Toggle;
                //绑定Key-Values
                if (isOnInit)
                {
                    self.controller.SetBool(key, Convert.ToBoolean(value));
                    self.controller.AddBoolAfterListener(key, v => comp.isOn = v);
                }
                else
                {
                    self.controller.SetString(key, value, false).isTrigger = true;
                }
            }
            //Toggle类型自动绑定
            else if (type == typeof(Image))
            {
                Image comp = obj as Image;
                //绑定Key-Values
                if (isOnInit)
                {
                    self.controller.SetString(key, value);
                    self.controller.AddStringAfterListener(key,
                        v => comp.sprite = Resources.Load<Sprite>(v));
                }
                else
                {
                    self.controller.SetString(key, value, false).isTrigger = true;
                }
            }
            else
            {
                CTLogger.Log("未识别的类型，无法完成自动绑定: " + type.Name);
            }
        }
        #endregion
    }
    
}