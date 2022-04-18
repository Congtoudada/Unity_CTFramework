/****************************************************
  文件：UIModelView.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：数据载体
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CT.UISys
{
    public class UIModelView
    {
        //数据字典
        private Dictionary<string, UIBool> _mvBool;
        [ShowInInspector]
        public Dictionary<string, UIBool> mvBool
        {
            get
            {
                if (_mvBool == null)
                    _mvBool = new Dictionary<string, UIBool>();
                return _mvBool;
            }
        }

        private Dictionary<string, UIInt> _mvInt;
        [ShowInInspector]
        public Dictionary<string, UIInt> mvInt
        {
            get
            {
                if (_mvInt == null)
                    _mvInt = new Dictionary<string, UIInt>();
                return _mvInt;
            }
        }

        private Dictionary<string, UIFloat> _mvFloat;
        [ShowInInspector]
        public Dictionary<string, UIFloat> mvFloat
        {
            get
            {
                if (_mvFloat == null)
                    _mvFloat = new Dictionary<string, UIFloat>();
                return _mvFloat;
            }
        }

        private Dictionary<string, UIString> _mvString;
        [ShowInInspector]
        public Dictionary<string, UIString> mvString
        {
            get
            {
                if (_mvString == null)
                    _mvString = new Dictionary<string, UIString>();
                return _mvString;
            }
        }

        public UIModelView(int boolCapacity = 0, int intCapacity = 0, int floatCapacity = 0, int stringCapacity = 0)
        {
            if (boolCapacity != 0)
            {
                _mvBool = new Dictionary<string, UIBool>(boolCapacity + 1);
            }
            if (intCapacity != 0)
            {
                _mvInt = new Dictionary<string, UIInt>(intCapacity + 1);
            }
            if (floatCapacity != 0)
            {
                _mvFloat = new Dictionary<string, UIFloat>(floatCapacity + 2);
            }
            if (stringCapacity != 0)
            {
                _mvString = new Dictionary<string, UIString>(stringCapacity + 3);
            }
        }

        public void Clear()
        {
            if (_mvBool != null) _mvBool.Clear();
            if (_mvInt != null) _mvInt.Clear();
            if (_mvFloat != null) _mvFloat.Clear();
            if (_mvString != null) _mvString.Clear();
        }
    }
}