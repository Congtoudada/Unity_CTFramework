/****************************************************
  文件：IFileLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/29 12:44:59
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.FileSys
{
    public interface IFileLoader
    {
        //将字符串转换为对象
        T GetObj<T>(string content);

        //将对象转字符串
        string GetString<T>(T obj);
    }
}
