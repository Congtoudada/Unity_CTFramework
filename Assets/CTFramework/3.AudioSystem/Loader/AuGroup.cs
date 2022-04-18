/****************************************************
  文件：AuGroup.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 21:54:38
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.AuSys
{
    public class AuGroup : IRelease
    {
        //音乐组配置文件
        private AudioCore profile;
        //private AuGroupProfile profile;

        //运行时背景音乐字典
        public Dictionary<string, AudioBG> bgDict;
        //运行时音效字典
        public Dictionary<string, AudioEffect> effectDict;

        //缓存背景音乐剪辑
        //参数①：key通常为音乐剪辑名
        private Dictionary<string, AudioClip> _bgClipDict;
        private Dictionary<string, AudioClip> bgClipDict
        {
            get
            {
                if (_bgClipDict == null)
                    _bgClipDict = new Dictionary<string, AudioClip>(2);
                return _bgClipDict;
            }
        }

        //缓存音效剪辑
        //参数①：key通常为音乐剪辑名
        private Dictionary<string, AudioClip> _effectClipDict;
        private Dictionary<string, AudioClip> effectClipDict
        {
            get
            {
                if (_effectClipDict == null)
                    _effectClipDict = new Dictionary<string, AudioClip>();
                return _effectClipDict;
            }
        }

        public AuGroup(AuGroupProfile profile)
        {
            if (profile == null)
            {
                CTLogger.Warning("致命错误，没有成功加载AuGroupProfile！", SysEnum.AudioSystem);
                return;
            }
            //初始化
            Init(profile);
        }

        private void Init(AuGroupProfile profile)
        {
            //核心配置文件赋值
            this.profile = new AudioCore();
            this.profile.isAssetBundle = profile.isAssetBundle;
            this.profile.resourcesDir_bg = profile.resourcesDir_bg;
            this.profile.resourcesDir_effect = profile.resourcesDir_effect;
            this.profile.abName_bg = profile.abName_bg;
            this.profile.abName_effect = profile.abName_effect;

            bgDict = new Dictionary<string, AudioBG>();
            effectDict = new Dictionary<string, AudioEffect>();

            //执行预加载
            foreach (var item in profile.bgs)
            {
                bgDict[item.clipKey] = item;
                if (item.isPreload) //如果设定了预加载，就需要提前加载进内存
                {
                    PreLoad(item.clipKey, true);
                }
            }
            foreach (var item in profile.effects)
            {
                effectDict[item.clipKey] = item;
                if (item.isPreload) //如果设定了预加载，就需要提前加载进内存
                {
                    PreLoad(item.clipKey, false);
                }
            }
        }

        //同步得到音乐剪辑
        //思路：先看缓存，没有再生成
        public AudioClip GetAudioClip(string clipKey, bool isBG)
        {
            AudioClip clip = null;
            if (isBG) //背景音乐
            {
                //如果已经缓存，则从缓存中取
                if (bgClipDict.ContainsKey(clipKey))
                {
                    clip = bgClipDict[clipKey];
                    //判断是否缓存（如果仅勾选预加载，则不再进行缓存）
                    if (!bgDict[clipKey].isCache)
                        bgClipDict.Remove(clipKey);
                }
                else
                {
                    //没有缓存，则同步加载
                    if (bgDict.ContainsKey(clipKey))
                    {
                        clip = LoadAudioClip(bgDict[clipKey].clipName, isBG);
                        //判断是否需要缓存
                        if (clip != null && bgDict[clipKey].isCache)
                        {
                            bgClipDict.Add(clipKey, clip);
                        }
                    }
                    else
                    {
                        CTLogger.Log("获取背景音乐Clip失败，没有找到Key: " + clipKey, SysEnum.AudioSystem);
                    }
                }
            }
            else  //音效音乐
            {
                //如果已经缓存，则从缓存中取
                if (effectClipDict.ContainsKey(clipKey))
                {
                    clip = effectClipDict[clipKey];
                    //判断是否缓存（如果仅勾选预加载，则不再进行缓存）
                    if (!effectDict[clipKey].isCache)
                        effectDict.Remove(clipKey);
                }
                else
                {
                    if (effectDict.ContainsKey(clipKey))
                    {
                        //没有缓存，则同步加载
                        clip = LoadAudioClip(effectDict[clipKey].clipName, isBG);
                        //判断是否需要缓存
                        if (clip != null && effectDict[clipKey].isCache)
                        {
                            effectClipDict.Add(clipKey, clip);
                        }
                    }
                    else
                    {
                        CTLogger.Log("获取音效Clip失败，没有找到Key: " + clipKey, SysEnum.AudioSystem);
                    }
                }
            }
            return clip;
        }

        //异步得到音乐剪辑并执行回调
        public void GetAudioClipAsync(string clipKey, bool isBG, UnityAction<AudioClip> callback)
        {
            if (isBG)
            {
                //如果已经缓存，直接触发回调
                if (bgClipDict.ContainsKey(clipKey))
                {
                    callback?.Invoke(bgClipDict[clipKey]);
                    return;
                }
                else
                {
                    //没有缓存，则异步加载
                   LoadAudioClipAsync(bgDict[clipKey].clipName, isBG, clip=> {
                       if (clip != null)
                       {
                           callback?.Invoke(clip); //外部回调
                           //判断是否需要缓存
                           if (bgDict[clipKey].isCache)
                               bgClipDict.Add(clipKey, clip);
                       }
                       else
                       {
                           CTLogger.Log("异步加载AudioClip失败: " + bgDict[clipKey].clipName, SysEnum.AudioSystem);
                       }
                   });
                }
            }
            else
            {
                //如果已经缓存，直接触发回调
                if (effectClipDict.ContainsKey(clipKey))
                {
                    callback?.Invoke(effectClipDict[clipKey]);
                    return;
                }
                else
                {
                    //没有缓存，则异步加载
                    LoadAudioClipAsync(effectDict[clipKey].clipName, isBG, clip =>
                    {
                        //判断是否需要缓存
                        if (clip != null && effectDict[clipKey].isCache)
                        {
                            effectClipDict.Add(clipKey, clip);
                            callback?.Invoke(clip); //播放
                        }
                        else
                        {
                            CTLogger.Log("异步加载AudioClip失败: " + bgDict[clipKey].clipName, SysEnum.AudioSystem);
                        }
                    });
                }
            }
        }

        //预加载
        private void PreLoad(string clipKey, bool isBG)
        {
            string clipName = "";
            //1.获取音乐剪辑名称
            if (isBG)
                clipName = bgDict[clipKey].clipName;
            else
                clipName = effectDict[clipKey].clipName;

            //2.动态加载音乐剪辑
            AudioClip tmp = LoadAudioClip(clipName, isBG);

            //3.缓存音乐剪辑
            if (isBG)
            {
                //如果有AudioClip则覆盖，没有则添加
                if (bgClipDict.ContainsKey(clipKey))
                    bgClipDict[clipKey] = tmp;
                else
                    bgClipDict.Add(clipKey, tmp);
            }
            else
            {
                //如果有AudioClip则覆盖，没有则添加
                if (effectClipDict.ContainsKey(clipKey))
                    effectClipDict[clipKey] = tmp;
                else
                    effectClipDict.Add(clipKey, tmp);
            }
        }

        //同步加载音乐剪辑
        //参数①由外界判断有效性
        private AudioClip LoadAudioClip(string clipKey, bool isBG)
        {
            AudioClip result = null;
            //1.获取加载信息
            string prefix = GetLoadPrefix(clipKey, isBG);
            string clipName = "";
            if (isBG)
            {
                clipName = bgDict[clipKey].clipName;
            }
            else
            {
                clipName = effectDict[clipKey].clipName;
            }
            //2.动态加载音乐剪辑
            //AssetBundle加载
            if (profile.isAssetBundle)
            {
                result = _CT.ResMgr.LoadRes<AudioClip>(prefix, clipName);
                if (result == null)
                    CTLogger.Log($"加载音乐剪辑失败，请检查ab包: { prefix }的音乐剪辑: { clipName }", SysEnum.AudioSystem);
            }
            //Resources加载
            else
            {
                result = Resources.Load<AudioClip>(Path.Combine(prefix, clipName));
                if (result == null)
                    CTLogger.Log($"加载音乐剪辑失败，请检查Resources路径: { Path.Combine(prefix, clipName) }", SysEnum.AudioSystem);
            }
            return result;
        }

        //异步加载音乐剪辑
        //参数①由外界判断有效性
        private void LoadAudioClipAsync(string clipKey, bool isBG, UnityAction<AudioClip> callback)
        {
            //1.获取加载信息
            string prefix = GetLoadPrefix(clipKey, isBG);
            string clipName = "";
            if (isBG)
            {
                clipName = bgDict[clipKey].clipName;
            }
            else
            {
                clipName = effectDict[clipKey].clipName;
            }
            //2.动态加载音乐剪辑
            //AssetBundle加载
            if (profile.isAssetBundle)
            {
                _CT.ResMgr.LoadResAsync(prefix, clipName, callback);
            }
            //Resources加载
            else
            {
                _CT.ResMgr.LoadResAsync(Path.Combine(prefix, clipName), callback);
            }
        }

        //获取加载前缀，AB包就是AB包名，Resouces就是Resources所在目录
        //参数①由外界判断有效性
        private string GetLoadPrefix(string clipKey, bool isBG)
        {
            string prefix = "";
            //如果是AB包加载，获取主包名
            if (profile.isAssetBundle)
            {
                //音乐类型
                if (isBG)
                {
                    //判断路径重载
                    if (bgDict[clipKey].isOverride)
                    {
                        prefix = bgDict[clipKey].overrideABName;
                    }
                    else
                    {
                        prefix = profile.abName_bg;
                    }
                }
                else
                {
                    //判断路径重载
                    if (effectDict[clipKey].isOverride)
                    {
                        prefix = effectDict[clipKey].overrideABName;
                    }
                    else
                    {
                        prefix = profile.abName_effect;
                    }
                }
            }
            else //如果是Resources加载，获取所在目录
            {
                //音乐类型
                if (isBG)
                {
                    //判断路径重载
                    if (bgDict[clipKey].isOverride)
                    {
                        prefix = bgDict[clipKey].overrideResDir;
                    }
                    else
                    {
                        prefix = profile.resourcesDir_bg;
                    }
                }
                else
                {
                    //判断路径重载
                    if (effectDict[clipKey].isOverride)
                    {
                        prefix = effectDict[clipKey].overrideResDir;
                    }
                    else
                    {
                        prefix = profile.resourcesDir_effect;
                    }
                }
            }
            return prefix;
        }

        public void Release()
        {
            bgDict.Clear();
            bgDict = null;
            effectDict.Clear();
            effectDict = null;
            _bgClipDict.Clear();
            _bgClipDict = null;
            _effectClipDict.Clear();
            _effectClipDict = null;
            profile = null;
        }
    }
}