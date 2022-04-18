/****************************************************
  文件：BasePanel.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/15 21:01:46
  功能：UI面板基类，游戏对象和UI数据的集合
*****************************************************/
using CT.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class BasePanel : IRelease
    {
        //ui面板游戏对象
        public GameObject go { get; private set; }
        //配置文件
        //private PanelProfile profile;
        //视图控制器，负责操控ModelView（数据通信）
        public UIController controller { get; private set; }
        //面板的ViewComponent（数据渲染）
        public ViewComponent view { get; private set; }

        /// 基本属性
        //是否缓存
        public bool isCache { get; private set; }
        //面板名称
        public string panelName { get; private set; }
        //是否在OnEnter时渲染
        private bool isPreLoad;
        //面板配置文件（必要时缓存，仅用于数据初始化）
        private PanelProfile profile;
        //是否是第一次初始化
        private bool isFirstInit = true;

        public BasePanel(PanelProfile profile)
        {
            //生成游戏对象（目前没有使用异步预加载，以后可以继续优化）
            go = UIFactory.CreateUIGo(profile);
            go.name = profile.panelName;
            //初始化数据
            isCache = profile.isCache;
            panelName = profile.panelName;
            isPreLoad = profile.isPreLoad;
            if (profile.isCacheProfile && (profile.isCache || profile.isPreLoad)) //缓存配置文件
                this.profile = profile;
            //初始化控制器
            controller = new UIController(go, profile);
            //初始化视图
            view = GetOrAddView(go);
            view.SetController(controller); //将controller注入到view中
            OnInit();
        }

        #region UI生命周期函数
        //面板初始化操作，创建UI面板对象时执行，整个生命周期只执行一次
        //调用该方法时，面板游戏对象已经创建
        public virtual void OnInit()
        {
            controller.InitData_OnEnter(profile); //先填充Key-Values
            view.OnInit();
        }
        //进入面板时候执行的操作，Push时执行（因为缓存的缘故，可能执行不止一次）
        public virtual void OnEnter(object obj)
        {
            if (isPreLoad)
            {
                if (go.activeSelf)
                    go.SetActive(false);
                //isCache = true;  //必开启isCache选项（留给外部判断了，不放心可以开）
            }
            else
            {
                //通常用于初始化面板数据
                if (!go.activeSelf)
                    go.SetActive(true);
                //初始化数据
                controller.InitData_OnEnter(profile);
                view.OnEnter(obj);
            }
        }
        //UI暂停时进行的操作
        public virtual void OnPause()
        {
            view.OnPause();
            SwitchUI(false);
        }
        //UI继续时进行的操作
        public virtual void OnResume()
        {
            SwitchUI(true);
            view.OnResume();
        }
        //UI退出时进行的操作，Pop时执行（因为缓存的缘故，可能执行不止一次）
        public virtual void OnExit()
        {
            if (isPreLoad)
            {
                isPreLoad = false;
            }
            else
            {
                view.OnExit();
                //隐藏GameObject，通常用于缓存
                if (go != null && go.activeSelf)
                    go.SetActive(false);
            }
        }
        //禁用
        public virtual void SwitchUI(bool isOn)
        {
            if (go != null)
                UniTool.FindComponent<CanvasGroup>(go, go.name, true).blocksRaycasts = isOn;
        }
        //释放整个UI面板
        public virtual void Release()
        {
            ReleaseTool.TryRelease(controller);  //销毁控制器
            ReleaseTool.TryRelease(view);
            GameObject.Destroy(go); //销毁游戏对象
            profile = null; //销毁配置文件
        }
        #endregion

        //当用户忘记或没有挂载View脚本时，自动推测并挂载
        private ViewComponent GetOrAddView(GameObject go)
        {
            ViewComponent result = go.GetComponent<ViewComponent>();
            if (result == null)
            {
                Type type = GetType().Assembly.GetType(go.name.Replace("Panel", "View"));
                if (type == null) //类型没有找到，猜测命名空间
                {
                    //string lastNamespace = "";
                    //foreach (var item in GetType().Assembly.GetTypes())
                    //{
                    //    //两次命名空间不同的话才推断类型
                    //    if (!item.Namespace.Equals(lastNamespace))
                    //        type = GetType().Assembly.GetType(item.Namespace + "." + profile.panelName.Replace("Panel", "View"));
                    //    if (type != null) break;
                    //    lastNamespace = item.Namespace;
                    //}
                    //如果猜不出来，非常不应该！报错！
                    CTLogger.Warning("没有找到该类型: " + type.FullName);
                }
                else if (type.IsSubclassOf(typeof(ViewComponent)))  //如果该类型是ViewComponent的实现类型
                {
                    //返回，供外部动态添加
                    result = go.AddComponent(type) as ViewComponent;
                }
            }
            return result;
        }
    }
}