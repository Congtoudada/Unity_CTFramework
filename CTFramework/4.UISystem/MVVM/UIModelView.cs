/****************************************************
  文件：UIModelView.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 9:40:45
  功能：Nothing
*****************************************************/
using CT.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    [CreateAssetMenu(menuName = "CT/UISystem/UIModelView", fileName = "UIModelView")]
    public class UIModelView : SerializedScriptableObject
    {
        [ShowInInspector]
        [Title("string类型的键值对")]
        public Dictionary<string, UIString> stringDict = new Dictionary<string, UIString>();

        [ShowInInspector]
        [Title("int类型的键值对")]
        public Dictionary<string, UIInt> intDict = new Dictionary<string, UIInt>();

        [ShowInInspector]
        [Title("float类型的键值对")]
        public Dictionary<string, UIFloat> floatDict = new Dictionary<string, UIFloat>();

        [ShowInInspector]
        [Title("bool类型的键值对")]
        public Dictionary<string, UIBool> boolDict = new Dictionary<string, UIBool>();

        #region UIData对象
        //设置UIString对象
        public void SetUIString(string key, string value, bool isAdd)
        {
            if (stringDict.ContainsKey(key))
                stringDict[key].data = value;
            else
            {
                if (isAdd)
                    stringDict.Add(key, new UIString() { data = value });
                else
                    DebugMgr.Log("设置UIString失败，没有找到Key: " + key, SystemEnum.UISystem);
            }
        }
        //设置UIInt对象
        public void SetUIInt(string key, int value, bool isAdd)
        {
            if (intDict.ContainsKey(key))
                intDict[key].data = value;
            else
            {
                if (isAdd)
                    intDict.Add(key, new UIInt() { data = value });
                else
                    DebugMgr.Log("设置UIInt失败，没有找到Key: " + key, SystemEnum.UISystem);
            }
        }
        //设置UIFloat对象
        public void SetUIFloat(string key, float value, bool isAdd)
        {
            if (floatDict.ContainsKey(key))
                floatDict[key].data = value;
            else
            {
                if (isAdd)
                    floatDict.Add(key, new UIFloat() { data = value });
                else
                    DebugMgr.Log("设置UIFloat失败，没有找到Key: " + key, SystemEnum.UISystem);
            }
        }
        //设置UIBool对象
        public void SetUIBool(string key, bool value, bool isAdd)
        {
            if (boolDict.ContainsKey(key))
                boolDict[key].data = value;
            else
            {
                if (isAdd)
                    boolDict.Add(key, new UIBool() { data = value });
                else
                    DebugMgr.Log("设置UIBool失败，没有找到Key: " + key, SystemEnum.UISystem);
            }
        }

        //获取UIString对象
        public UIString GetUIString(string key)
        {
            if (stringDict.ContainsKey(key))
                return stringDict[key];
            else
            {
                DebugMgr.Log("没有在stringDict中找到指定Key: " + key, SystemEnum.UISystem);
                return null;
            }
        }
        //获取UIInt对象
        public UIInt GetUIInt(string key)
        {
            if (intDict.ContainsKey(key))
                return intDict[key];
            else
            {
                DebugMgr.Log("没有在intDict中找到指定Key: " + key, SystemEnum.UISystem);
                return null;
            }
        }
        //获取UIFloat对象
        public UIFloat GetUIFloat(string key)
        {
            if (floatDict.ContainsKey(key))
                return floatDict[key];
            else
            {
                DebugMgr.Log("没有在flaotDict中找到指定Key: " + key, SystemEnum.UISystem);
                return null;
            }
        }
        //获取UIBool对象
        public UIBool GetUIBool(string key)
        {
            if (boolDict.ContainsKey(key))
                return boolDict[key];
            else
            {
                DebugMgr.Log("没有在boolDict中找到指定Key: " + key, SystemEnum.UISystem);
                return null;
            }
        }

        #endregion
    }
}
