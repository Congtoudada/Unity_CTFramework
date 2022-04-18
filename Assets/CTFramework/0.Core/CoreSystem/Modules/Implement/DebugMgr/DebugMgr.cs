/****************************************************
  文件：DebugMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/29 21:54:25
  功能：框架日志打印
*****************************************************/
#define DEBUG
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CT
{
    public enum SysEnum
    {
        Default,
        CoreSystem,
        ResourceSystem,
        UISystem,
        AudioSystem,
        InputSystem,
        AISystem,
        NetworkSystem,
        Tools
    }

    public class CTLogger
    {
        private static ICTDebug loader;
        public static bool isDebug = true;

        static CTLogger() {
            loader = new UnityDebugLoader(); //默认给UnityDebugLoader
        }
        
        private CTLogger() { }

        //设置DebugLoader
        public static void SetDebugLoader(ICTDebug loader)
        {
            CTLogger.loader = loader;
        }

        [Conditional("DEBUG")]
        public static void Log(string content, SysEnum sys = SysEnum.Default)
        {
            if (isDebug)
                loader.Log(content, sys);
        }

        [Conditional("DEBUG")]
        public static void Warning(string content, SysEnum sys = SysEnum.Default)
        {
            if (isDebug)
                loader.Warning(content, sys);
        }

        [Conditional("DEBUG")]
        public static void Error(string content, SysEnum sys = SysEnum.Default)
        {
            if (isDebug)
                loader.Error(content, sys);
        }
        [Conditional("DEBUG")]
        public static void Log(string content, string prefix)
        {
            if (isDebug)
                loader.Log(content, prefix);
        }

        [Conditional("DEBUG")]
        public static void Warning(string content, string prefix)
        {
            if (isDebug)
                loader.Warning(content, prefix);
        }

        [Conditional("DEBUG")]
        public static void Error(string content, string prefix)
        {
            if (isDebug)
                loader.Error(content, prefix);
        }
    }
}
