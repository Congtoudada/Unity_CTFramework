/****************************************************
  文件：UIMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 10:49:40
  功能：
*****************************************************/
using CT.UISys;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT
{
    public class UIMgr : IUI
    {
        //面板加载器
        private IUI loader;

        public UIMgr() { }

        //动态注入加载器
        public void SetLoader(IUI loader)
        {
            if (this.loader != null)
            {
                Release();
            }
            this.loader = loader;
        }

        #region 栈顶一般性操作
        //入栈
        public void Push(string resDir, string panelName, object obj = null)
        {
            loader.Push(resDir, panelName, obj);
        }

        public void Push(string panelName, object obj = null)
        {
            loader.Push(panelName, obj);
        }

        //UI缓存操作（需要在UIProfile中配置默认加载路径）
        public void AddToCachePool(string panelName)
        {
            loader.AddToCachePool(panelName);
        }

        //从缓存（池）中移除
        public bool RemoveFromCachePool(string panelName)
        {
            return loader.RemoveFromCachePool(panelName);
        }

        //出栈
        public void Pop()
        {
            BasePanel bp = loader.PeekPanel();
            loader.Pop(); //弹出当前堆栈
            UIFactory.CacheAndDestroyUl(bp); //判断是否进入缓存池
        }

        //清空栈
        public void AllPop()
        {
            loader.AllPop(); //弹出所有面板
            UIFactory.ClearAllCache(); //释放所有剩下的缓存
        }

        //获得栈顶UI面板
        public BasePanel PeekPanel()
        {
            return loader.PeekPanel();
        }

        //通过泛型获得栈顶UI面板的ViewComponent
        public T PeekViewComponent<T>() where T : ViewComponent
        {
            return loader.PeekViewComponent<T>();
        }

        //获得栈顶UI面板的游戏对象（包括缓存面板）
        public GameObject PeekObj()
        {
            return loader.PeekObj();
        }

        //获得栈顶UI面板的Controller（包括缓存面板）
        public IUIController PeekController()
        {
            return loader.PeekController();
        }

        //根据名称返回栈顶面板是否匹配
        public bool isPeek(string panelName)
        {
            return loader.isPeek(panelName);
        }
        #endregion

        #region 非栈顶操作
        //根据面板名称获得UI面板
        public BasePanel GetPanel(string panelName)
        {
            return loader.GetPanel(panelName);
        }

        //根据面板名称获得UI面板游戏对象
        public GameObject GetObj(string panelName)
        {
            return loader.GetObj(panelName);
        }

        //释放所有资源，勿乱调用，可能会寄
        public void Release()
        {
            AllPop();
            loader = null;
        }
        #endregion

        #region 栈顶UIController操作
        public UIModelView GetModelView()
        {
            return loader.GetModelView();
        }

        public UIBool SetBool(string key, bool value, bool isTrigger = true, bool isFirstRender = true)
        {
            return loader.SetBool(key, value, isTrigger, isFirstRender);
        }

        public UIInt SetInt(string key, int value, bool isTrigger = true, bool isFirstRender = true)
        {
            return loader.SetInt(key, value, isTrigger, isFirstRender);
        }

        public UIFloat SetFloat(string key, float value, bool isTrigger = true, bool isFirstRender = true)
        {
            return loader.SetFloat(key, value, isTrigger, isFirstRender);
        }

        public UIString SetString(string key, string value, bool isTrigger = true, bool isFirstRender = true)
        {
            return loader.SetString(key, value, isTrigger, isFirstRender);
        }

        public UIBool GetUIBool(string key)
        {
            return loader.GetUIBool(key);
        }

        public UIInt GetUIInt(string key)
        {
            return loader.GetUIInt(key);
        }

        public UIFloat GetUIFloat(string key)
        {
            return loader.GetUIFloat(key);
        }

        public UIString GetUIString(string key)
        {
            return loader.GetUIString(key);
        }

        public bool GetBoolValue(string key)
        {
            return loader.GetBoolValue(key);
        }

        public int GetIntValue(string key)
        {
            return loader.GetIntValue(key);
        }

        public float GetFloatValue(string key)
        {
            return loader.GetFloatValue(key);
        }

        public string GetStringValue(string key)
        {
            return loader.GetStringValue(key);
        }

        public void RemoveUIBool(string key)
        {
            loader.RemoveUIBool(key);
        }

        public void RemoveUIInt(string key)
        {
            loader.RemoveUIInt(key);
        }

        public void RemoveUIFloat(string key)
        {
            loader.RemoveUIFloat(key);
        }

        public void RemoveUIString(string key)
        {
            loader.RemoveUIString(key);
        }

        public void ModifyBoolCondition(string key, Func<bool, bool, bool> condition)
        {
            loader.ModifyBoolCondition(key, condition);
        }

        public void ModifyIntCondition(string key, Func<int, int, bool> condition)
        {
            loader.ModifyIntCondition(key, condition);
        }

        public void ModifyFloatCondition(string key, Func<float, float, bool> condition)
        {
            loader.ModifyFloatCondition(key, condition);
        }

        public void ModifyStringCondition(string key, Func<string, string, bool> condition)
        {
            loader.ModifyStringCondition(key, condition);
        }

        public UIBool AddBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            return loader.AddBoolBeforeListener(key, callback);
        }

        public UIInt AddIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            return loader.AddIntBeforeListener(key, callback);
        }

        public UIFloat AddFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            return loader.AddFloatBeforeListener(key, callback);
        }

        public UIString AddStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            return loader.AddStringBeforeListener(key, callback);
        }

        public UIBool AddBoolAfterListener(string key, UnityAction<bool> callback)
        {
            return loader.AddBoolAfterListener(key, callback);
        }

        public UIInt AddIntAfterListener(string key, UnityAction<int> callback)
        {
            return loader.AddIntAfterListener(key, callback);
        }

        public UIFloat AddFloatAfterListener(string key, UnityAction<float> callback)
        {
            return loader.AddFloatAfterListener(key, callback);
        }

        public UIString AddStringAfterListener(string key, UnityAction<string> callback)
        {
            return loader.AddStringAfterListener(key, callback);
        }

        public void RemoveBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            loader.RemoveBoolBeforeListener(key, callback);
        }

        public void RemoveIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            loader.RemoveIntBeforeListener(key, callback);
        }

        public void RemoveFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            loader.RemoveFloatBeforeListener(key, callback);
        }

        public void RemoveStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            loader.RemoveStringBeforeListener(key, callback);
        }

        public void RemoveBoolAfterListener(string key, UnityAction<bool> callback)
        {
            loader.RemoveBoolAfterListener(key, callback);
        }

        public void RemoveIntAfterListener(string key, UnityAction<int> callback)
        {
            loader.RemoveIntAfterListener(key, callback);
        }

        public void RemoveFloatAfterListener(string key, UnityAction<float> callback)
        {
            loader.RemoveFloatAfterListener(key, callback);
        }

        public void RemoveStringAfterListener(string key, UnityAction<string> callback)
        {
            loader.RemoveStringAfterListener(key, callback);
        }

        public void RemoveAllListener()
        {
            loader.RemoveAllListener();
        }

        public void InvokeAll()
        {
            loader.InvokeAll();
        }
        #endregion

    }
}