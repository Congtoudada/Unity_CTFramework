/****************************************************
  文件：TransitionProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/3 22:52:22
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AISys
{
    [CreateAssetMenu(menuName = "CT/AISystem/TransitionProfile", fileName = "TransitionProfile")]
    public class TransitionProfile : SerializedScriptableObject
    {
        [ShowInInspector]
        [DictionaryDrawerSettings(KeyLabel = "过渡", ValueLabel = "输出状态")]
        public Dictionary<Transition, StateID> transition = new Dictionary<Transition, StateID>();
    }
}
