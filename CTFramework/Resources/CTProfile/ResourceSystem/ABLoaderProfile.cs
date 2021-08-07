/****************************************************
  文件：ABProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/28 13:14:07
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.ResourceSys
{
    [CreateAssetMenu(menuName = "CT/ResourceSystem/ABLoaderProfile", fileName = "ABLoaderProfile")]
    public class ABLoaderProfile : SerializedScriptableObject
    {
        [Header("网络路径")]
        [TextArea]
        public string NetPath = "http://localhost:56239/AssetBundles/PC/";

        [Header("AB包本地路径")]
        [TextArea]
        public string LocalPath = Path.Combine(Application.streamingAssetsPath, "AssetBundle");

        [Header("主包名")]
#if UNITY_ANDROID
    public string MainABName = "Android";
#elif UNITY_IOS
    public string MainABName = "IOS";
#else
        public string MainABName = "PC";
#endif

        //[Header("Debug前缀")]
        //public string Prefix_Debug = "[ ResourceSystem-ABLoader ]";
    }
}
