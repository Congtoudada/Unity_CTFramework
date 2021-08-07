/****************************************************
  文件：ICTFactory.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 10:36:54
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public interface ICTFactory
    {
        ResourceSys.ResMgr CreateResMgr();

        SceneSys.SceneMgr CreateSceneMgr();

        UISys.UIMgr CreateUIMgr();

        AudioSys.AudioMgr CreateAuMgr();

        FileSys.FileMgr CreateFileMgr();

        TimerSys.TimerMgr CreateTimerMgr();
    }
}
