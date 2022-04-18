/****************************************************
  文件：AuLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 21:28:25
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CT.AuSys
{
    public class AuLoader : IAudio
    {
        //当前音乐组，每次设置新音乐组时会覆盖
        private AuGroup group;
        private string currentGroup = ""; //记录当前音乐组名称

        //1号音源，通常用于播放背景音乐
        private AudioSource source1;
        //2号音源，通常用于播放环境音乐
        private AudioSource source2;
        //音频全局配置文件
        private AuProfile profile;

        public AuLoader(AuProfile profile, AudioSource source1, AudioSource source2)
        {
            this.profile = profile;
            this.source1 = source1;
            this.source2 = source2;
        }

        public void SetGroup(string groupName)
        {
            if (groupName.Equals(currentGroup)) return;
            group = AuFactory.CreateAuGroup(groupName);
            currentGroup = groupName;
        }

        public void SetGroup(string resDir, string groupName)
        {
            if (groupName.Equals(currentGroup)) return;
            group = AuFactory.CreateAuGroup(resDir, groupName);
            currentGroup = groupName;
        }

        public void Play(string clipKey, float volumn, bool isEnv = false)
        {
            if (Check())
            {
                if (group.bgDict.ContainsKey(clipKey))
                {
                    AudioSource source = null;
                    float sysVolume = 1;
                    if (isEnv)
                    {
                        source = source2;
                        sysVolume = profile.envVolume;
                    }
                    else
                    {
                        source = source1;
                        sysVolume = profile.bgVolume;
                    }
                    AudioBG bg = group.bgDict[clipKey];
                    if (bg.isAsync) //异步播放
                    {
                        group.GetAudioClipAsync(clipKey, true, clip => {
                            source.volume = bg.volumn * volumn * sysVolume; //配置文件音量 * 传入音量 * 系统音量
                            source.loop = bg.loop;
                            source.clip = clip;
                            source.Play();
                        });
                    }
                    else //同步播放
                    {
                        AudioClip clip = group.GetAudioClip(clipKey, true);
                        if (clip == null) return;
                        source.volume = bg.volumn * volumn * sysVolume; //配置文件音量 * 传入音量 * 系统音量
                        source.loop = bg.loop;
                        source.clip = clip;
                        source.Play();
                    }
                }
                else
                {
                    CTLogger.Log("背景音乐clipKey不合法: " + clipKey, SysEnum.AudioSystem);
                }
            }
        }

        public void PlayOneShot(string clipKey, float volumn)
        {
            if (Check())
            {
                if (group.effectDict.ContainsKey(clipKey))
                {
                    AudioEffect effect = group.effectDict[clipKey];
                    if (effect.isAsync)
                    {
                        group.GetAudioClipAsync(clipKey, false, clip =>
                        {
                            if (clip != null)
                            {
                                float vol = volumn * effect.volumn * profile.effectVolume; //配置文件音量 * 传入音量 * 系统音量
                                source1.PlayOneShot(clip, vol);
                            }
                        });
                    }
                    else
                    {
                        AudioClip clip = group.GetAudioClip(clipKey, false);
                        if (clip == null) return;
                        float vol = volumn * group.effectDict[clipKey].volumn;
                        source1.PlayOneShot(clip, vol);
                    }
                }
                else
                {
                    CTLogger.Log("音效clipKey不合法: " + clipKey, SysEnum.AudioSystem);
                }
            }
        }

        public void PlayRandom(float volumn, int randomID = -1)
        {
            Random.InitState((int)Tools.TimeTool.GetTimeStamp());
            if (randomID == -1)
            {
                int random = Random.Range(1, group.bgDict.Count + 1);
                int index = 0;
                foreach (string clipKey in group.bgDict.Keys)
                {
                    if (++index == random)
                    {
                        Play(clipKey, volumn);
                        return;
                    }
                }
            }
            else
            {
                var keys = group.bgDict.Values.Where(v => v.randomGroupID == randomID).Select(v => v.clipKey).ToList();
                if (keys.Count == 0)
                {
                    CTLogger.Log("输入的RandomID不合法: " + randomID, SysEnum.AudioSystem);
                    return;
                }
                int random = Random.Range(0, keys.Count);
                Play(keys[random], volumn);
            }
        }

        public void PlayOneShotRandom(float volumn, int randomID = -1)
        {
            Random.InitState((int)Tools.TimeTool.GetTimeStamp());
            if (randomID == -1)
            {
                int random = Random.Range(1, group.effectDict.Count + 1);
                int index = 0;
                foreach (string clipKey in group.effectDict.Keys)
                {
                    if (++index == random)
                    {
                        PlayOneShot(clipKey, volumn);
                        return;
                    }
                }
            }
            else
            {
                var keys = group.effectDict.Values.Where(v => v.randomGroupID == randomID).Select(v => v.clipKey).ToList();
                if (keys.Count == 0)
                {
                    CTLogger.Log("输入的RandomID不合法: " + randomID, SysEnum.AudioSystem);
                    return;
                }
                int random = Random.Range(0, keys.Count);
                PlayOneShot(keys[random], volumn);
            }
        }

        public void Pause(bool isEnv)
        {
            if (isEnv)
                source2.Pause();
            else
                source1.Pause();
        }

        public void Resume(bool isEnv)
        {
            if (isEnv)
                source2.UnPause();
            else
                source1.UnPause();
        }

        public void Stop(bool isEnv)
        {
            if (isEnv)
                source2.Stop();
            else
                source1.Stop();
        }

        //合法性检查
        private bool Check()
        {
            if (source1 == null || source2 == null)
            {
                CTLogger.Warning("AudioSource为null，请赋值", SysEnum.AudioSystem);
                return false;
            }
            if (group == null)
            {
                CTLogger.Warning("没有找到AuGroup，请加载", SysEnum.AudioSystem);
                return false;
            }
            return true;
        }

        public AudioSource GetAudioSource(bool isEnv)
        {
            if (!isEnv)
                return source1;
            else
                return source2;
        }

        //设置音量
        public void SetVolume(float volume, bool isEnv)
        {
            if (!isEnv)
                source1.volume = volume;
            else
                source2.volume = volume;
        }
        
        //获取音量大小
        public float GetVolume(bool isEnv)
        {
            if (!isEnv)
                return source1.volume;
            else
                return source2.volume;
        }
        
        //设置系统音量
        public void SetSystemVolume(float volume, VolumeType type)
        {
            switch(type)
            {
                case VolumeType.BG:
                    profile.bgVolume = volume;
                    break;
                case VolumeType.Env:
                    profile.envVolume = volume;
                    break;
                case VolumeType.Effect:
                    profile.effectVolume = volume;
                    break;
            }
        }

        //得到系统音量
        public float GetSystemVolume(VolumeType type)
        {
            switch(type)
            {
                case VolumeType.BG:
                    return profile.bgVolume;
                case VolumeType.Env:
                    return profile.envVolume;
                case VolumeType.Effect:
                    return profile.effectVolume;
            }
            return 1.0f;
        }

        public float GetProfileVolume(VolumeType type, string key)
        {
            switch (type)
            {
                case VolumeType.BG:
                case VolumeType.Env:
                    if (@group.bgDict.TryGetValue(key, out AudioBG bg))
                        return bg.volumn;
                    break;
                case VolumeType.Effect:
                    if (@group.effectDict.TryGetValue(key, out AudioEffect effect))
                        return effect.volumn;
                    break;
            }
            return 1.0f;
        }

        public void Release()
        {
            ReleaseTool.TryRelease(group);
            group = null;
            source1.Stop();
            source1 = null;
            source2.Stop();
            source2 = null;
        }
    }
}