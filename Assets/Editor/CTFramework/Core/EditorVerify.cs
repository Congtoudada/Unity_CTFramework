/****************************************************
  文件：EditorVerify.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/18 20:47:01
  功能：合法性检查
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Editor
{
    public static class EditorVerify
    {
        //必填项检查
        public static bool RequiredCheck(object obj, out string info)
        {
            info = "";
            Type type = obj.GetType();
            //空字段检查
            object value;
            foreach (var field in type.GetFields())
            {
                //Debug.Log("field: " + field.Name);
                if (field.IsDefined(typeof(RequiredAttribute), false))
                {
                    value = field.GetValue(obj);
                    //obj为null
                    if (value == null)
                    {
                        info = field.Name + " 没有赋值，请检查！";
                        return false;
                    }
                    //value为空串
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(value as string))
                        {
                            info = field.Name + " 没有赋值，请检查！";
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}