/****************************************************
  文件：UIInt.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 10:57:05
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [System.Serializable]
    public class UIInt : UIData<int>
    {
        int threshold = 0; //阈值，当只改变大于阈值时才触发回调

        public UIInt(int threshold = 0)
        {
            this.threshold = threshold;
        }

        protected override bool CustomEquals(int value)
        {
            return Mathf.Abs(_data - value) < threshold;
        }
    }
}
