/****************************************************
  文件：ABLoaderImpl.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 13:03:43
  功能：AB包加载器，让外部更方便地进行资源加载
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CT.ResourceSys
{
    public class ABLoader : IRelease
    {
        //AB包不能重复加载，重复加载会报错
        //使用字典来存储加载过的AB包
        private Dictionary<string, AssetBundle> abDic;

        private AssetBundle mainAB = null;                  //主包
        private AssetBundleManifest manifest = null;  //依赖包
        private ABLoaderProfile profile = null;             //ABLoader配置文件

        public ABLoader(ABLoaderProfile profile)
        {
            Init(profile);
        }

        public void Init(ABLoaderProfile profile)
        {
            abDic = new Dictionary<string, AssetBundle>();
            this.profile = profile;
        }

        #region 同步加载

        //加载AB包
        private void LoadAB(string abName)
        {
            //1.加载主包和固定文件
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(Path.Combine(profile.LocalPath, profile.MainABName));
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //2.加载依赖包
            string[] strs = manifest.GetAllDependencies(abName);
            AssetBundle ab;
            for (int i = 0; i < strs.Length; i++)
            {
                //判断包是否加载过
                if (!abDic.ContainsKey(strs[i]))
                {
                    ab = AssetBundle.LoadFromFile(Path.Combine(profile.LocalPath, strs[i]));
                    abDic.Add(strs[i], ab);
                }
            }
            //3.加载目标包
            if (!abDic.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(Path.Combine(profile.LocalPath, abName));
                if (ab != null)
                    abDic.Add(abName, ab);
                else
                    DebugMgr.Warning("目标包不存在: " + abName, SystemEnum.ResourceSystem);
            }
        }

        //法1.泛型加载 (注意GameObject请自行实例化)
        //参数①：包名    参数②：资源名
        public T LoadRes<T>(string abName, string resName) where T : Object
        {
            //加载AB包
            LoadAB(abName);

            // 4.加载资源
            if (abDic.ContainsKey(abName))
            {
                return abDic[abName].LoadAsset<T>(resName);
            }
            else
            {
                DebugMgr.Warning("资源加载失败: " + resName, SystemEnum.ResourceSystem);
                return null;
            }
        }

        //法2.类型加载
        public Object LoadRes(string abName, string resName, System.Type type)
        {
            //加载AB包
            LoadAB(abName);

            // 4.加载资源
            if (abDic.ContainsKey(abName))
            {
                return abDic[abName].LoadAsset(resName, type);
            }
            else
            {
                DebugMgr.Warning("资源加载失败: " + resName, SystemEnum.ResourceSystem);
                return null;
            }
        }

        #endregion

        #region 异步加载（AB包无异步，仅加载资源异步）
        //使用泛型异步加载ab包资源
        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(abName, resName, callback));
        }
        private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : Object
        {
            //加载AB包
            LoadAB(abName);
            //异步加载ab包资源
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abr;
            //异步加载结束后，传递给外部使用
            if (abr.asset != null)
                callback(abr.asset as T);
        }
        //使用类型异步加载ab包资源
        public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(abName, resName, type, callback));
        }
        private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callback)
        {
            //加载AB包
            LoadAB(abName);
            //异步加载ab包资源
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
            yield return abr;
            //异步加载结束后，传递给外部使用
            if (abr.asset != null)
                callback(abr.asset);
        }
        #endregion

        #region 卸载
        //单个包卸载
        public void UnLoad(string abName)
        {
            if (abDic.ContainsKey(abName))
            {
                abDic[abName].Unload(false);
                abDic.Remove(abName);
            }
        }

        //所有包卸载
        public void ClearAB()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abDic.Clear();
            mainAB = null;
            manifest = null;
        }
        #endregion

        public void Release()
        {
            ClearAB();
            profile = null;
        }
    }
}
