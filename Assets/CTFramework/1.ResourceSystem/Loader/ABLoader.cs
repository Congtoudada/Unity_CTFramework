/****************************************************
  文件：ABLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 17:45:02
  功能：AB包加载器，处理AB包加载
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CT.ResSys
{
    public class ABLoader : IRes
    {
        //AB包不能重复加载，重复加载会报错
        //使用字典来存储加载过的AB包
        private Dictionary<string, AssetBundle> abDic;

        private AssetBundle mainAB = null;                  //主包
        private AssetBundleManifest manifest = null;  //依赖包
        private ResourceProfile profile = null;              //ABLoader配置文件
        private bool isSuccessfully;                               //ab包资源是否更新成功
        private UnityAction<float> loading;                 //ab包加载时回调
        private UnityAction successLoaded;                 //ab包加载成功回调
        private UnityAction failLoaded;                        //ab包加载失败回调
        private int proccess;                                         //当前进度 已更新文件数 / 总文件数


        //版本清单列表
        [Serializable]
        public class VersionList
        {
            public List<VersionItem> updateList = new List<VersionItem>();
        }

        //版本清单项
        [Serializable]
        public class VersionItem
        {
            public string fileName;
            public string md5;
            //[NonSerialized]
            //public bool isCompare; //是否已经比较过该资源
        }

        public ABLoader(ResourceProfile profile)
        {
            if (profile == null)
                CTLogger.Error("致命错误，加载ResProfile失败!", SysEnum.ResourceSystem);
            abDic = new Dictionary<string, AssetBundle>();
            this.profile = profile;
        }

        #region 初始化
        //更新AB包(从服务器下载AB包)
        public void UpdateAssetBundle(UnityAction<float> loading, UnityAction successLoaded, UnityAction failLoaded)
        {
            isSuccessfully = false;
            this.loading = loading;
            this.successLoaded = successLoaded;
            this.failLoaded = failLoaded;
            //开启网络更新，才更新
            if (profile.isNetUpdate)
                _CT.Instance.StartCoroutine(CompareVersionList());  
        }

        //1.准备清单，用于比较文本差异
        private IEnumerator CompareVersionList()
        {
            //请求版本列表，根据版本列表更新资源
            string uri = Path.Combine(profile.netPath, profile.abVersionList);

            //获取服务器配置文件和本地比较
            UnityWebRequest request = UnityWebRequest.Get(uri);//要使用这种方式获取资源,才能访问到他的字节流

            //开始下载
            loading?.Invoke(0);

            yield return request.SendWebRequest();

            //成功获取服务器版本列表
            if (request.isDone)
            {
                string json = request.downloadHandler.text; //最新清单列表
                string oldJson = string.Empty;
                string dirPath = profile.GetLocalPath();
                string filePath = Path.Combine(dirPath, profile.abVersionList); //本地清单列表

                //如果本地清单目录不存在，则创建目录
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                //如果本地清单不存在，直接写入新清单
                if (!File.Exists(filePath))
                {
                    FileTool.WriteTextToDisk(filePath, json);
                }
                else
                {
                    //读取旧清单，用于比较二者差异
                    oldJson = FileTool.ReadTextFromDisk(filePath);
                }
                yield return null; //避免1帧调用过多内容
                yield return UpdateFile(oldJson, json);
            }
        }

        //2.比较版本差异，更新资源     Important!!!
        private IEnumerator UpdateFile(string oldJson, string newJson)
        {
            //网络异常
            if (string.IsNullOrEmpty(newJson))
            {
                CTLogger.Warning("网络连接异常，资源清单下载失败!", SysEnum.ResourceSystem);
            }
            //首次更新
            else if (string.IsNullOrEmpty(oldJson))
            {
                VersionList newList = JsonUtility.FromJson<VersionList>(newJson); //根据最新清单列表生成对象
                VersionList checkList = new VersionList();   //临时清单，用于校验
                string uri = string.Empty;  //网络路径
                MD5 md5 = MD5.Create();

                yield return null; //避免1帧调用过多内容

                //根据最新清单列表下载资源
                foreach (VersionItem newItem in newList.updateList)
                {
                    uri = Path.Combine(profile.netPath, newItem.fileName);
                    yield return DownloadFile(uri, checkList, md5);
                    //没有校验，最高也到不了100%
                    loading?.Invoke((float)++proccess / (newList.updateList.Count + 1));
                }

                //更新校验
                yield return CheckFile(newList, checkList, md5, 3);
            }
            //正常更新
            else
            {
                //生成新旧版本列表对象
                VersionList oldList = JsonUtility.FromJson<VersionList>(oldJson);
                VersionList newList = JsonUtility.FromJson<VersionList>(newJson);
                VersionList checkList = new VersionList();   //临时清单，用于校验
                string uri = string.Empty;    //网络路径
                MD5 md5 = MD5.Create();

                yield return null; //避免1帧调用过多内容

                bool isNew; //判断是否是新资源
                //外层循环遍历新资源清单
                foreach (VersionItem newItem in newList.updateList)
                {
                    isNew = true;
                    uri = Path.Combine(profile.netPath, newItem.fileName);
                    //内层循环遍历旧资源清单
                    foreach (VersionItem oldItem in oldList.updateList)
                    {
                        //先判断新、旧版本文件是否同名
                        if (newItem.fileName.Equals(oldItem.fileName)) 
                        {
                            isNew = false;
                            //再判断MD5，不相等就更新该文件
                            if (!newItem.md5.Equals(oldItem.md5))
                            {
                                yield return DownloadFile(uri, checkList, md5);
                                break;
                            }
                            else break; //md5值相等，代表新旧文件相同,跳出
                        }
                    }
                    if (isNew) //如果没有同名资源，代表是新资源，直接更新
                    {
                        yield return DownloadFile(uri, checkList, md5);
                    }
                    //没有校验，最高也到不了100%
                    loading?.Invoke((float)++proccess / (newList.updateList.Count + 1));
                }
                //全部更新完毕，进行校验，失败最多重试3次
                yield return CheckFile(newList, checkList, md5, profile.retryCount);
            }

            //全部资源更新并校验，结果判断
            if (isSuccessfully)
            {
                //加载到1
                loading?.Invoke(1);
                yield return null; //缓冲1帧
                //更新本地资源清单
                string filePath = Path.Combine(profile.GetLocalPath(), profile.abVersionList);
                FileTool.WriteTextToDisk(filePath, newJson);
                CTLogger.Log("校验成功，资源更新成功!", SysEnum.ResourceSystem);
                successLoaded?.Invoke();
            }
            else
            {
                CTLogger.Warning("校验失败，资源更新失败!", SysEnum.ResourceSystem);
                failLoaded?.Invoke();
            }
        }

        //3.下载AssetBundle资源
        private IEnumerator DownloadFile(string uri, VersionList checkList, MD5 md5)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);//要使用这种方式获取资源,才能访问到他的字节流
            yield return request.SendWebRequest();

            //保存下载回来的文件
            if (request.isDone)
            {
                VersionItem item = new VersionItem();
                item.fileName = Path.GetFileName(uri);
                //更换为本地uri
                uri = Path.Combine(profile.GetLocalPath(), item.fileName);
                //计算改文件的md5
                item.md5 = System.BitConverter.ToString(md5.ComputeHash(request.downloadHandler.data)).Replace("-", "").ToLower();
                //Debug.Log(item.md5);
                //检查列表中是否存在该项，若是则更新其md5，否则添加
                if (checkList.updateList.Exists(t => t.fileName.Equals(item.fileName)))
                {
                    VersionItem updateItem = checkList.updateList.Find(t => t.fileName.Equals(item.fileName));
                    updateItem.md5 = item.md5;//更新md5
                }
                else
                {
                    checkList.updateList.Add(item);
                }
                CTLogger.Log("更新资源: " + item.fileName, SysEnum.ResourceSystem);
                FileTool.WriteBytesToDisk(profile.GetLocalPath(), Path.GetFileName(uri), request.downloadHandler.data);
            }
        }

        //4.更新校验
        private IEnumerator CheckFile(VersionList newList, VersionList checkList, MD5 md5, int retryCount)
        {
            //重试次数为0，更新失败
            if (retryCount == 0)
            {
                isSuccessfully = false;
                yield return null;
            }
            else
            {
                retryCount--;
                bool isOK = true;
                foreach (VersionItem checkItem in checkList.updateList)
                {
                    foreach (VersionItem newItem in newList.updateList)
                    {
                        if (checkItem.fileName.Equals(newItem.fileName))
                        {
                            if (!checkItem.md5.Equals(newItem.md5)) //如果MD5不相等就重新更新
                            {
                                string uri = Path.Combine(profile.netPath, checkItem.fileName);
                                isOK = false;
                                yield return DownloadFile(uri, checkList, md5);
                            }
                            else break;
                        }
                    }
                }
                if (isOK)
                {
                    isSuccessfully = true;
                    yield return null;
                }
                else yield return CheckFile(newList, checkList, md5, retryCount);
            }
        }

        //加载AB包
        private void LoadAB(string abName)
        {
            //1.加载主包和固定文件
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(Path.Combine(profile.GetLocalPath(), profile.mainABName));
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
                    ab = AssetBundle.LoadFromFile(Path.Combine(profile.GetLocalPath(), strs[i]));
                    abDic.Add(strs[i], ab);
                }
            }
            //3.加载目标包
            if (!abDic.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(Path.Combine(profile.GetLocalPath(), abName));
                if (ab != null)
                    abDic.Add(abName, ab);
                else
                    CTLogger.Warning("目标包不存在: " + abName, SysEnum.ResourceSystem);
            }
        }
        
        #endregion

        #region 同步加载
        //法1.泛型加载 (注意GameObject请自行实例化)
        //参数①：包名    参数②：资源名
        public T LoadRes<T>(string abName, string resName) where T : UnityEngine.Object
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
                CTLogger.Warning("资源加载失败: " + resName, SysEnum.ResourceSystem);
                return null;
            }
        }

        //法2.类型加载
        public UnityEngine.Object LoadRes(string abName, string resName, System.Type type)
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
                CTLogger.Warning("资源加载失败: " + resName, SysEnum.ResourceSystem);
                return null;
            }
        }

        #endregion

        #region 异步加载
        //使用泛型异步加载ab包资源
        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(abName, resName, callback));
        }
        private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
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
        public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<UnityEngine.Object> callback)
        {
            _CT.Instance.StartCoroutine(ReallyLoadResAsync(abName, resName, type, callback));
        }
        private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<UnityEngine.Object> callback)
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
        public void ClearAll()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abDic.Clear();
            mainAB = null;
            manifest = null;
        }

        //销毁AB包加载器
        public void Release()
        {
            ClearAll();
            profile = null;
        }

        #endregion

    }
}