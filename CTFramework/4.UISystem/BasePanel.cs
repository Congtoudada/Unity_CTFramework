/****************************************************
  文件：BasePanel.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 16:32:26
  功能：UI面板基类
*****************************************************/
using CT.ResourceSys;
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public abstract class BasePanel : IRelease
    {
        public LoadBaseProfile profile; //配置文件
        public UIController controller;//控制器，控制ModelView

        public GameObject ui;  //ui对象
        protected ViewComponent view;//视图

        public BasePanel(LoadBaseProfile profile) {
            this.profile = profile;
        }

        protected virtual void Init(string panelName)
        {
            //初始化profile
            profile.filename = panelName;
        }

        //进入面板时候执行的操作，仅一次
        public virtual void OnEnter()
        {

        }
        //UI暂停时进行的操作
        public virtual void OnPause()
        {
            SwitchUI(false);
        }
        //UI继续时进行的操作
        public virtual void OnResume()
        {
            SwitchUI(true);
        }
        //UI退出时进行的操作，仅一次
        public virtual void OnExit()
        {

        }
        //禁用
        public void SwitchUI(bool isOn)
        {
            UniTool.FindComponent<CanvasGroup>(ui, profile.filename, true).blocksRaycasts = isOn;
        }

        public virtual void Release()
        {
            profile = null;
            ReleaseTool.TryRelease(controller);
            ReleaseTool.TryRelease(view);
            GameObject.Destroy(ui);
        }
    }
}
