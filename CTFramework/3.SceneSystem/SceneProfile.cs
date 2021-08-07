/****************************************************
  文件：SceneState.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 12:07:48
  功能：加载场景前后的一些配置
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CT.SceneSys
{
    public class SceneProfile : IRelease
    {
        //跳转场景的名称
        public string sceneName;

        //是否自动切换场景（仅限异步加载）
        public bool isAutoLoad;

        //加载场景后的第一个面板

        //场景加载成功的回调（时机：awake -> onEnable -> 场景回调 -> start）
        public UnityAction<Scene, LoadSceneMode> loaded;

        //场景卸载的回调（时机：在OnDestroy之后）
        public UnityAction<Scene> unLoaded;

        //场景加载中的信息（仅限异步加载）
        public UnityAction<float, SceneProfile> loading;

        public void Release()
        {
            loaded = null;
            unLoaded = null;
            loading = null;
        }
    }
}
