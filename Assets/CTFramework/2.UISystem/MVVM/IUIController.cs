/****************************************************
  文件：IController.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/17 16:26:54
  功能：
*****************************************************/
using CT.UISys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT
{
    public interface IUIController
    {
        //得到ModelView
        UIModelView GetModelView();

        //设置Key-Value值，有则覆盖，无则添加
        //参数①：键
        //参数②：值
        //参数③：是否触发回调事件
        //参数④：是否在第一帧调用After监听，渲染数据
        UIBool SetBool(string key, bool value, bool isTrigger = true, bool isFirstRender = true);
        UIInt SetInt(string key, int value, bool isTrigger = true, bool isFirstRender = true);
        UIFloat SetFloat(string key, float value, bool isTrigger = true, bool isFirstRender = true);
        UIString SetString(string key, string value, bool isTrigger = true, bool isFirstRender = true);

        //根据Key，得到UI对象，访问data可以取出值
        UIBool GetUIBool(string key);
        UIInt GetUIInt(string key);
        UIFloat GetUIFloat(string key);
        UIString GetUIString(string key);

        //根据Key，得到值
        bool GetBoolValue(string key);
        int GetIntValue(string key);
        float GetFloatValue(string key);
        string GetStringValue(string key);
        
        //根据Key，删除Property
        void RemoveUIBool(string key);
        void RemoveUIInt(string key);
        void RemoveUIFloat(string key);
        void RemoveUIString(string key);

        //修改条件函数，每当根据Key赋值时会调用，返回true则更新值，false则不更新
        //参数①：键
        //参数②：二元谓词
        void ModifyBoolCondition(string key, System.Func<bool, bool, bool> condition);
        void ModifyIntCondition(string key, System.Func<int, int, bool> condition);
        void ModifyFloatCondition(string key, System.Func<float, float, bool> condition);
        void ModifyStringCondition(string key, System.Func<string, string, bool> condition);

        //添加before监听，数据改变前触发
        //参数①：键
        //参数②：回调函数，第一个参数代表旧值；第二个参数代表新值
        UIBool AddBoolBeforeListener(string key, UnityAction<bool, bool> callback);
        UIInt AddIntBeforeListener(string key, UnityAction<int, int> callback);
        UIFloat AddFloatBeforeListener(string key, UnityAction<float, float> callback);
        UIString AddStringBeforeListener(string key, UnityAction<string, string> callback);

        //添加after监听，数据改变后触发
        //参数①：键
        //参数②：回调函数，参数代表新值
        UIBool AddBoolAfterListener(string key, UnityAction<bool> callback);
        UIInt AddIntAfterListener(string key, UnityAction<int> callback);
        UIFloat AddFloatAfterListener(string key, UnityAction<float> callback);
        UIString AddStringAfterListener(string key, UnityAction<string> callback);

        //删除before监听
        void RemoveBoolBeforeListener(string key, UnityAction<bool, bool> callback);
        void RemoveIntBeforeListener(string key, UnityAction<int, int> callback);
        void RemoveFloatBeforeListener(string key, UnityAction<float, float> callback);
        void RemoveStringBeforeListener(string key, UnityAction<string, string> callback);

        //删除after监听
        void RemoveBoolAfterListener(string key, UnityAction<bool> callback);
        void RemoveIntAfterListener(string key, UnityAction<int> callback);
        void RemoveFloatAfterListener(string key, UnityAction<float> callback);
        void RemoveStringAfterListener(string key, UnityAction<string> callback);

        //删除所有监听
        void RemoveAllListener();

        //强制触发所有监听，一般用于初始化双向绑定后的数据渲染
        void InvokeAll();
    }
}