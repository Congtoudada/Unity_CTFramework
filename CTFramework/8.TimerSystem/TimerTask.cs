/****************************************************
  文件：TimerTask.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/4 18:51:03
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.TimerSys
{
    public class TimerTask
    {
        public string taskName;//任务名称，方便调试
        public int timerId;//timerId
        public UnityAction callback;//执行的回调
        public float delay;//延迟执行的时间 单位:s
        public float destTime;//执行回调时的时间 单位:s
        public int count;//回调执行次数,默认执行1次
        public float interval;//回调次数大于1，每次执行间隔

        public TimerTask(string taskName, int timerId, float delay, UnityAction callback, int count, float interval)
        {
            this.taskName = taskName;
            this.timerId = timerId;
            this.delay = delay;
            destTime = Time.time + delay;
            this.callback = callback;
            if (count <= 0) count = 1;
            this.count = count;
            if (interval <= 0) interval = delay;
            this.interval = interval;
        }
    }
}