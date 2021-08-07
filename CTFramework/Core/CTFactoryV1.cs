/****************************************************
  文件：CTFactory.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:33:21
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using CT.AudioSys;
using CT.FileSys;
using CT.ResourceSys;
using CT.SceneSys;
using CT.TimerSys;
using CT.UISys;
using UnityEngine;

namespace CT
{
    public class CTFactoryV1 : ICTFactory
    {
        public ResMgr CreateResMgr()
        {
            ResMgr mgr = new ResMgr();
            return mgr;
        }

        public SceneMgr CreateSceneMgr()
        {
            SceneMgr mgr = new SceneMgr();
            return mgr;
        }

        public AudioMgr CreateAuMgr()
        {
            AudioMgr mgr = new AudioMgr();
            return mgr;
        }

        public UIMgr CreateUIMgr()
        {
            UIMgr mgr = new UIMgr();
            return mgr;
        }

        //注意：使用时需要手动装配对应加载器
        public FileMgr CreateFileMgr()
        {
            FileMgr mgr = new FileMgr();
            return mgr;
        }

        public TimerMgr CreateTimerMgr()
        {
            TimerMgr mgr = new TimerMgr();
            return mgr;
        }
    }
}
