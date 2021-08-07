/****************************************************
  文件：AudioMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 22:22:20
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using CT.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.AudioSys
{
    public class AudioMgr : IRelease
    {
        //Audio核心配置文件
        public AudioCoreProfile coreProfile;
        //音乐加载器
        private Dictionary<string, AudioLoader> loaderDict;

        public AudioMgr()
        {
            loaderDict = new Dictionary<string, AudioLoader>();
            coreProfile = Resources.Load<AudioCoreProfile>(CTConstant.AU_CORE_PROFILE_PATH);
            if (coreProfile == null)
                DebugMgr.Error("没有找到音乐核心配置文件，请检查路径: " + CTConstant.AU_CORE_PROFILE_PATH, 
                    SystemEnum.AudioSystem);
        }

        //新增一个音乐加载器
        public void AddAudioLoader(string loadKey, string profileName, LoadBaseProfile loadProfile)
        {
            if(!loaderDict.ContainsKey(loadKey))
            {
                //生成一个空游戏对象在_CT之下，并挂载上AudioSouce组件
                GameObject obj = new GameObject(loadKey);
                obj.transform.parent = _CT.Instance.transform;
                AudioSource source = obj.AddComponent<AudioSource>();

                string profilePath = Path.Combine(_CT.AuMgr.coreProfile.profileDirPath, profileName);
                AudioLoader loader = new AudioLoader(profilePath, source, loadProfile); 

                loaderDict.Add(loadKey, loader);
            }
        }

        //卸载一个音乐加载器
        public void RemoveAudioLoader(string loadKey)
        {
            if(loaderDict.ContainsKey(loadKey))
            {
                ReleaseTool.TryRelease(loaderDict[loadKey]);
                loaderDict.Remove(loadKey);
            }
        }

        //得到一个音乐加载器
        public AudioLoader GetAudioLoader(string loadKey)
        {
            if (loaderDict.ContainsKey(loadKey))
                return loaderDict[loadKey];
            DebugMgr.Warning("没有找到音乐加载器: " + loadKey, SystemEnum.AudioSystem);
            return null;
        }

        //根据key播放音乐
        //参数①：loadKey, 选择加载器
        //参数②：clipKey, musicDict对应的key
        //参数③：volumn, 音量
        //参数④：isLoop, 是否循环播放
        //参数⑤：isAsync, 是否异步播放
        //参数⑥：isCache, 是否缓存
        public void Play(string loadKey, string clipKey, float volumn, bool isLoop = true, bool isAsync = false, bool isCache = false)
        {
            if (loaderDict.ContainsKey(loadKey))
                loaderDict[loadKey].Play(clipKey, volumn, isLoop, isAsync, isCache);
            else
                DebugMgr.Warning("没有找到音乐加载器: " + loadKey, SystemEnum.AudioSystem);
        }
        
        //根据key播放音效（不会影响当前AudioSource）
        //参数①：loadKey, 选择加载器
        //参数②：clipKey, effectDict对应的key 
        //参数③：volumn, 音量
        //参数④：isCache, 是否缓存，便于下次使用
        public void PlayOneShot(string loadKey, string clipKey, float volumn, bool isCache = true)
        {
            if (loaderDict.ContainsKey(loadKey))
                loaderDict[loadKey].PlayOneShot(clipKey, volumn, isCache);
            else
                DebugMgr.Warning("没有找到音乐加载器: " + loadKey, SystemEnum.AudioSystem);
        }
        
        //随机播放音乐
        public void PlayRandom(string loadKey, float volumn, bool isLoop = true, bool isAsync = false)
        {
            if (loaderDict.ContainsKey(loadKey))
                loaderDict[loadKey].PlayRandom(volumn, isLoop, isAsync);
            else
                DebugMgr.Warning("没有找到音乐加载器: " + loadKey, SystemEnum.AudioSystem);
        }

        public void Release()
        {
            foreach(string key in loaderDict.Keys)
            {
                RemoveAudioLoader(key);
            }
            loaderDict = null;
            coreProfile = null;
        }
    }
}
