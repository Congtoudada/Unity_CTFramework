/****************************************************
  文件：CT.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:22:09
  功能：核心驱动类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public class _CT : SingletonMono<_CT>
    {
        ICTFactory factory;

        public static ResourceSys.ResMgr ResMgr;

        public static SceneSys.SceneMgr SceneMgr;

        public static UISys.UIMgr UIMgr;

        public static AudioSys.AudioMgr AuMgr;

        public static FileSys.FileMgr FileMgr;

        public static TimerSys.TimerMgr TimerMgr;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this);

            factory = new CTFactoryV1();

            ResMgr = factory.CreateResMgr();

            SceneMgr = factory.CreateSceneMgr();

            UIMgr = factory.CreateUIMgr();

            AuMgr = factory.CreateAuMgr();

            FileMgr = factory.CreateFileMgr();

            TimerMgr = factory.CreateTimerMgr();
        }
    }
}

