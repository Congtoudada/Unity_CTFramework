/****************************************************
  文件：SceneMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/20 14:03:22
  功能：控制场景跳转
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CT
{
    public class SceneMgr : ISceneMgr
    {
        //场景生命周期：设A为初始场景，B为下一个场景
        //场景A：所有物体的Awake(Start)->LoadBefore(Pop UI)->所有物体的OnDestory->Unloaded
        //场景B：loaded->所有物体的Awake(Start)->afterLoaded
        //所有回调均为一次性回调，每次触发后都会清空
        public Action loadBefore; //场景切换前（此时没有物体被销毁）
        public UnityAction<Scene> unloaded; //前一个场景销毁前（此时所有物体已调用OnDestroy）
        public Action<float> loadingAsync; //场景异步加载中（持续调用）
        public Func<bool> loadedAsync; //场景异步加载完成（持续调用，为true切换）
        public UnityAction<Scene> loaded; //场景加载完成之后（Start前）触发
        public Action loadAfter; //场景加载完成之后（调用Start后）触发
        
        private AsyncOperation operation;

        //同步加载场景
        public void LoadScene(string name)
        {
            LoadBefore();
            SceneManager.LoadScene(name);
        }

        //异步加载场景（自动切换）
        public void LoadSceneAsync(string name, Action<float> loading = null, Func<bool> loaded = null)
        {
            loadingAsync = loading;
            loadedAsync = loaded;
            bool isAuto = loaded == null;
            LoadBefore();
            _CT.Instance.StartCoroutine(LoadingEnumerator(name, isAuto));
        }

        public void SetLoadBefore(Action callback)
        {
            loadBefore = callback;
        }

        public void SetUnLoaded(UnityAction<Scene> callback)
        {
            unloaded = callback;
        }

        public void SetLoaded(UnityAction<Scene> callback)
        {
            loaded = callback;
        }

        public void SetLoadAfter(Action callback)
        {
            loadAfter = callback;
        }

        #region 私有函数
        
        //场景加载前
        private void LoadBefore()
        {
            //加载场景前
            loadBefore?.Invoke();
            //如果有UIMgr，清空UI堆栈
            _CT.UIMgr?.AllPop();
            //如果有定时回调，则清空
            CTHelper.timerMgr?.CancelAll();
            //绑定回调
            SceneManager.sceneUnloaded += UnitySceneUnLoaded;
            SceneManager.sceneLoaded += UnitySceneLoaded;
        }
        
        //场景卸载回调函数
        private void UnitySceneUnLoaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= UnitySceneUnLoaded;
            unloaded?.Invoke(scene);
            unloaded = null;
        }
        
        //场景加载回调函数（完成一些自动卸载的工作）
        private void UnitySceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= UnitySceneLoaded;
            loadBefore = null;
            loadingAsync = null;
            loadedAsync = null;
            loaded?.Invoke(scene);
            loaded = null;
            _CT.Instance.StartCoroutine(LoadedEnumerator());
        }

        private IEnumerator LoadedEnumerator()
        {
            //Start -> FixedUpdate
            yield return new WaitForFixedUpdate(); 
            loadAfter?.Invoke();
            loadAfter = null;
        }

        //异步加载场景协程
        private IEnumerator LoadingEnumerator(string name, bool isAuto)
        {
            operation = SceneManager.LoadSceneAsync(name);
            operation.allowSceneActivation = isAuto;
            //allow为true会自动退出循环，加载新的场景
            while(!operation.isDone)
            {
                //暂时没有想好，场景准备好后时禁用loading还是继续调用。。。
                loadingAsync?.Invoke(operation.progress);
                if (!isAuto && operation.progress == 0.9f) //场景准备完毕
                {
                    if (loadedAsync()) //只有判断函数返回true，才切换
                    {
                        //如果有UIMgr，清空UI堆栈
                        _CT.UIMgr?.AllPop();
                        operation.allowSceneActivation = true;
                    }
                }
                yield return null;
            }
        }
        
        #endregion
    }
}