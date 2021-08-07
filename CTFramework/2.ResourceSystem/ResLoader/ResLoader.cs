/****************************************************
  文件：ResLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 15:55:29
  功能：Resouces资源加载器，改进Resources异步加载功能
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResourceSys
{
    public class ResLoader : IRelease
    {
        //同步加载也可以直接使用Resources.Load即可
        #region 同步加载
        public T LoadRes<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public Object LoadRes(string path, System.Type type)
        {
            return Resources.Load(path, type);
        }
        #endregion

        #region 异步加载
        //泛型加载
        public void LoadResAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(path, callback));
        }

        private IEnumerator ReallyLoadResAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            //异步加载res资源
            ResourceRequest req = Resources.LoadAsync<T>(path);
            yield return req;
            //异步加载结束后，传递给外部使用
            if(req.asset != null)
                callback(req.asset as T);      
        }

        //类型加载
        //使用类型异步加载ab包资源
        public void LoadResAsync(string path, System.Type type, UnityAction<Object> callback)
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(path, type, callback));
        }
        private IEnumerator ReallyLoadResAsync(string path, System.Type type, UnityAction<Object> callback)
        {
            //异步加载res资源
            Object req = Resources.Load(path, type);
            yield return req;
            //异步加载结束后，传递给外部使用
            if (req != null)
                callback(req);
        }
        #endregion

        public void Release() { }
    }
}
