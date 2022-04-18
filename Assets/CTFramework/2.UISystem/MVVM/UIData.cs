/****************************************************
  文件：UIData.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/17 10:20:58
  功能：支持响应的数据类型，适用范围不仅限于UI系统
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CT.UISys
{
    //自定义响应数据类型
    public abstract class UIData<T>
    {
        protected T _data; //绑定的数据
        public bool isTrigger; //是否触发回调函数
        public bool isFirstInvoke; //是否在初始化时调用After监听（提供外界使用）
        //当数据改变前触发的回调函数
        public UnityAction<T,T> onChange_Before;
        //当数据改变后触发的回调函数
        public UnityAction<T> onChange_After;
        //条件回调函数
        public Func<T, T, bool> condition;

        public UIData(T value = default(T), bool isTrigger = true, bool isFirstInvoke = true)
        {
            _data = value; //绑定的数据
            this.isTrigger = isTrigger; //是否触发回调函数
            this.isFirstInvoke = isFirstInvoke; //是否在初始化时调用监听，用以渲染页面
        }

        //特性仅用于Debug
        [ShowInInspector]
        [PropertyOrder(-1)]
        public T data
        {
            get => _data;
            set
            {
                //数据发生改变，就可能触发回调函数
                if (InvokeCondition(value))
                {
                    #region try...catch
                    try //如果太耗费性能，有把握可以去掉try...catch
                    {
                        if (isTrigger)
                        {
                            onChange_Before?.Invoke(_data, value);
                            _data = value;
                            onChange_After?.Invoke(_data);
                        }
                        else
                        {
                            _data = value;
                        }
                    }
                    catch (NullReferenceException nullException)
                    {
                        CTLogger.Warning("使用UIData时出现空指针异常: " + nullException, SysEnum.UISystem);
                    }
                    catch (InvalidCastException nullException)
                    {
                        CTLogger.Warning("使用UIData时出现类型转换异常: " + nullException, SysEnum.UISystem);
                    }
                    catch (Exception e)
                    {
                        CTLogger.Warning("使用UIData时出现异常: " + e, SysEnum.UISystem);
                    }
                    #endregion
                }
            }
        }

        //返回true，触发事件；否则，不触发
        protected abstract bool InvokeCondition(T value);
    }

    //自定义UIBool
    public class UIBool : UIData<bool>
    {
        public UIBool(bool value = false, bool isTrigger = true, bool isFirstInvoke = true)
            : base(value,isTrigger,isFirstInvoke)
        {
            
        }

        protected override bool InvokeCondition(bool value)
        {
            //如果自定义比较函数存在，就调用自定义比较函数
            if (condition != null)
                return condition(_data, value);
            //默认实现，两者数据不相等，就触发事件
            return _data != value;
        }
    }

    //自定义UIInt
    public class UIInt : UIData<int>
    {
        public UIInt(int value = 0, bool isTrigger = true, bool isFirstInvoke = true)
            : base(value, isTrigger, isFirstInvoke)
        {

        }

        protected override bool InvokeCondition(int value)
        {
            //如果自定义比较函数存在，就调用自定义比较函数
            if (condition != null)
                return condition(_data, value);
            //默认实现，两者数据不相等，就触发事件
            return _data != value;
        }
    }

    //自定义UIFloat
    public class UIFloat : UIData<float>
    {
        public UIFloat(float value = 0, bool isTrigger = true, bool isFirstInvoke = true)
            : base(value, isTrigger, isFirstInvoke)
        {

        }

        protected override bool InvokeCondition(float value)
        {
            //如果自定义比较函数存在，就调用自定义比较函数
            if (condition != null)
                return condition(_data, value);
            //默认实现，两者数据不相等，就触发事件
            return _data != value;
        }
    }

    //自定义UIString
    public class UIString : UIData<string>
    {
        public UIString(string value = "", bool isTrigger = true, bool isFirstInvoke = true)
            : base(value, isTrigger, isFirstInvoke)
        {

        }

        protected override bool InvokeCondition(string value)
        {
            //如果自定义比较函数存在，就调用自定义比较函数
            if (condition != null)
                return condition(_data, value);
            //默认实现，两者数据不相等，就触发事件
            return _data != value;
        }
    }
}