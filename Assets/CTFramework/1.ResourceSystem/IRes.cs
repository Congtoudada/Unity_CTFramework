/****************************************************
  文件：IRes.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 17:19:04
  功能：资源操作接口，v1.0版本仅支持AB包加载
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResSys
{
    public interface IRes : IRelease
    {
        //资源同步
        //参数①：更新时回调
        //参数②：更新成功回调
        //参数③：更新失败回调
        void UpdateAssetBundle(
            UnityAction<float> loading, 
            UnityAction successLoaded,
            UnityAction failLoaded);

        #region AB包同步加载
        //泛型同步加载
        //参数①：ab包名
        //参数②：资源名
        T LoadRes<T>(string abName, string resName) where T : Object;

        //类型同步加载
        //参数①：ab包名
        //参数②：资源名
        //参数③：类型
        Object LoadRes(string abName, string resName, System.Type type);
        #endregion

        #region AB包异步加载
        //泛型异步加载
        //参数①：ab包名
        //参数②：资源名
        //参数③：回调函数
        void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object;
        //类型异步加载
        //参数①：ab包名
        //参数②：资源名
        //参数③：类型名
        //参数④：回调函数
        void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback);
        #endregion

        #region AB包卸载
        //单个包卸载
        void UnLoad(string abName);
        //所有包卸载
        void ClearAll();
        #endregion
    }
}