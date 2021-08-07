/****************************************************
  文件：AudioCoreProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 22:58:51
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AudioSys
{
    [CreateAssetMenu(menuName = "CT/AudioSystem/AudioCoreProfile", fileName = "AudioCoreProfile")]
    public class AudioCoreProfile : SerializedScriptableObject
    {
        [Title("音乐音量")]
        [Range(0, 1f)]
        [BoxGroup("基本参数")]
        public float musicVolumn = 1f;

        [Title("音效音量")]
        [Range(0, 1f)]
        [BoxGroup("基本参数")]
        public float effectVolumn = 1f;

        [Title("背景音乐缓存上限")]
        [BoxGroup("基本参数")]
        public int maxMusic = 3;

        [Title("音乐配置文件所在Resources目录")]
        public string profileDirPath = CTConstant.AUDIO_PROFILE_DIR_PATH;

        [Title("音乐剪辑所在Resources目录")]
        public string clipDirPath = CTConstant.AUDIO_CLIP_DIR_PATH;
    }
}
