/****************************************************
  文件：ExtendResMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 11:59:38
  功能：
*****************************************************/
using CT.ResSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT
{
    public static class ExtendResMgr
    {
        //Resources异步加载增强
        public static void LoadResAsync<T>(this IRes mgr, string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            _CT.Instance.StartCoroutine(LoadEnumerator(path, callback));
        }

        //异步加载协程
        private static IEnumerator LoadEnumerator<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);

            while (!request.isDone)
            {
                //Debug.Log("Loading progress: " + resourcesRequest.progress);
                //资源没有加载完成  返回空，加载完以后进行后面的模块
                yield return null;
            }
            T asset = request.asset as T;
            callback.Invoke(asset);
        }
    }
}