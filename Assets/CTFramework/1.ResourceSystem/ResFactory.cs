/****************************************************
  文件：ResFactory.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 17:19:19
  功能：
*****************************************************/
using CT.ResSys;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT
{
    public static class ResFactory
    {
        //生产ResMgr
        public static ResMgr Build(ScriptableObject profile)
        {
            ResMgr mgr = null;
            if (profile != null)
            {
                mgr = new ResMgr();
                ResourceProfile resProfile = profile as ResourceProfile;
                //读取配置文件配置，是否注入ABLoader
                if (resProfile != null && resProfile.isABLoad)
                {
                    ABLoader abLoader = new ABLoader(resProfile);
                    mgr.SetLoader(abLoader);
                }
            }
            return mgr;
        }
    }
}