/****************************************************
  文件：UIMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 16:32:17
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class UIMgr : IRelease
    {
        //面板加载器
        private PanelLoader panelLoader;
        //UI核心配置文件
        public UICoreProfile coreProfile;

        public UIMgr()
        {
            panelLoader = new PanelLoader();
            coreProfile = Resources.Load<UICoreProfile>(CTConstant.UI_CORE_PROFILE_PATH);
            if (coreProfile == null)
                DebugMgr.Error("没有找到UI核心配置文件，请检查路径: " + CTConstant.UI_CORE_PROFILE_PATH, SystemEnum.UISystem);
        }

        #region 提供给用户的方法
        //UI入栈操作，此操作会显示一个面板
        public void Push(BasePanel nextPanel)
        {
            panelLoader.Push(nextPanel);
        }

        //出栈，并选择是否继续上一面板
        public void Pop()
        {
            panelLoader.Pop();
        }

        //清空栈
        public void AllPop()
        {
            panelLoader.AllPop();
        }

        //获得栈顶元素
        public BasePanel Peek()
        {
            return panelLoader.Peek();
        }

        //通过泛型获得栈顶元素
        public T Peek<T>() where T : BasePanel
        {
            return panelLoader.Peek<T>();
        }

        //返回栈顶面板对应的游戏对象
        public GameObject PeekObj()
        {
            return panelLoader.PeekObj();
        }

        //返回栈顶对应的Controller
        public UIController PeekController()
        {
            return panelLoader.Peek().controller;
        }

        //根据名称返回栈顶面板是否匹配
        public bool isPeek(string name)
        {
            return panelLoader.isPeek(name);
        }

        //根据名称得到已加载的面板游戏对象
        public GameObject GetPanelObject(string panelName)
        {
            return panelLoader.GetPanelObject(panelName);
        }
        #endregion

        //释放资源
        public void Release()
        {
            ReleaseTool.TryRelease(panelLoader);
            panelLoader = null;
        }
    }
}
