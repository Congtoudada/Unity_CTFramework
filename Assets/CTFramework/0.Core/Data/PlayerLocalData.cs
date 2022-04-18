/****************************************************
  文件：LocalData.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/12/3 14:28:06
  功能：玩家存档数据（格式为json，可以以关卡为单位加载数据）
            知识点：数据只存字典，不存PlayerData类名
            举例：玩家需要在本地硬盘存储的数据，例如金币、经验、已过关数等等
*****************************************************/
using CT.Tools;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CT
{
    //所有运行时存储的Key（不写入本地硬盘，以~开头）
    public enum PlayerRunningKey
    {
        
    }

    //所有本地存储的Key
    public enum PlayerLocalKey
    {

    }

    //本地存档类
    public class _PlayerData : IPlayerData
    {
        //玩家本地数据
        public Dictionary<string, string> keyValues { get; private set; }

        public _PlayerData()
        {
            keyValues = new Dictionary<string, string>();
        }

        //安全添加，有则覆盖，无则新建
        //参数③：开启后Key没有时自动添加
        public void SetValue(string key, string value, bool isAdd = true)
        {
            if (keyValues.ContainsKey(key))
                keyValues[key] = value;
            else
            {
                if (isAdd)
                    keyValues.Add(key, value);
            }
        }

        //根据Key获取Value
        public string GetValue(string key)
        {
            if (keyValues.TryGetValue(key, out string result))
            {
                return result;
            }
            return "";
        }

        //从硬盘更新数据
        //参数①：加载路径
        //参数②：是否安全更新。安全更新清空原先的数据，而是在此基础上进行数据的更新操作
        public void ReadFromDisk(string path, bool isSafe = false)
        {
            //从数据源生成KeyValues
            string content = FileTool.ReadTextFromDisk(path);
            if (isSafe)
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                //更新自身
                foreach (string key in data.Keys)
                {
                    SetValue(key, data[key]);
                }
            }
            else
            {
                keyValues.Clear();
                keyValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            }
        }

        //写入本地存档
        public void WriteToDisk(string path)
        {
            string content = JsonConvert.SerializeObject(keyValues.All(v=>!v.Value.StartsWith("_")));
            FileTool.WriteTextToDisk(path, content);
        }
    }
}