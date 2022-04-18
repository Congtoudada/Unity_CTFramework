/****************************************************
  文件：DeepCloneTool.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2022/4/18 15:25:01
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CT.Tools
{
  public static class DeepCloneTool
  {
    //深拷贝 无法拷贝事件和委托！！！
    //URL：https://www.cnblogs.com/luoocean/p/10441587.html
    //拷贝测试
    //直接new，手动赋值 10w次：11ms
    //浅拷贝 10w次：19ms
    //反射深拷贝 10w次：4778ms
    //二进制序列化深拷贝 10w次：13424ms 弃用
    public static T DeepCopyByReflection<T>(T obj)
    {
      //如果是string、值类型、Unity委托或事件，使用浅拷贝
      if (obj is string || obj.GetType().IsValueType)
        return obj;
      if (obj.GetType().FullName.Contains("UnityEngine.Events.UnityAction"))
        return obj;

      object retval = Activator.CreateInstance(obj.GetType());
      FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
      foreach (var field in fields)
      {
        try
        {
          field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
        }
        catch { }
      }
      return (T)retval;
    }
  }
}
