/****************************************************
    文件：Singleton.cs
    日期：2021/3/19 17:28:59
	功能：单例模式基类
*****************************************************/
using System.Collections;
using System.Collections.Generic;

namespace CT
{
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
        protected Singleton() { }
    }

}
