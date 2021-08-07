/****************************************************
  文件：CTModel.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 23:40:54
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using CT.Tools;
using CT.UISys;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.UISys
{
    public abstract class ModelPanel : BasePanel
    {
        //自定义UIProfile，自动配置ModelView
        public ModelPanel(LoadBaseProfile profile) : base(profile)
        { }

        protected override void Init(string panelName)
        {
            base.Init(panelName);

            //自动加载ModelView，也可由外部加载
            if (controller == null)
            {
                string path = Path.Combine(_CT.UIMgr.coreProfile.modelviewPath, panelName + "Profile");
                controller = new UIController(path);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            view = ui.GetComponent<ViewComponent>();
            if (view == null)
            {
                DebugMgr.Error($"没有在 {ui.name} 上找到ViewComponent组件", SystemEnum.UISystem);
                return;
            }
            view.controller = controller;

            //绑定数据
            BindingData();
            //绑定后端回调函数
            BindingCallback();
            //初始化前端视图
            view.InitView();
        }

        //绑定数据（可以在modelview上直接设置初始值，也可以在这里通过脚本设置）
        protected abstract void BindingData();

        //其他回调事件
        protected abstract void BindingCallback();
    }
}
