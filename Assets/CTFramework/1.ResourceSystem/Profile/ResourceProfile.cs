/****************************************************
  文件：ResProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 17:20:29
  功能：资源配置文件
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.ResSys
{
    [CreateAssetMenu(menuName = "CT/ResourceProfile", fileName = "ResourceProfile", order = 15)]
    public class ResourceProfile : ScriptableObject
    {
        [Header("是否启用AssetBundle加载")]
        public bool isABLoad = false;

        #region Resources加载路径配置
        //放在各模块的配置文件里
        #endregion

        #region AssetBundle主包加载路径配置
        [Header("是否从StreamingAssets加载本地资源")]
        [EnableIf("isABLoad")]
        public bool isStreamingAssets = true;

        [Header("AB包本地路径")]
        [Tooltip("如果从StreamingAssets加载，直接写子目录；否则写完整路径")]
        [TextArea]
        [SerializeField]
        [EnableIf("isABLoad")]
        [FolderPath]
        private string localPath = "AssetBundle";

        [Header("主包名")]
        [EnableIf("isABLoad")]
#if UNITY_ANDROID
    public string mainABName = "Android";
#elif UNITY_IOS
    public string mainABName = "IOS";
#else
        public string mainABName = "PC";
#endif

        [Header("是否启用网络加载")]
        [EnableIf("isABLoad")]
        public bool isNetUpdate = true;

        [EnableIf("isNetUpdate")]
        [EnableIf("isABLoad")]
        [Header("AB包清单列表文件名")]
        public string abVersionList = "versionList.json";

        [EnableIf("isNetUpdate")]
        [EnableIf("isABLoad")]
        [Header("网络路径")]
        [TextArea]
        public string netPath = "http://localhost:56239/AssetBundles/PC/";

        [EnableIf("isNetUpdate")]
        [EnableIf("isABLoad")]
        [Header("更新失败的重试次数")]
        public int retryCount = 3;

        public string GetLocalPath()
        {
            if (isStreamingAssets)
                return Path.Combine(Application.streamingAssetsPath, localPath);
            else return localPath;
        }
        #endregion
    }
}