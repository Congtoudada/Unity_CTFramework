/****************************************************
  文件：ResMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 17:19:37
  功能：资源管理类，函数解释具体看接口，这里不重复
*****************************************************/
using CT.ResSys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT
{
    public class ResMgr : IRes
    {
        private IRes loader;

        public ResMgr() { }

        //动态注入加载器
        public void SetLoader(IRes loader)
        {
            if (this.loader != null)
                Release();
            this.loader = loader;
        }

        private bool isValid()
        {
            if (loader == null)
            {
                CTLogger.Log("没有找到Loader", SysEnum.ResourceSystem);
                return false;
            }
            return true;
        }

        public void UpdateAssetBundle(UnityAction<float> loading, UnityAction successLoaded, UnityAction failLoaded)
        {
            if (isValid())
            {
                loader.UpdateAssetBundle(loading, successLoaded, failLoaded);
            }
        }

        public T LoadRes<T>(string abName, string resName) where T : UnityEngine.Object
        {
            if (isValid())
            {
                return loader.LoadRes<T>(abName, resName);
            }
            return default(T);
        }

        public UnityEngine.Object LoadRes(string abName, string resName, Type type)
        {
            if (isValid())
            {
                return loader.LoadRes(abName, resName, type);
            }
            return null;
        }

        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            if (isValid())
            {
                loader.LoadResAsync<T>(abName, resName, callback);
            }
        }

        public void LoadResAsync(string abName, string resName, Type type, UnityAction<UnityEngine.Object> callback)
        {
            if (isValid())
            {
                loader.LoadResAsync(abName, resName, type, callback);
            }
        }

        public void Release()
        {
            ReleaseTool.TryRelease(loader);
            loader = null;
        }

        public void UnLoad(string abName)
        {
            if (isValid())
            {
                loader.UnLoad(abName);
            }
        }

        public void ClearAll()
        {
            if (isValid())
            {
                loader.ClearAll();
            }
        }
    }
}