/****************************************************
  文件：EditorCache.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 13:45:46
  功能：用于缓存编辑器拓展的数据
    目前支持缓存string，bool
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Editor
{
    //缓存属性
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EditorCacheAttribute : Attribute
    { }

    //缓存字典
    public class EditorDict
    {
         //Key-Value字典
         //Key: 键，命名规范: 类类型_字段名
         public SerializableDictionary<string, string> editorDict;

         public EditorDict()
         {
            editorDict = new SerializableDictionary<string, string>();
         }

        //从硬盘读取数据
        public void ReadData()
        {
            editorDict = editorDict.Read(CTConstant.EDITOR_CACHE_PATH);
            if(editorDict == null)
            {
                editorDict = new SerializableDictionary<string, string>();
                editorDict["version"] = "v1.0";
            }
        }

        //将数据写入硬盘
        public void WriteData()
        {
            editorDict.Write(CTConstant.EDITOR_CACHE_PATH);
            UnityEditor.AssetDatabase.Refresh();
        }

        //初始化字段
        public void InitFields(object obj)
        {
            //Console.WriteLine("获取Editor数据");
            ReadData();
            Type type = obj.GetType();
            foreach (var field in type.GetFields())
            {
                //如果含有EditorCacheAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(EditorCacheAttribute), false))
                {
                    string key = type.Name + "_" + field.Name;
                    if (editorDict.ContainsKey(key)) //存在缓存
                    {
                        if (field.FieldType == typeof(bool)) //如果属性是bool类型
                        {
                            field.SetValue(obj, Convert.ToBoolean(editorDict[type.Name + "_" + field.Name]));
                        }
                        else
                        {
                            field.SetValue(obj, editorDict[type.Name + "_" + field.Name]);
                        }
                    }
                }
            }
        }

        //缓存字段
        public void SaveFields(object obj)
        {
            //Console.WriteLine("保存Editor数据");
            Type type = obj.GetType();
            foreach (var field in type.GetFields())
            {
                //如果含有EditorCacheAttribute，就尝试从字典中读取
                if (field.IsDefined(typeof(EditorCacheAttribute), false))
                {
                    if (editorDict.ContainsKey(type.Name + "_" + field.Name))
                    {
                        editorDict[type.Name + "_" + field.Name] = Convert.ToString(field.GetValue(obj));
                    }
                    else
                    {
                        editorDict.Add(type.Name + "_" + field.Name, Convert.ToString(field.GetValue(obj)));
                    }
                }
            }
            WriteData(); //写入一次硬盘
        }
    }
}
