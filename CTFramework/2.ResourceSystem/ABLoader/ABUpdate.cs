/****************************************************
  文件：ABUpdate.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 14:27:20
  功能：更新AB包并校验
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CT.ResourceSys
{
    public class ABUpdate : IRelease
    {
        private ABLoaderProfile profile;   //配置文件
        private bool isSuccessfully;        //资源更新结果

        public ABUpdate(ABLoaderProfile profile)
        {
            Init(profile);
        }

        public void Init(ABLoaderProfile profile)
        {
            this.profile = profile;
        }

        public void UpdateAssetBundle()
        {
            _CT.Instance.StartCoroutine(UpdateEnumerator());
        }

        #region 更新AB包(从服务器下载AB包)
        private IEnumerator UpdateEnumerator()
        {
            //请求版本列表，根据版本列表更新资源
            string uri = Path.Combine(profile.NetPath, CTConstant.AB_VERSION_LIST);

            //获取服务器配置文件和本地比较
            UnityWebRequest request = UnityWebRequest.Get(uri);//要使用这种方式获取资源,才能访问到他的字节流

            yield return request.SendWebRequest();

            if(request.isDone)
            {
                string json = request.downloadHandler.text;
                string oldJson = string.Empty;
                string dirPath = profile.LocalPath;
                string filePath = Path.Combine(profile.LocalPath, CTConstant.AB_VERSION_LIST);
                //比较本地资源清单
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                //如果文件不存在，直接写入新清单
                if(!File.Exists(filePath))
                {
                    _CT.FileMgr.WriteString(filePath, json);
                }
                else
                {
                    //读取旧清单，用于比较二者差异
                    oldJson = _CT.FileMgr.ReadString(filePath);
                }

                yield return null; //避免1帧调用过多内容
                yield return UpdateFile(oldJson, json);
                Debug.Log("资源更新结果为: " + isSuccessfully);
            }
        }

        private IEnumerator UpdateFile(string oldJson, string newJson)
        {
            isSuccessfully = false;

            if (string.IsNullOrEmpty(newJson)) //网络异常
            {
                DebugMgr.Warning("网络连接异常，资源清单下载失败!", SystemEnum.ResourceSystem);
            }
            else if(string.IsNullOrEmpty(oldJson)) //第一次更新
            {
                VersionList newList = JsonUtility.FromJson<VersionList>(newJson);
                VersionList checkList = new VersionList();   //临时清单，用于校验
                string uri = string.Empty;  //网络路径
                MD5 md5 = MD5.Create();

                yield return null; //避免1帧调用过多内容

                foreach (VersionItem newItem in newList.updateList)
                {
                    uri = Path.Combine(profile.NetPath, newItem.fileName);
                    yield return DownloadFile(uri, checkList, md5);
                }
                //更新校验
                yield return CheckFile(newList, checkList, md5, 3);
            }
            else //正常更新
            {
                VersionList oldList = JsonUtility.FromJson<VersionList>(oldJson);
                VersionList newList = JsonUtility.FromJson<VersionList>(newJson);
                VersionList checkList = new VersionList();   //临时清单，用于校验
                string uri = string.Empty;  //网络路径
                bool isHas;                         //判断是否是新资源
                MD5 md5 = MD5.Create();

                yield return null; //避免1帧调用过多内容

                foreach (VersionItem newItem in newList.updateList)
                {
                    isHas = false;
                    uri = Path.Combine(profile.NetPath, newItem.fileName);
                    foreach (VersionItem oldItem in oldList.updateList)
                    {
                        if (newItem.fileName.Equals(oldItem.fileName)) //找到旧版本文件
                        {
                            isHas = true;
                            //判断MD5，不相等就更新
                            if (!newItem.md5.Equals(oldItem.md5))
                            {
                                yield return DownloadFile(uri, checkList, md5);
                            }
                            else break; //同名同MD5，无需更新，直接break节省性能
                        }
                    }
                    if (!isHas) //代表是新资源，直接更新
                    {
                        yield return DownloadFile(uri, checkList, md5);
                    }
                }
                //更新校验
                yield return CheckFile(newList, checkList, md5, 3);
            }
            //结果判断
            if(isSuccessfully)
            {
                //更新本地资源清单
                string filePath = Path.Combine(profile.LocalPath, CTConstant.AB_VERSION_LIST);
                _CT.FileMgr.WriteString(filePath, newJson);
                DebugMgr.Log("资源更新成功!", SystemEnum.ResourceSystem);
            }
            else
            {
                DebugMgr.Warning("资源更新失败!", SystemEnum.ResourceSystem);
            }
        }

        //下载AssetBundle资源
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
                uri = Path.Combine(profile.LocalPath, item.fileName);
                item.md5 = System.BitConverter.ToString(md5.ComputeHash(request.downloadHandler.data)).Replace("-", "").ToLower();
                //Debug.Log(item.md5);
                //检查列表中已经存在该项，则重新更新，否则添加
                if (checkList.updateList.Exists(t => t.fileName.Equals(item.fileName)))
                {
                   VersionItem updateItem =  checkList.updateList.Find(t => t.fileName.Equals(item.fileName));
                    updateItem.md5 = item.md5;//更新md5
                }
                else
                {
                    checkList.updateList.Add(item);
                }
                DebugMgr.Log("更新资源: " + item.fileName, SystemEnum.ResourceSystem);
                _CT.FileMgr.WriteBytes(profile.LocalPath, Path.GetFileName(uri), request.downloadHandler.data);
            }
        }
        //更新校验
        private IEnumerator CheckFile(VersionList newList, VersionList checkList, MD5 md5, int retryCount)
        {
            if (retryCount == 0)
            {
                isSuccessfully = false;
                yield return null;
            }
            else
            {
                retryCount--;
                bool result = true;
                foreach (VersionItem checkItem in checkList.updateList)
                {
                    foreach (VersionItem newItem in newList.updateList)
                    {
                        if (checkItem.fileName.Equals(newItem.fileName))
                        {
                            if (!checkItem.md5.Equals(newItem.md5)) //如果MD5不相等就重新更新
                            {
                                string uri = Path.Combine(profile.NetPath, checkItem.fileName);
                                result = false;
                                yield return DownloadFile(uri, checkList, md5);
                            }
                            else break;
                        }
                    }
                }
                if (result)
                {
                    isSuccessfully = true;
                    yield return null;
                }
                else yield return CheckFile(newList, checkList, md5, retryCount);
            }
        }

        public void Release()
        {
            profile = null;
        }
        #endregion
    }

    [Serializable]
    public class VersionList
    {
        public List<VersionItem> updateList = new List<VersionItem>();
    }

    [Serializable]
    public class VersionItem
    {
        public string fileName;
        public string md5;
    }
}
