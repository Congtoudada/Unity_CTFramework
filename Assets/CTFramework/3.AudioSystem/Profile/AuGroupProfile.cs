/****************************************************
  文件：AuGroupProfile.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 21:31:42
  功能：
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.AuSys
{
    [CreateAssetMenu(menuName = "CT/AuGroupProfile", fileName = "AuGroupProfile", order = 46)]
    public class AuGroupProfile : ScriptableObject 
    {
        [Title("是否启用AB包加载AudioClip")]
        public bool isAssetBundle;

        [HideIf("isAssetBundle")]
        [Title("背景音乐所在Resources目录")]
        [FolderPath]
        [OnValueChanged("OnResoucesBgChanged")]
        public string resourcesDir_bg = "CTFramework/Example/AuSys/Bg";

        [HideIf("isAssetBundle")]
        [Title("音效所在Resources目录")]
        [FolderPath]
        [OnValueChanged("OnResoucesEffectChanged")]
        public string resourcesDir_effect = "CTFramework/Example/AuSys/Effect";

        [ShowIf("isAssetBundle")]
        [Title("背景音乐所在主包(AssetBundle)")]
        public string abName_bg;

        [ShowIf("isAssetBundle")]
        [Title("音效所在主包(AssetBundle)")]
        public string abName_effect;

        [Button("刷新")]
        private void Refresh()
        {
            foreach (var item in bgs)
            {
                item.OnTitleChanged();
                item.isAssetBundle = isAssetBundle;
            }
            foreach (var item in effects)
            {
                item.OnTitleChanged();
                item.isAssetBundle = isAssetBundle;
            }
        }

        [TabGroup("AudioBG")]
        [LabelText("背景音乐集合")]
        [OnValueChanged("OnBgsChanged")]
        public List<AudioBG> bgs;

        [TabGroup("AudioEffect")]
        [LabelText("音效集合")]
        [OnValueChanged("OnEffectsChanged")]
        public List<AudioEffect> effects;

        private void OnResoucesBgChanged()
        {
            resourcesDir_bg = EditorHelper.GetResourcesPath(resourcesDir_bg);
        }

        private void OnResoucesEffectChanged()
        {
            resourcesDir_effect = EditorHelper.GetResourcesPath(resourcesDir_effect);
        }

        private void OnBgsChanged()
        {
            if (bgs.Count >= 1)
                bgs[bgs.Count - 1].isAssetBundle = isAssetBundle;
        }

        private void OnEffectsChanged()
        {
            if (effects.Count >= 1)
                effects[effects.Count - 1].isAssetBundle = isAssetBundle;
        }
    }

    //运行时音乐配置文件核心数据
    public class AudioCore
    {
        public bool isAssetBundle;
        public string resourcesDir_bg;
        public string resourcesDir_effect;
        public string abName_bg;
        public string abName_effect;
    }

    //背景音乐对象
    [Serializable]
    public class AudioBG
    {
        [FoldoutGroup("$title")]
        [Title("背景音乐的Key，用于唯一标识")]
        [OnValueChanged("OnTitleChanged")]
        public string clipKey;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑名，用于动态加载")]
        [FilePath]
        [OnValueChanged("OnClipNameChanged")]
        [OnValueChanged("OnTitleChanged")]
        public string clipName;

        [FoldoutGroup("$title")]
        [Title("音量")]
        public float volumn = 1f;

        [FoldoutGroup("$title")]
        [Title("是否循环播放")]
        public bool loop;

        [FoldoutGroup("$title")]
        [Range(0, 32)]
        [Title("随机组ID，用于同组内容的随机播放")]
        public int randomGroupID;

        [FoldoutGroup("$title")]
        [Title("是否同步预加载，自动缓存（背景音乐不建议启用）")]
        public bool isPreload = false;

        [FoldoutGroup("$title")]
        [Title("使用时是否缓存（背景音乐不建议启用）")]
        [Tooltip("内存消耗大，通常不需要")]
        public bool isCache = false;

        [FoldoutGroup("$title")]
        [Title("是否使用异步加载")]
        [DisableIf("isPreload")]
        public bool isAsync = false;

        [FoldoutGroup("$title")]
        [Title("是否使用路径重载")]
        public bool isOverride = false;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑Resources加载路径（用于重载默认路径）")]
        [FolderPath]
        [OnValueChanged("OnOverrideResPathChanged")]
        [HideIf("isAssetBundle")]
        [EnableIf("isOverride")]
        public string overrideResDir;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑重载AB包")]
        [ShowIf("isAssetBundle")]
        [EnableIf("isOverride")]
        public string overrideABName;


        [HideInInspector]
        public bool isAssetBundle; //是否AB包加载

        private string title = "背景音乐: ";

        public void OnClipNameChanged()
        {
            clipName = Path.GetFileNameWithoutExtension(clipName);
            if (string.IsNullOrEmpty(clipKey))
                clipKey = clipName;
        }

        public void OnTitleChanged()
        {
            title = "背景音乐: " + clipKey + "-" + clipName;
            title = title.Substring(0, Mathf.Min(24, title.Length));
        }

        public void OnOverrideResPathChanged()
        {
            overrideResDir = EditorHelper.GetResourcesPath(overrideResDir);
        }
    }

    //音效音乐对象
    [Serializable]
    public class AudioEffect
    {
        [FoldoutGroup("$title")]
        [Title("音效的Key，用于唯一标识")]
        [OnValueChanged("OnTitleChanged")]
        public string clipKey;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑名，用于动态加载")]
        [FilePath]
        [OnValueChanged("OnClipNameChanged")]
        [OnValueChanged("OnTitleChanged")]
        public string clipName;

        [FoldoutGroup("$title")]
        [Title("音量")]
        public float volumn = 1f;

        [FoldoutGroup("$title")]
        [Range(0, 32)]
        [Title("随机组ID，用于同组内容的随机播放")]
        public int randomGroupID;

        [FoldoutGroup("$title")]
        [Title("是否同步预加载，自动缓存")]
        public bool isPreload = true;

        [FoldoutGroup("$title")]
        [Title("使用时是否缓存")]
        public bool isCache = true;

        [FoldoutGroup("$title")]
        [Title("是否使用异步加载")]
        [DisableIf("isPreload")]
        public bool isAsync = false;

        [FoldoutGroup("$title")]
        [Title("是否使用路径重载")]
        public bool isOverride = false;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑Resources加载路径（用于重载默认路径）")]
        [FolderPath]
        [OnValueChanged("OnOverrideResPathChanged")]
        [HideIf("isAssetBundle")]
        [EnableIf("isOverride")]
        public string overrideResDir;

        [FoldoutGroup("$title")]
        [Title("音乐剪辑重载AB包")]
        [ShowIf("isAssetBundle")]
        [EnableIf("isOverride")]
        public string overrideABName;

        [HideInInspector]
        public bool isAssetBundle; //是否AB包加载

        private string title = "音效音乐：";

        public void OnClipNameChanged()
        {
            clipName = Path.GetFileNameWithoutExtension(clipName);
            if (string.IsNullOrEmpty(clipKey))
                clipKey = clipName;
        }

        public void OnTitleChanged()
        {
            title = "音效音乐: " + clipKey + "-" + clipName;
            title = title.Substring(0, Mathf.Min(24, title.Length));
        }

        public void OnOverrideResPathChanged()
        {
            overrideResDir = EditorHelper.GetResourcesPath(overrideResDir);
        }
    }
}