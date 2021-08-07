/****************************************************
  文件：UIResProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 16:56:32
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class UIResProfile : ResProfile
    {
        public UIResProfile(string dirPath) : base(dirPath)
        {

        }
        public UIResProfile() : this(_CT.UIMgr.coreProfile.prefabPath)
        {

        }
    }
}
