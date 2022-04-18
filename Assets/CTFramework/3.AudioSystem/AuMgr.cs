/****************************************************
  文件：AuMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 21:27:44
  功能：
*****************************************************/
using CT.AuSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    //剪辑Key的"枚举"类
    // public partial class AuKeyEnum
    // { }

    public class AuMgr : IAudio
    {
        private IAudio loader;

        public void SetGroup(string groupName)
        {
            loader.SetGroup(groupName);
        }

        public void SetGroup(string resDir, string groupName)
        {
            loader.SetGroup(resDir, groupName);
        }

        public void SetLoader(IAudio loader)
        {
            this.loader = loader;
        }

        public void Play(string clipKey, float volumn, bool isEnv = false)
        {
            loader.Play(clipKey, volumn);
        }

        public void PlayOneShot(string clipKey, float volumn)
        {
            loader.PlayOneShot(clipKey, volumn);
        }

        public void PlayOneShotRandom(float volumn, int randomID = -1)
        {
            loader.PlayOneShotRandom(volumn, randomID);
        }

        public void PlayRandom(float volumn, int randomID = -1)
        {
            loader.PlayRandom(volumn, randomID);
        }

        public void Pause(bool isEnv = false)
        {
            loader.Pause(isEnv);
        }

        public void Resume(bool isEnv = false)
        {
            loader.Resume(isEnv);
        }

        public void Stop(bool isEnv = false)
        {
            loader.Stop(isEnv);
        }

        public AudioSource GetAudioSource(bool isEnv = false)
        {
            return loader.GetAudioSource(isEnv);
        }
        
        //获取音量大小
        public float GetVolume(bool isEnv)
        {
            return loader.GetVolume(isEnv);
        }
        
        //设置系统音量
        public void SetSystemVolume(float volume, VolumeType type)
        {
            loader.SetSystemVolume(volume, type);
        }

        public void SetVolume(float volume, bool isEnv = false)
        {
            loader.SetVolume(volume, isEnv);
        }

        //得到系统音量
        public float GetSystemVolume(VolumeType type)
        {
            return loader.GetSystemVolume(type);
        }

        public float GetProfileVolume(VolumeType type, string key)
        {
            return loader.GetProfileVolume(type, key);
        }

        public void Release()
        {
            ReleaseTool.TryRelease(loader);
            loader = null;
        }
    }
}