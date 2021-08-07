/****************************************************
  文件：SceneMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 12:01:08
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
    public class SceneMgr : IRelease
    {
        //场景加载器
        private SceneLoader _sceneLoader;

        private SceneLoader sceneLoader
        {
            get
            {
                if (_sceneLoader == null)
                    _sceneLoader = new SceneLoader();
                return _sceneLoader;
            }
        }

        #region 场景加载配置文件
        //创建同步加载基础配置
        public SceneProfile CreateProfile(string sceneName)
        {
            SceneProfile profile = new SceneProfile();
            profile.sceneName = sceneName;
            return profile;
        }

        //创建异步加载基础配置（当进度为0.9时表示场景加载完成，此时将isAutoLoad设为true则自动切换）
        public SceneProfile CreateAsyncProfile(string sceneName,
            bool isAutoLoad,
            UnityAction<float, SceneProfile> loading)
        {
            SceneProfile profile = new SceneProfile();
            profile.sceneName = sceneName;
            profile.isAutoLoad = isAutoLoad;
            profile.loading += loading;
            return profile;
        }

        //新增场景加载完成回调
        public void AddLoadedCallback(SceneProfile profile, UnityAction<Scene, LoadSceneMode> loaded)
        {
            profile.loaded += loaded;
        }

        //新增场景卸载完成回调（指的是旧场景销毁时的回调，并非新场景销毁时的回调）
        public void AddUnloadCallback(SceneProfile profile, UnityAction<Scene> unLoaded)
        {
            profile.unLoaded += unLoaded;
        }

        //新增场景加载时回调
        public void AddLoadingCallback(SceneProfile profile, UnityAction<float, SceneProfile> loading)
        {
            profile.loading += loading;
        }
        #endregion

        #region 场景加载
        //同步加载场景
        public void LoadScene(SceneProfile profile)
        {
            sceneLoader.LoadScene(profile);
        }

        //异步加载场景
        public void LoadSceneAsync(SceneProfile profile)
        {
            sceneLoader.LoadSceneAsync(profile);
        }
        #endregion

        //释放资源
        public void Release()
        {
            ReleaseTool.TryRelease(sceneLoader);
            _sceneLoader = null;
        }
    }
}
