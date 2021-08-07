/****************************************************
  文件：PanelLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 18:27:19
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class PanelLoader : IRelease
    {
        //当前所有UI面板的堆栈
        private Stack<BasePanel> stackPanel;
        //UI加载器
        private UILoader uiLoader;

        public PanelLoader()
        {
            stackPanel = new Stack<BasePanel>();
            uiLoader = new UILoader();
        }

        //UI入栈操作，此操作会显示一个面板
        public void Push(BasePanel nextPanel)
        {
            GameObject panelGo = uiLoader.CreateUI(nextPanel.profile);
            if(panelGo != null)
            {
                nextPanel.ui = panelGo;
                if (stackPanel.Count > 0)
                {
                    stackPanel.Peek().OnPause(); //暂停顶部面板
                }
                stackPanel.Push(nextPanel);
                nextPanel.OnEnter();
            }
            else
            {
                DebugMgr.Warning("入栈失败，资源没有找到", SystemEnum.UISystem);
            }
        }
        
        //出栈，并选择是否继续上一面板
        public void Pop()
        {
            if (stackPanel.Count > 0)
            {
                stackPanel.Peek().OnExit();//面板退出
                uiLoader.DestroyUI(stackPanel.Peek().profile);//面板游戏对象并移除字典（不摧毁）
                ReleaseTool.TryRelease(stackPanel.Peek()); //释放面板资源
                stackPanel.Pop();
            }
            if (stackPanel.Count > 0)
            {
                stackPanel.Peek().OnResume();
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
        
        //获得栈顶元素
        public BasePanel Peek()
        {
            if (stackPanel.Count > 0)
                return stackPanel.Peek();
            else
            {
                DebugMgr.Log("堆栈为空，无法获得栈顶元素", SystemEnum.UISystem);
                return null;
            }
        }
        
        //通过泛型获得栈顶元素
        public T Peek<T>() where T : BasePanel
        {
            if (stackPanel.Count > 0)
            {
                if (stackPanel.Peek() as T != null)
                    return stackPanel.Peek() as T;
                else
                {
                    DebugMgr.Log("无法获得栈顶元素，类型不正确!", SystemEnum.UISystem);
                    return null;
                }
            }
            else
            {
                DebugMgr.Log("无法获得栈顶元素，堆栈为空!", SystemEnum.UISystem);
                return null;
            }
        }
        
        //返回栈顶面板对应的游戏对象
        public GameObject PeekObj()
        {
            return Peek()?.ui;
        }
        
        //根据名称返回栈顶面板是否匹配
        public bool isPeek(string name)
        {
            if (stackPanel.Count > 0)
                return Peek().profile.filename.Equals(name);
            return false;
        }
        
        //根据名称得到已加载的面板游戏对象
        public GameObject GetPanelObject(string panelName)
        {
            return uiLoader.GetUIObject(panelName);
        }

        public void Release()
        {
            AllPop();
            ReleaseTool.TryRelease(uiLoader);
            uiLoader = null;
            stackPanel.Clear();
            stackPanel = null;
        }
    }
}
