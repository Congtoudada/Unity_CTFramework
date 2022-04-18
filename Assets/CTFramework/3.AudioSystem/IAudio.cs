/****************************************************
  文件：IAudio.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/20 17:10:15
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AuSys
{
    public interface IAudio : IRelease
    {
        //设置音乐组
        //参数①：音乐组配置文件名称
        void SetGroup(string groupName);

        //设置音乐组
        //参数①：音乐组配置文件Resources加载目录或ab包名
        //参数②：音乐组配置文件名称
        void SetGroup(string resDir, string groupName);

        //根据key播放音乐
        //参数①：clipKey, musicDict对应的key
        //参数②：volumn, 音量
        //参数③：是否是环境音
        void Play(string clipKey, float volumn, bool isEnv = false);

        //根据key播放音效（不会影响当前AudioSource）
        //参数①：clipKey, effectDict对应的key 
        //参数②：volumn, 音量
        void PlayOneShot(string clipKey, float volumn);

        //在随机组内随机播放音乐
        //参数①：volumn, 音量
        //参数②：随机组ID(-1代表全部随机）
        void PlayRandom(float volumn, int randomID = -1);

        //随机播放音效（不会影响当前AudioSource）
        //参数①：volumn, 音量
        //参数②：随机组ID(-1代表全部随机）
        void PlayOneShotRandom(float volumn, int randomID = -1);

        //暂停播放
        //参数①：是否是环境音
        void Pause(bool isEnv = false);

        //继续播放
        //参数①：是否是环境音
        void Resume(bool isEnv = false);

        //停止播放
        //参数①：是否是环境音
        void Stop(bool isEnv);

        //得到AudioSource
        //参数①：是否是环境音
        AudioSource GetAudioSource(bool isEnv);

        //设置音量（如果有，该音量会和系统音量做乘积）
        //参数①：音量值
        void SetVolume(float volume, bool isEnv);

        //获取音量大小（直接返回当前Source的音量值）
        //参数①：是否是环境音
        float GetVolume(bool isEnv);
        
        //设置系统音量
        //参数①：音量
        //参数②：音量类型
        void SetSystemVolume(float volume, VolumeType type);
        
        //得到系统音量
        //参数①：音量类型
        float GetSystemVolume(VolumeType type);
        
        //得到音乐组配置文件的音量
        float GetProfileVolume(VolumeType type, string key);
    }
}