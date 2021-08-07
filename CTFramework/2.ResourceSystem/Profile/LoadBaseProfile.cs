/****************************************************
  文件：LoadBaseProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 16:29:41
  功能：同步加载，配置文件
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResourceSys
{
    public abstract class LoadBaseProfile
    {
        public string filename; //资源名称

        //同步加载游戏对象
        public abstract T Load<T>() where T : Object;

        //异步加载游戏对象
        public abstract void LoadAsync<T>(UnityAction<T> callback) where T : Object;
    }
}
