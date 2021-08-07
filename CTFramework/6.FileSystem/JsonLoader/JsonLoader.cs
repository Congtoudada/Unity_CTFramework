/****************************************************
  文件：JsonLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:19:13
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.FileSys
{
    public class JsonLoader : IFileLoader
    {
        public T GetObj<T>(string content)
        {
            return JsonUtility.FromJson<T>(content);
        }

        public string GetString<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
