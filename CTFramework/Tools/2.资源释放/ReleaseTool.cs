/****************************************************
  文件：ReleaseTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 10:19:35
  功能：帮助释放资源
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class ReleaseTool
    {
        public static void TryRelease(object obj)
        {
            IRelease release = obj as IRelease;
            release?.Release();
        }
    }
}
