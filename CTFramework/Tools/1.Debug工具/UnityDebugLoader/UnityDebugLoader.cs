/****************************************************
  文件：UnityDebugLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:01:26
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class UnityDebugLoader : ICTDebug
    {
        public void Error(string content, SystemEnum sys = SystemEnum.Default)
        {
            Debug.LogError($"[ {sys.ToString()} ] {content}");
        }

        public void Error(string content, string prefix)
        {
            Debug.LogError($"[ {prefix} ] {content}");
        }

        public void Log(string content, SystemEnum sys = SystemEnum.Default)
        {
            Debug.Log($"[ {sys.ToString()} ] {content}");
        }

        public void Log(string content, string prefix)
        {
            Debug.Log($"[ {prefix} ] {content}");
        }

        public void Warning(string content, SystemEnum sys = SystemEnum.Default)
        {
            Debug.LogWarning($"[ {sys.ToString()} ] {content}");
        }

        public void Warning(string content, string prefix)
        {
            Debug.LogWarning($"[ {prefix} ] {content}");
        }
    }
}
