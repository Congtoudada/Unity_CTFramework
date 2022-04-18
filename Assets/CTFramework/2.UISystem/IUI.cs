/****************************************************
  文件：IUI.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/15 20:22:48
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public interface IUI : IRelease, IUIController
    {
        #region 栈顶操作
        //UI入栈操作（UIProfile中默认配置为Resources）
        //参数①：加载目录或者AB包
        //参数②：加载PanelProfile名称（可以省略后缀）
        //参数③：传递给面板的参数，在OnEnter时访问
        void Push(string resDir, string panelName, object obj = null);

        //UI入栈操作（需要在UIProfile中配置默认加载路径）
        //参数①：加载PanelProfile名称
        //参数②：传递给面板的参数，在OnEnter时访问
        void Push(string panelName, object obj = null);

        //UI缓存操作（需要在UIProfile中配置默认加载路径）
        //参数①：加载PanelProfile名称（可以省略后缀）
        //默认将面板设为可缓存类型
        void AddToCachePool(string panelName);

        //从缓存（池）中移除
        //参数①：加载PanelProfile名称
        bool RemoveFromCachePool(string panelName);

        //出栈
        void Pop();

        //清空栈
        void AllPop();

        //获得栈顶UI面板
        BasePanel PeekPanel();

        //通过泛型获得栈顶UI面板的ViewComponent具体实现类
        T PeekViewComponent<T>() where T : ViewComponent;

        //获得栈顶UI面板的游戏对象
        GameObject PeekObj();

        //获得栈顶UI面板的Controller
        IUIController PeekController();

        //根据名称返回栈顶面板是否匹配
        bool isPeek(string panelName);
        #endregion

        #region 非栈顶操作
        //根据面板名称获得UI面板（包括缓存面板）
        BasePanel GetPanel(string panelName);

        //根据面板名称获得UI面板游戏对象（包括缓存面板）
        GameObject GetObj(string panelName);
        
        #endregion
    }
}