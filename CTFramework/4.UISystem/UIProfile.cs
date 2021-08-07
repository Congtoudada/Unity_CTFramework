/****************************************************
  文件：UIProfile.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 16:35:15
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.UISys
{
    //public abstract class UIProfile
    //{
    //    public string filename; //资源名称

    //    public abstract GameObject LoadGameObject();
    //}

    //public class UIResProfile : UIProfile
    //{
    //    public string dirPath; //Resource所在路径，不包括文件名

    //    public UIResProfile(string dirPath)
    //    {
    //        this.dirPath = dirPath;
    //    }

    //    public UIResProfile()
    //    {
    //        dirPath = _CT.UIMgr.coreProfile.prefabPath;
    //    }

    //    public override GameObject LoadGameObject()
    //    {
    //        string path = Path.Combine(dirPath, filename);
    //        GameObject template = _CT.ResMgr.LoadByRes<GameObject>(path);
    //        if (template == null)
    //            DebugMgr.Warning("没有找到资源，请检查路径: " + path, SystemEnum.UISystem);
    //        return template;
    //    }
    //}

    //public class UIABProfile : UIProfile
    //{
    //    public string abName; //AB包名

    //    public UIABProfile(string abName)
    //    {
    //        this.abName = abName;
    //    }

    //    public override GameObject LoadGameObject()
    //    {
    //        GameObject template = _CT.ResMgr.LoadByAB<GameObject>(abName, filename);
    //        if (template == null)
    //            DebugMgr.Warning("没有找到资源，请检查ab包: " + abName, SystemEnum.UISystem);
    //        return template;
    //    }
    //}
}
