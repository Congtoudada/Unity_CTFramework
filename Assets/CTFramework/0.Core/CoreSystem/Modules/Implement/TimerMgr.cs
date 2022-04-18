/****************************************************
  文件：TimerMgr.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/20 17:13:10
  功能：定时回调管理类
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace CT
{
    public class TimerMgr : ITimerMgr
    {
        private int Global_Id = 0;

        //如果异步任务非常多的话，可以考虑根据destTime，采用小顶堆优化算法
        //原因：如果destTime值小的都没有执行，大的更不可能执行
        private List<TimerTask> timerList;

        private bool isRunning = true;

        //定时调用（场景有效）
        //①延迟执行的时间 ②执行的回调 ③执行次数(-1无限执行) ④每次执行间隔时间 ⑤任务名称（调试用）
        public int Wait(float delay, UnityAction callback, int count = 1, float interval = 0)
        {
            if (count == -1 && interval == 0)
                CTLogger.Warning("你注册了一个无限执行且没有间隔的回调，这是一个危险操作！除非您明确要这样做！");
            if (timerList == null)
            {
                //CT在，协程就在，一个生命周期就初始化一次
                if (_CT.Instance == null)
                {
                    CTLogger.Log("对不起，没有找到_CT脚本，无法使用定时回调系统", SysEnum.CoreSystem);
                    return -1;
                }
                timerList = new List<TimerTask>();
                isRunning = true;
                _CT.Instance.StartCoroutine(TimeUpdate());
            }
            TimerTask task = new TimerTask(
                GetId(),
                delay,
                callback,
                count,
                interval
            );

            //如果任务数量过多，则不再添加
            //if (timerList.Count > maxTask)
            //{
            //    DebugMgr.Log("任务数量过多，无法继续添加", SystemEnum.CoreSystem);
            //    return -1;
            //}
            timerList.Add(task);
            return task.timerId;
        }

        public void SetGroupName(int ID, string groupName)
        {
            foreach (var item in timerList)
            {
                if (item.timerId == ID)
                {
                    item.groupName = groupName;
                }
            }
        }

        public void CancelGroup(string groupName)
        {
            for (int i = 0; i < timerList.Count; i++)
            {
                if (timerList[i].groupName == groupName)
                {
                    timerList.RemoveAt(i--); //手动调整i值
                }
            }
        }
        
        //取消某个定时任务
        public void Cancel(int id)
        {
            if (id < 0) return;//不合法
            //删除任务
            timerList.Remove(timerList.Find(t => t.timerId == id));
        }

        //处于安全性考虑，场景切换时会自动调用CancelAll
        public void CancelAll()
        {
            timerList.Clear();
        }
        
        #region 私有函数
        
        private IEnumerator TimeUpdate()
        {
            while(isRunning)
            {
                //如果没有回调，直接返回节省性能
                if (timerList.Count <= 0) yield return null;
                float time = Time.time; //获取程序从开始到现在的运行时间（受暂停影响）
                //处理每一个任务项
                for (int i = 0; i < timerList.Count; i++)
                {
                    //如果当前时间 >= 目标时间
                    if (time >= timerList[i].destTime)
                    {
                        try
                        {
                            timerList[i].callback(); //触发回调函数  
                        }
                        catch (Exception ex)
                        {
                            CTLogger.Warning(ex.ToString());
                            timerList.RemoveAt(i--); //直接移除该异常的任务
                            continue;
                        }
                        if (timerList[i].count == -1) //代表无限循环执行
                        {
                            //重新计算目标时间
                            timerList[i].destTime = time + timerList[i].interval;
                            continue;
                        }
                        if (--timerList[i].count < 1) //调用后次数-1，如果小于1就结束该任务
                        {
                            timerList.RemoveAt(i--); //手动调整i值
                        }
                        else
                        {
                            //重新计算目标时间
                            if (timerList[i].interval < 0)
                            {
                                timerList[i].destTime = time + timerList[i].delay;
                            }
                            else
                            {
                                timerList[i].destTime = time + timerList[i].interval;
                            }
                        }
                    }
                }
                yield return null;
            }
        }
        
        private int GetId()
        {
            if (Global_Id == int.MaxValue)
                Global_Id = 0;
            return Global_Id++;
        }
        
        #endregion
    }

    public class TimerTask
    {
        public int timerId; //Id
        public UnityAction callback;//执行的回调
        public float delay;//延迟执行的时间 单位:s
        public float destTime;//执行回调时的时间 单位:s
        public int count;//回调执行次数,默认执行1次
        public float interval;//回调次数大于1，每次执行间隔
        public string groupName = "";

        public TimerTask(int timerId, float delay, UnityAction callback, int count, float interval)
        {
            this.delay = delay;
            destTime = Time.time + delay;
            this.callback = callback;
            if (count < -1) count = 1;
            this.count = count;
            if (interval <= 0) interval = delay;
            this.interval = interval;
        }
    }
}