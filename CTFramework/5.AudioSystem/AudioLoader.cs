/****************************************************
  文件：AudioLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 22:25:15
  功能：主要负责2D音乐，一个音乐加载器控制一个AudioSource，负责一种类型的音乐
*****************************************************/
using CT.ResourceSys;
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AudioSys
{
    public class AudioLoader : IRelease
    {
        private LoadBaseProfile loadProfile; //加载配置文件，传入AuABProfile或AuResProfile

        private AudioProfile profile; //音乐配置文件

        public AudioSource source; //外界分配的音源

        public Dictionary<string, AudioClip> cacheMusic;//主音乐缓存，不建议多，占用内存

        public Dictionary<string, AudioClip> cacheEffect;//缓存音效

        //参数：音乐Profile的完整路径
        public AudioLoader(string path, AudioSource source, LoadBaseProfile loadProfile)
        {
            cacheMusic = new Dictionary<string, AudioClip>();
            cacheEffect = new Dictionary<string, AudioClip>();
            profile = Resources.Load<AudioProfile>(path);
            this.loadProfile = loadProfile;
            this.source = source;

            //预加载
            PreLoad();
        }

        //预加载（预缓存）
        public void PreLoad()
        {
            //路径覆盖
            if(loadProfile is AuResProfile)
            {
                AuResProfile arp = loadProfile as AuResProfile;
                if(profile.isOverrideDirPath)
                {
                    arp.dirPath = profile.overrideClipDirPath;
                }
            }

            AudioClip clip = null;
            //背景音乐预缓存
            foreach(string key in profile.bgDict.Keys)
            {
                if (profile.bgDict[key].isPreload)
                {
                    loadProfile.filename = profile.bgDict[key].clipName;
                    clip = loadProfile.Load<AudioClip>();
                    AddCache(key, clip, true);
                }
            }
            //音效预缓存
            foreach (string key in profile.effectDict.Keys)
            {
                if (profile.effectDict[key].isPreload)
                {
                    loadProfile.filename = profile.effectDict[key].clipName;
                    clip = loadProfile.Load<AudioClip>();
                    AddCache(key, clip, false);
                }
            }
        }

        //根据key播放音乐
        //参数①：musicDict对应的key
        //参数②：外部音量 参数
        //参数③：是否循环播放 参数
        //参数④：是否异步加载 参数
        //参数⑤：是否缓存
        public void Play(string key, float volumn, bool isLoop = true, bool isAsync = false, bool isCache = false)
        {
            //检查资源合法性
            if(profile.bgDict.ContainsKey(key))
            {
                AudioItem item = profile.bgDict[key];
                //设置播放器参数
                source.volume = volumn * item.volumn;
                source.loop = isLoop;
                //查看是否缓存过，是的话直接播放
                if (cacheMusic.ContainsKey(key))
                {
                    source.clip = cacheMusic[key];
                    source.Play();
                }
                else //没有缓存过，加载并播放
                {
                    //加载音乐
                    loadProfile.filename = item.clipName;
                    //同步加载并播放
                    if (!isAsync)
                    {
                        AudioClip clip = loadProfile.Load<AudioClip>();
                        if (clip != null)
                        {
                            source.clip = clip;
                            source.Play();
                            if (isCache)
                                AddCache(key, clip, true);
                        }
                    }
                    else //异步加载并播放
                    {
                        loadProfile.LoadAsync<AudioClip>(res =>
                        {
                            if (res != null)
                            {
                                source.clip = res;
                                source.Play();
                                if (isCache)
                                    AddCache(key, res, true);
                            }
                        });
                    }
                }
            }
            else
            {
                DebugMgr.Warning("在bgDict中没有找到Key: " + key, SystemEnum.AudioSystem);
            }
        }
        //根据key播放音效（不会影响当前AudioSource）
        //参数①：effectDict对应的key 
        //参数②：外部音量 参数
        //参数③：是否缓存，便于下次使用
        public void PlayOneShot(string key, float volumn, bool isCache = true)
        {
            //检查资源合法性
            if (profile.effectDict.ContainsKey(key))
            {
                AudioItem item = profile.effectDict[key];
                //查看是否缓存过，是的话直接播放
                if (cacheEffect.ContainsKey(key))
                {
                    source.PlayOneShot(cacheEffect[key], volumn * item.volumn);
                }
                else //没有缓存过，加载并播放
                {
                    //加载音乐
                    loadProfile.filename = item.clipName;
                    //同步加载音效并播放
                    AudioClip clip = loadProfile.Load<AudioClip>();
                    if (clip != null)
                    {
                        source.PlayOneShot(clip, volumn * item.volumn);
                        if (isCache)
                            AddCache(key, clip, false);
                    }
                }
            }
            else
            {
                DebugMgr.Warning("在bgDict中没有找到Key: " + key, SystemEnum.AudioSystem);
            }
        }
        //随机播放音乐
        public void PlayRandom(float volumn, bool isLoop = true, bool isAsync = false)
        {
            Random.InitState((int)TimeTool.GetTimeStamp());
            int range = Random.Range(0, profile.bgDict.Count);
            int index = 0;
            foreach(string key in profile.bgDict.Keys)
            {
                if(index == range)
                {
                    Play(key, volumn, isLoop, isAsync, false);
                }
                index++;
            }
        }

        //增加缓存
        private void AddCache(string key, AudioClip clip, bool isMusic)
        {
            if(isMusic)
            {
                if(cacheMusic.Count >= _CT.AuMgr.coreProfile.maxMusic) //缓存已满，会全部清空
                {
                    cacheMusic.Clear();
                }
                if(!cacheMusic.ContainsKey(key))
                {
                    cacheMusic.Add(key, clip);
                }
            }
            else
            {
                if(!cacheEffect.ContainsKey(key))
                {
                    cacheEffect.Add(key, clip);
                }
            }
        }
        //取消缓存
        private void RemoveCache(string key, bool isMusic)
        {
            if(isMusic)
            {
                if (cacheMusic.ContainsKey(key))
                {
                    cacheMusic.Remove(key);
                }
            }
            else
            {
                if(cacheEffect.ContainsKey(key))
                {
                    cacheEffect.Remove(key);
                }
            }
        }

        public void Release()
        {
            loadProfile = null; //加载配置文件，传入AuABProfile或AuResProfile
            profile = null; //音乐配置文件
            source = null; //外界分配的音源
            cacheMusic.Clear();//主音乐缓存，不建议多，占用内存
            cacheMusic = null;
            cacheEffect.Clear();//缓存音效
            cacheEffect = null;
    }
    }
}
