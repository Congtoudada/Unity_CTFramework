/****************************************************
  文件：CTHelper.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2022/4/17 20:17:06
  功能：框架辅助模块（重要性介于工具类和核心模块之间，保持通用性的同时强调实用性）
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CT
{
  public class CTHelper : Singleton<CTHelper>
  {
    //玩家数据
    public static IPlayerData data { get; }
    
    public static ISceneMgr sceneMgr { get; }

    public static ITimerMgr timerMgr { get; }
    

    static CTHelper()
    {
      CTConfig config = CTFactory.config;
      if (config != null)
      {
        if (config.isPlayerData) data = new _PlayerData();
        if (config.isSceneMgr) sceneMgr = new SceneMgr();
        if (config.isTimerMgr) timerMgr = new TimerMgr();
      }
    }
  }
}
