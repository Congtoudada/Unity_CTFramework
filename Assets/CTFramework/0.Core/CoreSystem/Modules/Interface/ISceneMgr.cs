/****************************************************
  文件：ISceneMgr.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2022/4/17 20:02:21
  功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CT
{
  public interface ISceneMgr
  {
    //同步加载场景
    //参数①：场景名称
    void LoadScene(string name);
    
    //异步加载场景
    //参数①：场景名称
    //参数②：加载时回调函数
    //参数③：切换场景条件
    void LoadSceneAsync(string name, Action<float> loading = null, Func<bool> loaded = null);
    
    //场景生命周期：设A为初始场景，B为下一个场景
    //场景A：所有物体的Awake(Start)->LoadBefore(Pop UI)->所有物体的OnDestory->Unloaded
    //场景B：loaded->所有物体的Awake(Start)->LoadAfter
    //所有回调均为一次性回调，每次触发后都会清空
    
    void SetLoadBefore(Action callback);

    void SetUnLoaded(UnityAction<Scene> callback);

    void SetLoaded(UnityAction<Scene> callback);

    void SetLoadAfter(Action callback);

  }
}
