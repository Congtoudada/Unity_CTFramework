/****************************************************
  文件：TimeTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/3 12:08:51
  功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class TimeTool
    {
        //获得时间戳 单位: ms
        public static long GetTimeStamp(bool isMilliseconds = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            if(isMilliseconds)
                return Convert.ToInt64(ts.TotalMilliseconds);
            else
                return Convert.ToInt64(ts.TotalSeconds);
        }
        //根据时间戳获得日期字符串(1999-11-01 \n 18:00:00) 单位:ms
        public static string GetTimeString(long time, bool isOnlyDate = false)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(time).AddHours(8);
            //04/17/2021 00:58:10
            string[] strs1 = dateTime.ToString().Split(' ');
            string[] strs2 = strs1[0].Split('/');
            if (isOnlyDate)
                return string.Format($"{strs2[0]}-{strs2[1]}-{strs2[2]}");
            else
            {
                string[] strs3 = strs1[1].Split(':');
                return string.Format($"{strs2[0]}-{strs2[1]}-{strs2[2]}\n{strs3[0]}:{strs3[1]}:{strs3[2]}");
            }
        }
    }
}
