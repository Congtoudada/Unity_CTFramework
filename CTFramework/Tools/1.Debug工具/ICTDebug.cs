/****************************************************
  文件：IDebug.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:01:46
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public interface ICTDebug
    {
        void Log(string content, SystemEnum sys = SystemEnum.Default);
        void Warning(string content, SystemEnum sys = SystemEnum.Default);
        void Error(string content, SystemEnum sys = SystemEnum.Default);

        void Log(string content, string prefix);
        void Warning(string content, string prefix);
        void Error(string content, string prefix);
    }
}
