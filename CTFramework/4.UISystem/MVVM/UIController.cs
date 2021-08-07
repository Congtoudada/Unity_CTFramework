/****************************************************
  文件：UIController.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 10:29:51
  功能：Nothing
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    public class UIController : IRelease
    {
        private UIModelView modelview;

        public UIController(string path)
        {
            modelview = Resources.Load<UIModelView>(path);
            if (modelview == null)
                DebugMgr.Error("没有找到ModelView配置文件，请检查：" + path, SystemEnum.UISystem);
        }

        #region data值操作
        //设置属性值
        public void SetProperty<T>(string key, T value, bool isAdd = false)
        {
            if(value is string)
            {
                modelview.SetUIString(key, value as string, isAdd);
            }
            else if(value is float)
            {
                float? param = value as float?;
                if(param.HasValue)
                {
                    modelview.SetUIFloat(key,param.Value, isAdd);
                }
                else
                    DebugMgr.Log("value值不合法，请确保为float", SystemEnum.UISystem);
            }
            else if(value is int)
            {
                int? param = value as int?;
                if (param.HasValue)
                {
                    modelview.SetUIInt(key, param.Value, isAdd);
                }
                else
                    DebugMgr.Log("value值不合法，请确保为int", SystemEnum.UISystem);
            }
            else if(value is bool)
            {
                bool? param = value as bool?;
                if (param.HasValue)
                {
                    modelview.SetUIBool(key, param.Value, isAdd);
                }
                else
                    DebugMgr.Log("value值不合法，请确保为bool", SystemEnum.UISystem);
            }
            else
            {
                DebugMgr.Log("无法设置属性，不支持该类型", SystemEnum.UISystem);
            }
        }

        public void SetString(string key, string value, bool isAdd = false)
        {
            modelview.SetUIString(key, value, isAdd);
        }

        public void SetFloat(string key, float value, bool isAdd = false)
        {
            modelview.SetUIFloat(key, value, isAdd);
        }

        public void SetInt(string key, int value, bool isAdd = false)
        {
            modelview.SetUIInt(key, value, isAdd);
        }

        public void SetBool(string key, bool value, bool isAdd = false)
        {
            modelview.SetUIBool(key, value, isAdd);
        }

        //获取属性值
        public string GetString(string key)
        {
            return modelview.GetUIString(key).data;
        }

        public float GetFloat(string key)
        {
            return modelview.GetUIFloat(key).data;
        }

        public int GetInt(string key)
        {
            return modelview.GetUIInt(key).data;
        }

        public bool GetBool(string key)
        {
            return modelview.GetUIBool(key).data;
        }

        #endregion

        #region 回调操作
        //参数①：key 参数②：值变换时的回调 参数③：是否是后端回调
        public void AddStringListener(string key, UnityAction<string> callback, bool isBackground = false)
        {
            if(modelview.GetUIString(key) != null)
            {
                if(isBackground)
                {
                    //后端回调
                    modelview.GetUIString(key).onDataChange.AddListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIString(key).viewUpdate.AddListener(callback);
                }
            }
        }

        public void AddFloatListener(string key, UnityAction<float> callback, bool isBackground = false)
        {
            if (modelview.GetUIFloat(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIFloat(key).onDataChange.AddListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIFloat(key).viewUpdate.AddListener(callback);
                }
            }
        }

        public void AddIntListener(string key, UnityAction<int> callback, bool isBackground = false)
        {
            if (modelview.GetUIInt(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIInt(key).onDataChange.AddListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIInt(key).viewUpdate.AddListener(callback);
                }
            }
        }

        public void AddBoolListener(string key, UnityAction<bool> callback, bool isBackground = false)
        {
            if (modelview.GetUIBool(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIBool(key).onDataChange.AddListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIBool(key).viewUpdate.AddListener(callback);
                }
            }
        }

        //解绑视图更新回调
        public void RemoveStringListener(string key, UnityAction<string> callback, bool isBackground = false)
        {
            if (modelview.GetUIString(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIString(key).onDataChange.RemoveListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIString(key).viewUpdate.RemoveListener(callback);
                }
            }
        }

        public void RemoveFloatListener(string key, UnityAction<float> callback, bool isBackground = false)
        {
            if (modelview.GetUIFloat(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIFloat(key).onDataChange.RemoveListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIFloat(key).viewUpdate.RemoveListener(callback);
                }
            }
        }

        public void RemoveIntListener(string key, UnityAction<int> callback, bool isBackground = false)
        {
            if (modelview.GetUIInt(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIInt(key).onDataChange.RemoveListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIInt(key).viewUpdate.RemoveListener(callback);
                }
            }
        }

        public void RemoveBoolListener(string key, UnityAction<bool> callback, bool isBackground = false)
        {
            if (modelview.GetUIBool(key) != null)
            {
                if (isBackground)
                {
                    //后端回调
                    modelview.GetUIBool(key).onDataChange.RemoveListener(callback);
                }
                else
                {
                    //前端回调
                    modelview.GetUIBool(key).viewUpdate.RemoveListener(callback);
                }
            }
        }

        //解绑全部回调，一般用于释放资源
        public void RemoveAllListener()
        {
            foreach(UIString item in modelview.stringDict.Values)
            {
                item.viewUpdate.RemoveAllListeners();
                item.onDataChange.RemoveAllListeners();
            }
            foreach(UIFloat item in modelview.floatDict.Values)
            {
                item.viewUpdate.RemoveAllListeners();
                item.onDataChange.RemoveAllListeners();
            }
            foreach (UIInt item in modelview.intDict.Values)
            {
                item.viewUpdate.RemoveAllListeners();
                item.onDataChange.RemoveAllListeners();
            }
            foreach (UIBool item in modelview.boolDict.Values)
            {
                item.viewUpdate.RemoveAllListeners();
                item.onDataChange.RemoveAllListeners();
            }
        }

        //强制触发所有回调，一般用于初始化
        public void InvokeAll()
        {
            foreach (UIString item in modelview.stringDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
                item.onDataChange?.Invoke(item.data);
            }
            foreach (UIFloat item in modelview.floatDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
                item.onDataChange?.Invoke(item.data);
            }
            foreach (UIInt item in modelview.intDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
                item.onDataChange?.Invoke(item.data);
            }
            foreach (UIBool item in modelview.boolDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
                item.onDataChange?.Invoke(item.data);
            }
        }

        //强制触发视图更新，一般用于渲染整个前端页面
        public void InvokeViewUpdate(string key)
        {
            foreach (UIString item in modelview.stringDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
            }
            foreach (UIFloat item in modelview.floatDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
            }
            foreach (UIInt item in modelview.intDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
            }
            foreach (UIBool item in modelview.boolDict.Values)
            {
                item.viewUpdate?.Invoke(item.data);
            }
        }

        #endregion

        public void Release()
        {
            RemoveAllListener();
            modelview.stringDict.Clear();
            modelview.floatDict.Clear();
            modelview.intDict.Clear();
            modelview.boolDict.Clear();
            modelview = null;
        }
    }
}
