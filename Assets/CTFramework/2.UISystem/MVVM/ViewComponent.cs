/****************************************************
  文件：ViewComponent.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 19:36:20
  功能：通用的View层
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UISys
{
    public abstract class ViewComponent : MonoBehaviour, IRelease
    {
        //UI控制器
        public UIController controller;
        [FoldoutGroup("DEBUG")]
        [Title("运行时ModelView")]
        [ShowInInspector]
        public UIModelView modelView;

        //动态注入Controller
        public void SetController(UIController controller)
        {
            this.controller = controller;
            modelView = controller.GetModelView();
        }

        #region 生命周期函数 (Awake->OnInit->OnEnter(可能多次)->Start->OnExit(可能多次)->OnRelease)
        //OnInit，整个生命周期仅一次
        //查找UI组件
        public virtual void OnInit() { }
        

        //面板加载时调用，数据初始化和渲染（一帧内完成...）
        //最后需要强制触发一次所有modelView的监听，渲染数据
        //controller.InvokeAll(); //触发所有回调，完成数据渲染
        public virtual void OnEnter(object obj) { }

        //面板暂停时调用
        public virtual void OnPause() { }

        //面板恢复时调用
        public virtual void OnResume() { }

        //面板退出时调用
        public virtual void OnExit() { }
        
        #endregion

        public void Release()
        {
            controller = null;
        }
    }
}