/****************************************************
  文件：UIController.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 10:35:18
  功能：controller层，view和model层的桥梁
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    public class UIController : IRelease, IUIController
    {
        //前后端交互载体：负责管理运行时的ModelView
        //public Dictionary<string, UIData> modelView { get; private set; }
        private UIModelView modelView;

        public UIController(GameObject go, PanelProfile profile)
        {
            int sumBool = 0;
            int sumInt = 0;
            int sumFloat = 0;
            int sumString = 0;
            if (profile != null && profile.keyValues != null)
            {
                foreach (var item in profile.keyValues)
                {
                    switch (item.type)
                    {
                        case CustomUIType.String:
                            ++sumString;
                            break;
                        case CustomUIType.Float:
                            ++sumFloat;
                            break;
                        case CustomUIType.Bool:
                            ++sumBool;
                            break;
                        case CustomUIType.Int:
                            ++sumInt;
                            break;
                    }
                }
            }
            modelView = new UIModelView(sumBool, sumInt, sumFloat, sumString);
        }

        #region 接口实现
        //得到ModelView
        public UIModelView GetModelView()
        {
            return modelView;
        }

        //用于OnEnter时初始化数据（第一次在OnInit时调用）
        public void InitData_OnEnter(PanelProfile profile)
        {
            //如果缓存了配置文件，就执行数据初始化
            if (profile != null)
            {
                //注意避免初始化时触发回调函数
                foreach (var item in profile.keyValues)
                {
                    switch (item.type)
                    {
                        case CustomUIType.String:
                            SetString(item.key, item.value, false, item.isFirstInvoke).isTrigger = true;
                            break;
                        case CustomUIType.Float:
                            SetFloat(item.key, Convert.ToSingle(item.value), false, item.isFirstInvoke).isTrigger = true;
                            break;
                        case CustomUIType.Bool:
                            SetBool(item.key, Convert.ToBoolean(item.value), false, item.isFirstInvoke).isTrigger = true;
                            break;
                        case CustomUIType.Int:
                            SetInt(item.key, Convert.ToInt32(item.value), false, item.isFirstInvoke).isTrigger = true;
                            break;
                    }
                }
            }
        }


        //设置Key-Value
        public UIBool SetBool(string key, bool value, bool isTrigger = true, bool isFirstRender = true)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].isTrigger = isTrigger;
                modelView.mvBool[key].isFirstInvoke = isFirstRender;
                modelView.mvBool[key].data = value;
            }
            else
            {
                modelView.mvBool.Add(key, new UIBool(value, isTrigger, isFirstRender));
            }
            return modelView.mvBool[key];
        }
        public UIInt SetInt(string key, int value, bool isTrigger = true, bool isFirstRender = true)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].isTrigger = isTrigger;
                modelView.mvInt[key].isFirstInvoke = isFirstRender;
                modelView.mvInt[key].data = value;
            }
            else
            {
                modelView.mvInt.Add(key, new UIInt(value, isTrigger, isFirstRender));
            }

            return modelView.mvInt[key];
        }
        public UIFloat SetFloat(string key, float value, bool isTrigger = true, bool isFirstRender = true)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].isTrigger = isTrigger;
                modelView.mvFloat[key].isFirstInvoke = isFirstRender;
                modelView.mvFloat[key].data = value;
            }
            else
            {
                modelView.mvFloat.Add(key, new UIFloat(value, isTrigger, isFirstRender));
            }
            return modelView.mvFloat[key];
        }
        public UIString SetString(string key, string value, bool isTrigger = true, bool isFirstRender = true)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].isTrigger = isTrigger;
                modelView.mvString[key].isFirstInvoke = isFirstRender;
                modelView.mvString[key].data = value;
            }
            else
            {
                modelView.mvString.Add(key, new UIString(value, isTrigger, isFirstRender));
            }
            return modelView.mvString[key];
        }

        //获取UIData对象
        public UIBool GetUIBool(string key)
        {
            if (modelView.mvBool.ContainsKey(key))
                return modelView.mvBool[key];
            CTLogger.Log("获取UIBool值失败，没有找到Key: " + key, SysEnum.UISystem);
            return null;
        }
        public UIInt GetUIInt(string key)
        {
            if (modelView.mvInt.ContainsKey(key))
                return modelView.mvInt[key];
            CTLogger.Log("获取UIInt值失败，没有找到Key: " + key, SysEnum.UISystem);
            return null;
        }
        public UIFloat GetUIFloat(string key)
        {
            if (modelView.mvFloat.ContainsKey(key))
                return modelView.mvFloat[key];
            CTLogger.Log("获取UIFloat值失败，没有找到Key: " + key, SysEnum.UISystem);
            return null;
        }
        public UIString GetUIString(string key)
        {
            if (modelView.mvString.ContainsKey(key))
                return modelView.mvString[key];
            CTLogger.Log("获取UIString值失败，没有找到Key: " + key, SysEnum.UISystem);
            return null;
        }

        //根据Key获取Value值
        public bool GetBoolValue(string key)
        {
            if (modelView.mvBool.ContainsKey(key))
                return modelView.mvBool[key].data;
            CTLogger.Log("获取bool值失败，没有找到Key: " + key, SysEnum.UISystem);
            return false;
        }
        public int GetIntValue(string key)
        {
            if (modelView.mvInt.ContainsKey(key))
                return modelView.mvInt[key].data;
            CTLogger.Log("获取int值失败，没有找到Key: " + key, SysEnum.UISystem);
            return 0;
        }
        public float GetFloatValue(string key)
        {
            if (modelView.mvFloat.ContainsKey(key))
                return modelView.mvFloat[key].data;
            CTLogger.Log("获取float值失败，没有找到Key: " + key, SysEnum.UISystem);
            return 0;
        }
        public string GetStringValue(string key)
        {
            if (modelView.mvString.ContainsKey(key))
                return modelView.mvString[key].data;
            CTLogger.Log("获取string值失败，没有找到Key: " + key, SysEnum.UISystem);
            return "";
        }

        public void RemoveUIBool(string key)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].onChange_Before = null;
                modelView.mvBool[key].onChange_After = null;
                modelView.mvBool.Remove(key);
            }
        }

        public void RemoveUIInt(string key)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].onChange_Before = null;
                modelView.mvInt[key].onChange_After = null;
                modelView.mvInt.Remove(key);
            }
        }

        public void RemoveUIFloat(string key)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].onChange_Before = null;
                modelView.mvFloat[key].onChange_After = null;
                modelView.mvFloat.Remove(key);
            }
        }

        public void RemoveUIString(string key)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].onChange_Before = null;
                modelView.mvString[key].onChange_After = null;
                modelView.mvString.Remove(key);
            }
        }

        //修改比较函数
        public void ModifyBoolCondition(string key, Func<bool, bool, bool> condition)
        {
            UIBool customData = GetUIBool(key);
            if (customData != null)
            {
                customData.condition = condition;
                CTLogger.Log("修改UIBool的比较函数成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("修改UIBool的比较函数失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void ModifyIntCondition(string key, Func<int, int, bool> condition)
        {
            UIInt customData = GetUIInt(key);
            if (customData != null)
            {
                customData.condition = condition;
                CTLogger.Log("修改UIInt的比较函数成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("修改UIInt的比较函数失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void ModifyFloatCondition(string key, Func<float, float, bool> condition)
        {
            UIFloat customData = GetUIFloat(key);
            if (customData != null)
            {
                customData.condition = condition;
                CTLogger.Log("修改UIFloat的比较函数成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("修改UIFloat的比较函数失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void ModifyStringCondition(string key, Func<string, string, bool> condition)
        {
            UIString customData = GetUIString(key);
            if (customData != null)
            {
                customData.condition = condition;
                CTLogger.Log("修改UIString的比较函数成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("修改UIString的比较函数失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }

        //添加值修改前的监听
        public UIBool AddBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].onChange_Before += callback;
                CTLogger.Log("为UIBool新增Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIBool新增Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetBool(key, false);
                AddBoolBeforeListener(key, callback);
            }
            return modelView.mvBool[key];
        }
        public UIInt AddIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].onChange_Before += callback;
                CTLogger.Log("为UIInt新增Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIInt新增Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetInt(key, 0);
                AddIntBeforeListener(key, callback);
            }
            return modelView.mvInt[key];
        }
        public UIFloat AddFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].onChange_Before += callback;
                CTLogger.Log("为UIFloat增Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIFloat新增Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetFloat(key, 0);
                AddFloatBeforeListener(key, callback);
            }
            return modelView.mvFloat[key];
        }
        public UIString AddStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].onChange_Before += callback;
                CTLogger.Log("为UIString新增Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIString新增Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetString(key, "");
                AddStringBeforeListener(key, callback);
            }
            return modelView.mvString[key];
        }

        //添加值修改后监听
        public UIBool AddBoolAfterListener(string key, UnityAction<bool> callback)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].onChange_After += callback;
                CTLogger.Log("为UIBool新增After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIBool新增After监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetBool(key, false);
                AddBoolAfterListener(key, callback);
            }
            return modelView.mvBool[key];
        }
        public UIInt AddIntAfterListener(string key, UnityAction<int> callback)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].onChange_After += callback;
                CTLogger.Log("为UIInt新增After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIInt新增After监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetInt(key, 0);
                AddIntAfterListener(key, callback);
            }
            return modelView.mvInt[key];
        }
        public UIFloat AddFloatAfterListener(string key, UnityAction<float> callback)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].onChange_After += callback;
                CTLogger.Log("为UIFloat新增After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIFloat新增After监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetFloat(key, 0);
                AddFloatAfterListener(key, callback);
            }
            return modelView.mvFloat[key];
        }
        public UIString AddStringAfterListener(string key, UnityAction<string> callback)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].onChange_After += callback;
                CTLogger.Log("为UIString新增After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                // CTLogger.Log("为UIString新增After监听失败，没有找到Key: " + key, SysEnum.UISystem);
                SetString(key, "");
                AddStringAfterListener(key, callback);
            }
            return modelView.mvString[key];
        }

        //移除Before监听
        public void RemoveBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].onChange_Before -= callback;
                CTLogger.Log("为UIBool删除Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIBool删除Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void RemoveIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].onChange_Before -= callback;
                CTLogger.Log("为UIInt删除Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIInt删除Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void RemoveFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].onChange_Before -= callback;
                CTLogger.Log("为UIFloat删除Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIFloat删除Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }
        public void RemoveStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].onChange_Before -= callback;
                CTLogger.Log("为UIString删除Before监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIString删除Before监听失败，没有找到Key: " + key, SysEnum.UISystem);
            }
        }

        //移除After监听
        public void RemoveBoolAfterListener(string key, UnityAction<bool> callback)
        {
            if (modelView.mvBool.ContainsKey(key))
            {
                modelView.mvBool[key].onChange_After -= callback;
                CTLogger.Log("为UIBool删除After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIBool删除After监听失败，没有找到Key", SysEnum.UISystem);
            }
        }
        public void RemoveIntAfterListener(string key, UnityAction<int> callback)
        {
            if (modelView.mvInt.ContainsKey(key))
            {
                modelView.mvInt[key].onChange_After -= callback;
                CTLogger.Log("为UIInt删除After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIInt删除After监听失败，没有找到Key", SysEnum.UISystem);
            }
        }
        public void RemoveFloatAfterListener(string key, UnityAction<float> callback)
        {
            if (modelView.mvFloat.ContainsKey(key))
            {
                modelView.mvFloat[key].onChange_After -= callback;
                CTLogger.Log("为UIFloat删除After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIFloat删除After监听失败，没有找到Key", SysEnum.UISystem);
            }
        }
        public void RemoveStringAfterListener(string key, UnityAction<string> callback)
        {
            if (modelView.mvString.ContainsKey(key))
            {
                modelView.mvString[key].onChange_After -= callback;
                CTLogger.Log("为UIString删除After监听成功: " + key, SysEnum.UISystem);
            }
            else
            {
                CTLogger.Log("为UIString删除After监听失败，没有找到Key", SysEnum.UISystem);
            }
        }

        //移除所有监听
        public void RemoveAllListener()
        {
            if (modelView.mvBool != null)
            {
                foreach (var item in modelView.mvBool.Values)
                {
                    item.onChange_Before = null;
                    item.onChange_After = null;
                }
            }

            if (modelView.mvInt != null)
            {
                foreach (var item in modelView.mvInt.Values)
                {
                    item.onChange_Before = null;
                    item.onChange_After = null;
                }
            }

            if (modelView.mvFloat != null)
            {
                foreach (var item in modelView.mvFloat.Values)
                {
                    item.onChange_Before = null;
                    item.onChange_After = null;
                }
            }

            if (modelView.mvString != null)
            {
                foreach (var item in modelView.mvString.Values)
                {
                    item.onChange_Before = null;
                    item.onChange_After = null;
                }
            }
        }

        //强制调用所有监听
        public void InvokeAll()
        {
            foreach (var item in modelView.mvBool.Values)
            {
                if (item.isFirstInvoke)
                    item.onChange_After?.Invoke(item.data);
            }
            foreach (var item in modelView.mvInt.Values)
            {
                if (item.isFirstInvoke)
                    item.onChange_After?.Invoke(item.data);
            }
            foreach (var item in modelView.mvFloat.Values)
            {
                if (item.isFirstInvoke)
                    item.onChange_After?.Invoke(item.data);
            }
            foreach (var item in modelView.mvString.Values)
            {
                if (item.isFirstInvoke)
                    item.onChange_After?.Invoke(item.data);
            }
        }

        //面板释放
        public void Release()
        {
            RemoveAllListener();
            modelView.Clear();
            modelView = null;
        }
        #endregion
    }
}