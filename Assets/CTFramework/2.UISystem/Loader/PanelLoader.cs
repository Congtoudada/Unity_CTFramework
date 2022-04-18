/****************************************************
  文件：PanelLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 10:08:15
  功能：面板加载器，外界确保数据合法性。不负责资源的加载与释放，仅控制面板的压入弹出和一些基本操作
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    public class PanelLoader : IUI
    {
        //当前所有UI面板的堆栈
        private Stack<BasePanel> stackPanel;

        public PanelLoader()
        {
            stackPanel = new Stack<BasePanel>();
        }

        #region 栈顶一般性操作
        //UI入栈操作
        private void Push(BasePanel nextPanel, object obj = null)
        {
            if (nextPanel == null)
            {
                CTLogger.Warning("面板不存在，Push失败", SysEnum.UISystem);
                return;
            }
            if (stackPanel.Count > 0)
            {
                stackPanel.Peek().OnPause(); //暂停栈顶面板
            }
            // 排到最前（UI管理组）
            if (PeekPanel() != null)
                nextPanel.go.transform.SetSiblingIndex(PeekObj().transform.GetSiblingIndex() + 1);
            stackPanel.Push(nextPanel); //压入新面板
            nextPanel.OnEnter(obj); //面板初始化
        }

        public void Push(string resDir, string panelName, object obj = null)
        {
            Push(UIFactory.CreateBasePanel(resDir, panelName), obj);
        }

        public void Push(string panelName, object obj = null)
        {
            Push(UIFactory.CreateBasePanel(panelName), obj);
        }

        //UI缓存操作（需要在UIProfile中配置默认加载路径）
        public void AddToCachePool(string panelName)
        {
            Push(panelName);
            Pop();
        }

        //从缓存（池）中移除
        public bool RemoveFromCachePool(string panelName)
        {
            BasePanel panel = GetPanel(panelName);
            //只有不在stackPanel中时，才可以调用
            if (panel != null)
            {
                //将其从缓存中移除
                UIFactory.ClearCache(panelName);
                return true;
            }
            else
            {
                CTLogger.Log("无法将: " + panelName + " 从缓存池中移除，该面板正在使用..." + SysEnum.UISystem);
                return false;
            }
        }

        //出栈
        public void Pop()
        {
            if (stackPanel.Count > 0)
            {
                stackPanel.Peek().OnExit(); //面板退出
                stackPanel.Pop();
            }
            if (stackPanel.Count > 0)
            {
                stackPanel.Peek().OnResume(); //恢复栈顶面板
            }
        }

        //清空栈
        public void AllPop()
        {
            while (stackPanel.Count > 0)
            {
                Pop();
            }
        }

        //获得栈顶UI面板
        public BasePanel PeekPanel()
        {
            if (stackPanel.Count == 0) return null;
            return stackPanel.Peek();
        }

        //通过泛型获得栈顶UI面板的ViewComponent
        public T PeekViewComponent<T>() where T : ViewComponent
        {
            return PeekObj().GetComponent<T>();
        }

        //获得栈顶UI面板的游戏对象
        public GameObject PeekObj()
        {
            if (stackPanel.Count == 0) return null;
            return stackPanel.Peek().go;
        }

        //获得栈顶UI面板的Controller
        public IUIController PeekController()
        {
            return PeekPanel()?.controller;
        }

        //根据名称返回栈顶面板是否匹配
        public bool isPeek(string panelName)
        {
            if (stackPanel.Count > 0 && PeekPanel().panelName.Equals(panelName))
                return true;
            else return false;
        }
        #endregion

        #region 非栈顶操作
        //根据面板名称获得UI面板（包括缓存面板）
        public BasePanel GetPanel(string panelName)
        {
            foreach(BasePanel item in stackPanel)
            {
                if (item.panelName.Equals(panelName))
                    return item;
            }
            return null;
        }

        //根据面板名称获得UI面板游戏对象（包括缓存面板）
        public GameObject GetObj(string panelName)
        {
            foreach (BasePanel item in stackPanel)
            {
                if (item.panelName.Equals(panelName))
                    return item.go;
            }
            return null;
        }

        public void Release()
        {
            //因为涉及到缓存，这里stack的释放交给外层控制
        }
        #endregion

        #region 栈顶UIController操作

        public UIModelView GetModelView()
        {
            return PeekPanel()?.controller.GetModelView();
        }

        public UIBool SetBool(string key, bool value, bool isTrigger = true, bool isFirstRender = true)
        {
            return PeekPanel()?.controller.SetBool(key, value, isTrigger, isFirstRender);
        }

        public UIInt SetInt(string key, int value, bool isTrigger = true, bool isFirstRender = true)
        {
            return PeekPanel()?.controller.SetInt(key, value, isTrigger, isFirstRender);
        }

        public UIFloat SetFloat(string key, float value, bool isTrigger = true, bool isFirstRender = true)
        {
            return PeekPanel()?.controller.SetFloat(key, value, isTrigger, isFirstRender);
        }

        public UIString SetString(string key, string value, bool isTrigger = true, bool isFirstRender = true)
        {
            return PeekPanel()?.controller.SetString(key, value, isTrigger, isFirstRender);
        }

        public UIBool GetUIBool(string key)
        {
            return PeekPanel()?.controller.GetUIBool(key);
        }

        public UIInt GetUIInt(string key)
        {
            return PeekPanel()?.controller.GetUIInt(key);
        }

        public UIFloat GetUIFloat(string key)
        {
            return PeekPanel()?.controller.GetUIFloat(key);
        }

        public UIString GetUIString(string key)
        {
            return PeekPanel()?.controller.GetUIString(key);
        }

        public bool GetBoolValue(string key)
        {
            if (PeekPanel() != null)
                return PeekPanel().controller.GetBoolValue(key);
            else return false;
        }

        public int GetIntValue(string key)
        {
            if (PeekPanel() != null)
                return PeekPanel().controller.GetIntValue(key);
            else return 0;
        }

        public float GetFloatValue(string key)
        {
            if (PeekPanel() != null)
                return PeekPanel().controller.GetFloatValue(key);
            else return 0;
        }

        public string GetStringValue(string key)
        {
            if (PeekPanel() != null)
                return PeekPanel().controller.GetStringValue(key);
            else return "";
        }

        public void RemoveUIBool(string key)
        {
            PeekPanel()?.controller.RemoveUIBool(key);
        }

        public void RemoveUIInt(string key)
        {
            PeekPanel()?.controller.RemoveUIInt(key);
        }

        public void RemoveUIFloat(string key)
        {
            PeekPanel()?.controller.RemoveUIFloat(key);
        }

        public void RemoveUIString(string key)
        {
            PeekPanel()?.controller.RemoveUIString(key);
        }

        public void ModifyBoolCondition(string key, Func<bool, bool, bool> condition)
        {
            PeekPanel()?.controller.ModifyBoolCondition(key, condition);
        }

        public void ModifyIntCondition(string key, Func<int, int, bool> condition)
        {
            PeekPanel()?.controller.ModifyIntCondition(key, condition);
        }

        public void ModifyFloatCondition(string key, Func<float, float, bool> condition)
        {
            PeekPanel()?.controller.ModifyFloatCondition(key, condition);
        }

        public void ModifyStringCondition(string key, Func<string, string, bool> condition)
        {
            PeekPanel()?.controller.ModifyStringCondition(key, condition);
        }

        public UIBool AddBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            return PeekPanel()?.controller.AddBoolBeforeListener(key, callback);
        }

        public UIInt AddIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            return PeekPanel()?.controller.AddIntBeforeListener(key, callback);
        }

        public UIFloat AddFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            return PeekPanel()?.controller.AddFloatBeforeListener(key, callback);
        }

        public UIString AddStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            return PeekPanel()?.controller.AddStringBeforeListener(key, callback);
        }

        public UIBool AddBoolAfterListener(string key, UnityAction<bool> callback)
        {
            return PeekPanel()?.controller.AddBoolAfterListener(key, callback);
        }

        public UIInt AddIntAfterListener(string key, UnityAction<int> callback)
        {
            return PeekPanel()?.controller.AddIntAfterListener(key, callback);
        }

        public UIFloat AddFloatAfterListener(string key, UnityAction<float> callback)
        {
            return PeekPanel()?.controller.AddFloatAfterListener(key, callback);
        }

        public UIString AddStringAfterListener(string key, UnityAction<string> callback)
        {
            return PeekPanel()?.controller.AddStringAfterListener(key, callback);
        }

        public void RemoveBoolBeforeListener(string key, UnityAction<bool, bool> callback)
        {
            PeekPanel()?.controller.RemoveBoolBeforeListener(key, callback);
        }

        public void RemoveIntBeforeListener(string key, UnityAction<int, int> callback)
        {
            PeekPanel()?.controller.RemoveIntBeforeListener(key, callback);
        }

        public void RemoveFloatBeforeListener(string key, UnityAction<float, float> callback)
        {
            PeekPanel()?.controller.RemoveFloatBeforeListener(key, callback);
        }

        public void RemoveStringBeforeListener(string key, UnityAction<string, string> callback)
        {
            PeekPanel()?.controller.RemoveStringBeforeListener(key, callback);
        }

        public void RemoveBoolAfterListener(string key, UnityAction<bool> callback)
        {
            PeekPanel()?.controller.RemoveBoolAfterListener(key, callback);
        }

        public void RemoveIntAfterListener(string key, UnityAction<int> callback)
        {
            PeekPanel()?.controller.RemoveIntAfterListener(key, callback);
        }

        public void RemoveFloatAfterListener(string key, UnityAction<float> callback)
        {
            PeekPanel()?.controller.RemoveFloatAfterListener(key, callback);
        }

        public void RemoveStringAfterListener(string key, UnityAction<string> callback)
        {
            PeekPanel()?.controller.RemoveStringAfterListener(key, callback);
        }

        public void RemoveAllListener()
        {
            PeekPanel()?.controller.RemoveAllListener();
        }

        public void InvokeAll()
        {
            PeekPanel()?.controller.InvokeAll();
        }

        #endregion
    }
}