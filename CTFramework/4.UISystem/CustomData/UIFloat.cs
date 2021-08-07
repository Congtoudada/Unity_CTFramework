/****************************************************
  文件：UIFloat.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 10:56:03
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [System.Serializable]
    public class UIFloat : UIData<float>
    {
        float threshold = 0; //阈值，当只改变大于阈值时才触发回调

        public UIFloat(float threshold = 0)
        {
            this.threshold = threshold;
        }

        protected override bool CustomEquals(float value)
        {
            return Mathf.Abs(_data - value) < threshold;
        }
    }
}
