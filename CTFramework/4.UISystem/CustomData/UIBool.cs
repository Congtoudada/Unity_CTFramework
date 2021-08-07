/****************************************************
  文件：UIBool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 10:58:02
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    [System.Serializable]
    public class UIBool : UIData<bool>
    {
        protected override bool CustomEquals(bool value)
        {
            return _data == value;
        }
    }
}
