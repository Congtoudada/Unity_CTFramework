/****************************************************
  文件：EditorHelper.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public static class EditorHelper
    {
        //去掉不必要的路径节点，得到Resources路径
        public static string GetResourcesPath(string path)
        {
            if (path.Contains("Resources"))
            {
                return path.Substring(path.IndexOf("Resources/") + 10); //提取Resources加载路径
            }
            else
            {
                return "";
            }
        }
    }
}