/****************************************************
  文件：AuFactory.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/24 23:11:49
  功能：
*****************************************************/
using CT.AuSys;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.AuSys
{
    public static class AuFactory
    {
        private static AuProfile auProfile;
        //生产AuMgr
        public static AuMgr Build(ScriptableObject profile)
        {
            AuMgr mgr = null;
            if (profile != null)
            {
                mgr = new AuMgr();
                //准备两个音源
                AudioSource source1 = _CT.Instance.gameObject.AddComponent<AudioSource>();
                GameObject obj = new GameObject("extraSource");
                obj.transform.parent = _CT.Instance.transform;
                AudioSource source2 = obj.AddComponent<AudioSource>();
                //缓存配置文件
                AuFactory.auProfile = profile as AuProfile;
                AuLoader loader = new AuLoader(auProfile, source1, source2);
                mgr.SetLoader(loader);
            }
            return mgr;
        }

        //生产AuGroupProfile
        //参数①：加载目录或者AB包
        //参数②：加载AuGroupProfile名称
        private static AuGroupProfile CreateAuGroupProfile(string resDir, string groupName)
        {
            AuGroupProfile profile = null;
            if (auProfile.isABLoad) //AB包加载
            {
                profile = _CT.ResMgr.LoadRes<AuGroupProfile>(resDir, groupName);
                if (profile == null)
                    CTLogger.Log("没有找到AuGroupProfile，请AB包是否存在：" + resDir, SysEnum.AudioSystem);
            }
            else //Resources加载
            {
                if (!groupName.EndsWith("Profile"))
                    groupName += "Profile";
                string path = Path.Combine(resDir, groupName);
                profile = Resources.Load<AuGroupProfile>(path);
                if (profile == null)
                    CTLogger.Log("没有找到AuGroupProfile，请检查路径：" + path, SysEnum.AudioSystem);
            }
            return profile;
        }

        //生产AuGroupProfile，根据AuProfile配置选择加载路径
        private static AuGroupProfile CreateAuGroupProfile(string groupName)
        {
            if (auProfile.isABLoad)
                return CreateAuGroupProfile(auProfile.abName, groupName);
            else
                return CreateAuGroupProfile(auProfile.profileDir, groupName);
        }

        //生成Au组对象
        //参数①：加载目录或者AB包
        //参数②：加载groupName名称
        public static AuGroup CreateAuGroup(AuGroupProfile profile)
        {
            AuGroup group = new AuGroup(profile);
            return group;
        }

        //生成AuGroup对象
        //参数①：加载目录或者AB包
        //参数②：加载AuGroupProfile名称
        public static AuGroup CreateAuGroup(string resDir, string groupName)
        {
            return CreateAuGroup(CreateAuGroupProfile(resDir, groupName));
        }

        //根据AuProfile配置选择加载路径，生成AuGroup对象
        public static AuGroup CreateAuGroup(string groupName)
        {
            return CreateAuGroup(CreateAuGroupProfile(groupName));
        }
    }
}