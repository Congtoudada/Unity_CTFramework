/****************************************************
  文件：UIString.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 10:59:28
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [System.Serializable]
    public class UIString : UIData<string>
    {
        protected override bool CustomEquals(string value)
        {
            if (_data == null) return false;
            return _data.Equals(value);
        }
    }
}
