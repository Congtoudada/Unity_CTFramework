/****************************************************
  文件：Singleton.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/10 22:47:13
  功能：单例模式基类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    //全局单例（懒加载）
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected Singleton()
        {
            InitSelf();
        }
        
        protected virtual void InitSelf(){ }
    }
    //依赖场景的单例，继承MonoBehaviour，需要提前挂载（懒加载）
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get => _instance;
        }

        //子类可以重写父类Awake，一定要先调用base.Awake()
        protected virtual void Awake()
        {
            _instance = this as T;
        }

        //子类可以重写父类OnDestroy，一定要最后调用base.OnDestroy()
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
