/****************************************************
  文件：CTConstant.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 16:30:32
  功能：保存所有常量
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT
{
    public class CTConstant
    {
        #region 资源加载系统
        //AB包加载配置文件存放路径，不可改
        public const string AB_PROFILE_PATH = "CTProfile/ResourceSystem/ABLoaderProfile";
        //生成的AB包清单列表名称，建议
        public const string AB_VERSION_LIST = "versionList.json";
        #endregion

        #region UI系统
        //UI核心配置文件存放路径，不可改
        public const string UI_CORE_PROFILE_PATH = "CTProfile/UISystem/UICoreProfile";
        //UI预制体 存放目录，建议，可以在配置文件中修改
        public const string UI_PREFAB_DIR_PATH = "UI/Prefab/";
        //ModelView 文件存放目录，建议，可以在配置文件中修改
        public const string UI_MODELVIEW_DIR_PATH = "UI/Profile/";
        #endregion

        #region 音乐系统
        //音乐核心配置文件存放路径，不可改
        public const string AU_CORE_PROFILE_PATH = "CTProfile/AudioSystem/AudioCoreProfile";
        //音乐剪辑 存放目录，建议，可以在配置文件中修改
        public const string AUDIO_CLIP_DIR_PATH = "Audio/Clip/";
        //音乐配置 文件存放目录，建议，可以在配置文件中修改
        public const string AUDIO_PROFILE_DIR_PATH = "Audio/Profile/";
        //内置音乐加载器的键
        public const string AUDIO_DEFAULT_KEY = "Congtou";
        #endregion

        #region 编辑器拓展
        //编辑器拓展本地缓存路径，建议
        public const string EDITOR_CACHE_PATH = "Assets/Editor/editor.json";
        #endregion
    }
}
