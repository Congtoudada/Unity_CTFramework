/****************************************************
  文件：UIString.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 23:50:33
  功能：Equals和==区别：https://www.cnblogs.com/changbaishan/p/10579021.html
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    [Serializable]
    public class UIData<T>
    {
        [SerializeField]
        [HideInInspector]
        protected T _data = default(T); //绑定的数据

        [LabelText("描述信息")]
        public string description; //描述信息

        [ShowInInspector]
        [PropertyOrder(-1)]
        public T data
        {
            get => _data;
            set
            {
                //数据发生改变，就可能触发回调函数
                if (!CustomEquals(value))
                {
                    viewUpdate?.Invoke(value);
                    onDataChange?.Invoke(value); 
                    _data = value;
                }
            }
        }

        protected virtual bool CustomEquals(T value)
        {
            return _data.Equals(value);
        }

        //更新绑定视图的回调，前端响应（必须实现）
        [HideInInspector]
        public OnUpdateView<T> viewUpdate = new OnUpdateView<T>();

        //当数据改变时的回调事件，后端响应（按需实现）
        [HideInInspector]
        public OnDataChange<T> onDataChange = new OnDataChange<T>();
    }

    public class OnUpdateView<T> : UnityEvent<T>
    {

    }

    public class OnDataChange<T> : UnityEvent<T>
    {

    }
}
