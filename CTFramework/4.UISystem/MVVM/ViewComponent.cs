/****************************************************
  文件：CTView.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 23:41:03
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UISys
{
    public abstract class ViewComponent : MonoBehaviour, IRelease
    {
        [HideInInspector]
        public UIController controller;

        public void InitView()
        {
            //渲染视图 （等一帧渲染，确保UI组件都在Start内绑定成功）
            StartCoroutine(deferRenderEnumerator());
        }

        IEnumerator deferRenderEnumerator()
        {
            yield return null;
            //绑定视图
            BindingViewUpdate();
            //渲染
            Renderer();
        }

        //绑定前端回调函数，即绑定视图
        public abstract void BindingViewUpdate();

        //根据模型渲染数据
        public void Renderer()
        {
            controller.InvokeAll(); //前后端回调一起触发
            //controller.InvokeViewUpdate(); //只触发前端回调
        }

        public virtual void Release()
        {
            controller = null;
        }
    }
}
