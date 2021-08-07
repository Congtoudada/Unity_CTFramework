/****************************************************
  文件：DebugMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/29 21:54:25
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class DebugMgr
    {
        private static ICTDebug loader;

        static DebugMgr() {
            DebugMgr.loader = new UnityDebugLoader(); //默认给UnityDebugLoader
        }
        
        private DebugMgr() { }

        //设置DebugLoader
        public static void SetDebugLoader(ICTDebug loader)
        {
            DebugMgr.loader = loader;
        }

        public static void Log(string content, SystemEnum sys = SystemEnum.Default)
        {
            loader.Log(content, sys);
        }

        public static void Warning(string content, SystemEnum sys = SystemEnum.Default)
        {
            loader.Warning(content, sys);
        }

        public static void Error(string content, SystemEnum sys = SystemEnum.Default)
        {
            loader.Error(content, sys);
        }

        public static void Log(string content, string prefix)
        {
            loader.Log(content, prefix);
        }

        public static void Warning(string content, string prefix)
        {
            loader.Warning(content, prefix);
        }

        public static void Error(string content, string prefix)
        {
            loader.Error(content, prefix);
        }
    }
}
