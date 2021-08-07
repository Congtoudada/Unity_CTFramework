/****************************************************
  文件：AuResProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/2 17:04:21
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.AudioSys
{
    public class AuResProfile : ResProfile
    {
        public AuResProfile(string path) : base(path)
        {

        }

        public AuResProfile() : base(_CT.AuMgr.coreProfile.clipDirPath)
        {

        }
    }
}
