/****************************************************
  文件：UIABProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 16:56:47
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class UIABProfile : ABProfile
    {
        public UIABProfile(string abName) : base(abName)
        {

        }

        //也可以自己写一个无参构造函数，base里加上默认包名
    }
}
