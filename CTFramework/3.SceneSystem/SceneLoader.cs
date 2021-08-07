/****************************************************
  文件：SceneLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 12:02:23
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CT.SceneSys
{
    public class SceneLoader : IRelease
    {
        private SceneProfile _profile;
        public SceneProfile profile { get; set; }

        private AsyncOperation operation; //用于异步加载

        //同步加载
        public void LoadScene(SceneProfile profile)
        {
            if(CheckScene(profile.sceneName))
            {
                this.profile = profile;
                SceneManager.sceneLoaded += Loaded;
                SceneManager.sceneUnloaded += UnLoaded;
                SceneManager.LoadScene(profile.sceneName);
            }
        }

        //异步加载
        public void LoadSceneAsync(SceneProfile profile)
        {
            if(CheckScene(profile.sceneName))
            {
                this.profile = profile;
                _CT.Instance.StartCoroutine(LoadEnumerator(profile));
            }
        }

        private IEnumerator LoadEnumerator(SceneProfile profile)
        {
            operation = SceneManager.LoadSceneAsync(profile.sceneName);
            operation.allowSceneActivation = profile.isAutoLoad; //加载完成后是否自动切换
            while(!operation.isDone)
            {
                profile.loading?.Invoke(operation.progress, profile);
                if (profile.isAutoLoad)
                {
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        //检查
        private bool CheckScene(string sceneName)
        {
            //场景不重复
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                DebugMgr.Warning("场景已加载，不可重复加载: " + sceneName, SystemEnum.SceneSystem);
                return false;
            }
            else if (SceneManager.GetSceneByName(sceneName) == null)
            {
                DebugMgr.Warning("场景不存在: " + sceneName, SystemEnum.SceneSystem);
                return false;
            }
            return true;
        }

        //场景加载成功的回调（时机：awake -> onEnable -> 场景回调 -> start）
        private void Loaded(Scene scene, LoadSceneMode mode)
        {
            //内部回调

            //外部回调
            profile.loaded?.Invoke(scene, mode);
            //收尾工作
            SceneManager.sceneLoaded -= Loaded;
            Release();
        }

        //场景卸载的回调（时机：在OnDestroy之后）
        private void UnLoaded(Scene scene)
        {
            //内部回调

            //外部回调
            profile.unLoaded?.Invoke(scene);
            //收尾工作
            SceneManager.sceneUnloaded -= UnLoaded;
        }

        public void Release()
        {
            ReleaseTool.TryRelease(profile);
            profile = null;
        }
    }
}
