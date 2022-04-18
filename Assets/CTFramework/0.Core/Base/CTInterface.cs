/****************************************************                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
 文件：CoreInterface.cs
 作者：聪头
 邮箱:  1322080797@qq.com
 日期：2021/11/10 22:50:54
 功能：核心接口类，存放CTFramework的核心接口
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    //资源释放接口
    public interface IRelease
    {
        void Release();
    }

    //资源释放工具类
    public class ReleaseTool
    {
        public static void TryRelease(object obj)
        {
            IRelease release = obj as IRelease;
            release?.Release();
        }
    }

    //本地存档
    public interface IPlayerData
    {
        //安全添加，有则覆盖，无则新建
        //参数③：开启后Key没有时自动添加
        void SetValue(string key, string value, bool isAdd = true);

        //根据Key获取Value
        string GetValue(string key);

        //从硬盘更新数据
        //参数①：加载路径
        //参数②：是否安全更新。安全更新(为true)将不清空原先的数据，而是在此基础上进行数据的更新操作
        void ReadFromDisk(string path, bool isSafe = false);

        //写入本地存档
        void WriteToDisk(string path);
    }
}