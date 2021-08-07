/****************************************************
  文件：ResourceMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 12:56:06
  功能：资源管理类，因为AB包加载和Resouces加载规则不同，故只能硬编码实现
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.ResourceSys
{
    public class ResMgr : IRelease
    {
        private ABLoaderProfile profile;
        private ABLoader _abLoader; //AB包加载器
        private ABUpdate abUpdate;
        private ResLoader _resLoader; //Resources加载器

        private ABLoader abLoader
        {
            set { _abLoader = value; }
            get
            {
                if (_abLoader == null)
                {
                    //DebugMgr.Warning("没有加载abLoader", SystemEnum.ResourceSystem);
                    WithABLoader(true);
                }
                return _abLoader;
            }
        }
        private ResLoader resLoader
        {
            set { _resLoader = value; }
            get
            {
                if (_resLoader == null)
                {
                    //DebugMgr.Warning("没有加载resLoader", SystemEnum.ResourceSystem);
                    WithResLoader();
                }
                return _resLoader;
            }
        }

        #region 构造器
        //装配ResLoder加载器
        public ResMgr WithResLoader()
        {
            if(_resLoader == null)
                _resLoader = new ResLoader();
            return this;
        }

        //装配ABLoader加载器，指定加载时是否更新资源
        public ResMgr WithABLoader(bool isUpdate)
        {
            //准备默认配置文件
            if (profile == null)
            {
                profile = Resources.Load<ABLoaderProfile>(CTConstant.AB_PROFILE_PATH);
                if (profile == null)
                {
                    DebugMgr.Error($"Resources下 {CTConstant.AB_PROFILE_PATH} 没有找到ABLoaderProfile!", SystemEnum.ResourceSystem);
                    return this;
                }
            }

            //准备abUpdate
            if (abUpdate == null)
                abUpdate = new ABUpdate(profile);
            
            //如果需要更新，就更新
            if (isUpdate)
            {
                //更新AB包
                abUpdate.UpdateAssetBundle();   //更新AssetBundle
            }

            if(_abLoader == null)
                _abLoader = new ABLoader(profile);
            return this;
        }
        
        #endregion

        ///提供用户调用的方法
        #region 更新资源
        public void UpdateAssetBundle()
        {
            if(abUpdate != null)
            {
                abUpdate.UpdateAssetBundle();
            }
            else
            {
                DebugMgr.Warning("更新失败，请先装配abLoader", SystemEnum.ResourceSystem);
            }
        }
        #endregion

        #region 加载资源

        #region Resources加载
        //同步加载 通过Resources加载
        public T LoadByRes<T>(string path) where T : Object
        {
            if (resLoader != null)
                return resLoader.LoadRes<T>(path);
            else return default(T);
        }

        public Object LoadByRes(string path, System.Type type)
        {
            return resLoader?.LoadRes(path, type);
        }

        //异步加载 通过Resources加载
        public void LoadByResAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            resLoader?.LoadResAsync<T>(path, callback);
        }

        public void LoadByResAsync(string path, System.Type type, UnityAction<Object> callback)
        {
            resLoader?.LoadResAsync(path, type, callback);
        }
        #endregion

        #region AssetBundle加载
        //同步加载 通过AssetBundle
        public T LoadByAB<T>(string abName, string resName) where T : Object
        {
            if (abLoader != null)
                return abLoader.LoadRes<T>(abName, resName);
            else return default(T);
        }

        public Object LoadByAB(string abName, string resName, System.Type type)
        {
            return abLoader?.LoadRes(abName, resName, type);
        }

        //异步加载 通过AssetBundle
        public void LoadByABAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
        {
            abLoader?.LoadResAsync(abName, resName, callback);
        }

        public void LoadByABAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)
        {
            abLoader?.LoadResAsync(abName, resName, type, callback);
        }
        #endregion

        #endregion

        //释放资源
        public void Release()
        {
            ReleaseTool.TryRelease(abLoader);
            ReleaseTool.TryRelease(resLoader);
            abLoader = null;
            resLoader = null;
        }
    }
}
