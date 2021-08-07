/****************************************************
  文件：ResProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 16:54:20
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResourceSys
{
    public abstract class ResProfile : LoadBaseProfile
    {
        public string dirPath; //Resource所在路径，不包括文件名

        public ResProfile(string dirPath)
        {
            this.dirPath = dirPath;
        }

        public override T Load<T>()
        {
            if(Check())
            {
                string path = Path.Combine(dirPath, filename);
                T template = _CT.ResMgr.LoadByRes<T>(path);
                if (template == null)
                    DebugMgr.Warning("没有找到资源，请检查路径: " + path, SystemEnum.ResourceSystem);
                return template;
            }
            return null;
        }

        public override void LoadAsync<T>(UnityAction<T> callback)
        {
            if (Check())
            {
                string path = Path.Combine(dirPath, filename);
                _CT.ResMgr.LoadByResAsync<T>(path, callback);
            }
        }

        private bool Check()
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(dirPath))
            {
                DebugMgr.Warning("加载失败，路径或文件名为空", SystemEnum.ResourceSystem);
                return false;
            }
            return true;
        }
    }
}
