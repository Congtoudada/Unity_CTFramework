/****************************************************
  文件：CTConstant.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/16 21:21:22
  功能：
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public class CTConstant
    {
        ///全局配置文件默认加载路径
        public const string DEFAULT_CONFIG_PATH = "CTFramework/Default/CTConfig";

        ///其他配置
        //存档路径
        public static readonly string LOCAL_PATH = Application.streamingAssetsPath + "/playerData.json";

        /////资源系统
        ////资源默认配置文件加载路径
        //public const string DEFAULT_AB_PATH = "CTFramework/Default/ABProfile";

        /////UI系统
        ////UI默认配置文件加载路径
        //public const string DEFAULT_UI_PATH = "CTFramework/Default/UIProfile";

        /////Audio系统
        ////Au默认配置文件加载路径
        //public const string DEFAULT_AU_PATH = "CTFramework/Default/AuProfile";

        /////网络系统
        ////Net默认配置文件加载路径
        //public const string DEFAULT_NET_PATH = "CTFramework/Default/NetProfile";

        ///工具集
        //编辑器拓展本地缓存路径
        public const string EDITOR_CACHE_PATH = "Assets/Editor/CTFramework/EditorCache.json";
        //ViewTemplate的路径
        public const string EDITOR_TEMPLATE_VIEWPATH = "Assets/Editor/CTFramework/Template/ViewTemplate.txt";
        //AuKeyTemplate的路径
        public const string EDITOR_TEMPLATE_AUKEY = "Assets/Editor/CTFramework/Template/AuKeyTemplate.txt";

    }
}