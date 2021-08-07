/****************************************************
  文件：ABProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 16:55:25
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResourceSys
{
    public abstract class ABProfile : LoadBaseProfile
    {
        public string abName; //AB包名

        public ABProfile(string abName)
        {
            this.abName = abName;
        }

        public override T Load<T>()
        {
            if(Check())
            {
                T template = _CT.ResMgr.LoadByAB<T>(abName, filename);
                if (template == null)
                    DebugMgr.Warning("没有找到资源，请检查AB包: " + abName + "--" + filename, SystemEnum.ResourceSystem);
                return template;
            }
            return null;
        }

        public override void LoadAsync<T>(UnityAction<T> callback)
        {
            if (Check())
            {
                _CT.ResMgr.LoadByABAsync<T>(abName, filename, callback);
            }
        }

        private bool Check()
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(abName))
            {
                DebugMgr.Warning("加载失败，包名或文件名为空", SystemEnum.ResourceSystem);
                return false;
            }
            return true;
        }
    }
}
