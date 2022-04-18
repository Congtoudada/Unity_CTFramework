/****************************************************
  文件：ITimerMgr.cs
  作者：聪头
  邮箱: 1322080797@qq.com
  日期：2022/4/17 20:02:29
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.Events;

namespace CT
{
  public interface ITimerMgr
  {
    //定时回调函数(受Timescale的影响)
    //返回值：TaskID
    //参数①：延迟时间(单位:秒)
    //参数②：回调函数
    //参数③：触发次数（默认1次，-1代表无限循环）
    //参数④：触发间隔（默认-1，-1代表以延迟时间为间隔）
    int Wait(float delay, UnityAction callback, int count = 1, float interval = -1);
    
    //设置组名
    //参数①：Task ID
    //参数②：组名
    void SetGroupName(int ID, string groupName);
    
    //根据ID取消任务
    void Cancel(int id);
    
    //取消一组任务
    void CancelGroup(string groupName);
    
    //取消所有任务
    void CancelAll();
  }
}
