/****************************************************
  文件：AudioItem.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 22:36:20
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AudioSys
{
    [Serializable]
    public class AudioItem
    {
        [LabelText("参考的标准音量")]
        [Range(0,1f)]
        public float volumn = 1f;

        [LabelText("音乐剪辑名称（用于加载）")]
        public string clipName = "";

        [LabelText("是否预加载")]
        public bool isPreload = false;

        [LabelText("描述信息")]
        public string description;
    }
}

