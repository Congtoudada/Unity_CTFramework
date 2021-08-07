/****************************************************
  文件：AudioProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 22:30:06
  功能：Nothing
*****************************************************/
using CT.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AudioSys
{
    [CreateAssetMenu(menuName = "CT/AudioSystem/AudioProfile", fileName ="AudioProfile")]
    public class AudioProfile : SerializedScriptableObject
    {
        [Title("背景音乐集合")]
        [ShowInInspector]
        public Dictionary<string, AudioItem> bgDict = new Dictionary<string, AudioItem>();

        [Title("音效集合")]
        [ShowInInspector] 
        public Dictionary<string, AudioItem> effectDict = new Dictionary<string, AudioItem>();

        [BoxGroup("Resources加载路径覆盖")]
        [Title("是否覆盖AudioCoreProfile中ClipDirPath的目录配置")]
        [LabelText("是否覆盖")]
        public bool isOverrideDirPath = false;

        [BoxGroup("Resources加载路径覆盖")]
        [EnableIf("isOverrideDirPath")]
        [LabelText("覆盖路径")]
        public string overrideClipDirPath = CTConstant.AUDIO_CLIP_DIR_PATH;

    }
}
