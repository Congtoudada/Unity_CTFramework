/****************************************************
  文件：CTFactory.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using CT.AuSys;
using CT.ResSys;
using CT.UISys;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT
{
    public static class CTFactory
    {
        private static CTConfig _config; // 全局配置文件（整个游戏运行时有效）

        public static CTConfig config => _config;

        private static Dictionary<ProfileEnum, ScriptableObject> profileCache; //全局缓存列表
        
        static CTFactory()
        {
            _config = Resources.Load<CTConfig>(CTConstant.DEFAULT_CONFIG_PATH);
            if (_config == null) CTLogger.Error("找不到全局配置文件，请检查: " + CTConstant.DEFAULT_CONFIG_PATH);
            //重定向
            CTConfig temp = null;
            while (!string.IsNullOrEmpty(_config.OverridePath))
            {
                temp = Resources.Load<CTConfig>(_config.OverridePath);
                if (temp != null)
                {
                    _config = temp;
                }
                else
                {
                    CTLogger.Log("全局配置文件重定向失败，请检查路径: " + _config.OverridePath);
                    return;
                }
            }
            
            //配置文件加载成功，配置参数
            CTLogger.isDebug = _config.isDebug; //是否打印框架DEBUG输出
            
            //配置核心模块
            profileCache = new Dictionary<ProfileEnum, ScriptableObject>(_config.list.Count);
            foreach (var item in _config.list)
            {
                if (item.isCacheProfile)
                {
                    ScriptableObject profile =
                        Resources.Load<ScriptableObject>(Path.Combine(_config.ResourceLoadDir, item.moduleName));
                    if (profile != null)
                    {
                        profileCache.Add(item.type, profile);
                    }
                    else
                    {
                        Debug.LogWarning($"没有找到配置文件，请检查路径: {Path.Combine(_config.ResourceLoadDir, item.moduleName)}");
                    }
                }
            }
        }

        //得到最终版本的Profile
        public static ScriptableObject GetFinalProfile(ProfileEnum profileEnum)
        {
            ScriptableObject result = null;
            //先从缓存中取，没有再尝试加载
            if (profileCache.ContainsKey(profileEnum))
            {
                result = profileCache[profileEnum];
            }
            else
            {
                foreach (var item in _config.list)
                {
                    if (item.type == profileEnum)
                    {
                        result = Resources.Load<ScriptableObject>(Path.Combine(_config.ResourceLoadDir, item.moduleName));
                    }
                }
            }
            if (result == null)
                CTLogger.Warning($"获取 {profileEnum.ToString()} 模块配置文件失败，请仔细检查全局配置文件");
            return result;
        }

        //得到资源管理类
        public static ResMgr GetResMgr()
        {
            return ResFactory.Build(GetFinalProfile(ProfileEnum.Res));
        }

        //得到UI管理类
        public static UIMgr GetUIMgr()
        {
            return UIFactory.Build(GetFinalProfile(ProfileEnum.UI));
        }

        //得到Au管理类
        public static AuMgr GetAuMgr()
        {
            return AuFactory.Build(GetFinalProfile(ProfileEnum.Au));
        }
    }
}