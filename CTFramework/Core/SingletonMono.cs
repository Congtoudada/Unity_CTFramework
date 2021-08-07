/****************************************************
    文件：SingletonMono.cs
    日期：2021/3/19 17:24:06
	功能：MonoBehaviour单例的基类，需要自己保证唯一性，
            不能拖多个，且挂载对象切场景不删除
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get => _instance;
        }
        //子类可以重写父类，并调用base.Awake()
        protected virtual void Awake()
        {
            _instance = this as T;

        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}

