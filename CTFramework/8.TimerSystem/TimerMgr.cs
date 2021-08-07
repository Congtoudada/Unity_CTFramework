/****************************************************
  文件：TimerManager.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/4 18:50:47
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CT.TimerSys
{
    public class TimerMgr : IRelease
    {
        private int timerId = 0;

        private bool isRunning = true;

        private List<TimerTask> timerList = new List<TimerTask>();

        public TimerMgr()
        {
            _CT.Instance.StartCoroutine(TimeUpdate());
        }

        private IEnumerator TimeUpdate()
        {
            while(isRunning)
            {
                if (timerList.Count <= 0) yield return null;
                float time = Time.time;
                //处理每一个任务项
                for (int i = 0; i < timerList.Count; i++)
                {
                    if (time >= timerList[i].destTime)
                    {
                        timerList[i].callback();
                        Debug.Log("Timer触发回调: " + timerList[i].taskName);
                        if (timerList[i].count <= 1)
                        {
                            timerList.RemoveAt(i--);
                        }
                        else
                        {
                            timerList[i].count--;
                            timerList[i].destTime = time + timerList[i].interval;
                        }
                    }
                }
                yield return null;
            }
        }

        //定时调用
        //①延迟执行的时间 ②执行的回调 ③执行次数 ④每次执行间隔时间
        public void Wait(string taskName, float delay, UnityAction callback, int count = 1, float interval = 0)
        {
            if (timerList == null) return;
            TimerTask task = new TimerTask(
                taskName,
                GetTimerId(),
                delay,
                callback,
                count,
                interval
            );
            //Debug.Log("Wait: " + task.timerId);
            timerList.Add(task);
        }
        //得到任务id
        private int GetTimerId()
        {
            if (timerId >= int.MaxValue) timerId = 0;
            return timerId++;
        }

        //轻量释放（释放资源后不需要重新new）
        //适用于场景切换时，需要清空所有TimeTask
        public void Release()
        {
            isRunning = false;
            timerList.Clear();
            timerId = 0;
        }
    }
}
