/****************************************************
  文件：UIAutoScriptEditor.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 14:22:45
  功能：UI自动脚本
*****************************************************/
using CT.UISys;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CT.Tools;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace CT.Editor
{
    public class UIAutoScriptEditor
    {
        #region 脚本参数(含必填项)
        [ToggleGroup("ScriptParam", "脚本参数(含必填项)")]
        public bool ScriptParam = true;

        [ToggleGroup("ScriptParam")]
        [Title("面板根对象", "传入面板对象的Prefab 命名规范：xxxPanel")]
        [Tooltip("最终会根据这个Prefab生成三个文件 xxxPanel.cs, xxxView.cs和xxxPanelProfile.asset")]
        [Required]
        public GameObject panelRoot;

        [ToggleGroup("ScriptParam")]
        [Title("脚本输出目录")]
        [FolderPath, Required, EditorCache]
        public string scriptPath = "";

        [ToggleGroup("ScriptParam")]
        [Title("是否使用纯净版(无UI自动绑定)")]
        [EditorCache]
        public bool isPure = false;

        [ToggleGroup("ScriptParam")]
        [Title("是否包含示例")]
        [EditorCache]
        public bool withExample = true;

        [ToggleGroup("ScriptParam")]
        [Title("是否生成常量Key")]
        [EditorCache]
        public bool isConstKey = true;

        [ToggleGroup("ScriptParam")]
        [Title("常量Key的声明是否均为大写")]
        [ShowIf("isConstKey")]
        [EditorCache]
        public bool isUpperKey = false;
        #endregion

        #region 配置文件参数(含必填项)
        [ToggleGroup("ProfileParam", "配置文件参数(含必填项)")]
        public bool ProfileParam = true;

        [ToggleGroup("ProfileParam")]
        [Title("配置文件输出目录")]
        [FolderPath, Required, EditorCache]
        public string profileDir = "";
        
        [ToggleGroup("ProfileParam")]
        [HideIf("isAssetBundle")]
        [Title("UI面板预制体所在目录，勿加资源名(Resources)")]
        [FolderPath, EditorCache]
        [OnValueChanged("OnResourcesDirChanged")]
        public string resourcesDir = "";

        [ToggleGroup("ProfileParam")]
        [Title("UI面板是否缓存")]
        [EditorCache]
        public bool isCache = true;
        
        [ToggleGroup("ProfileParam")]
        [Title("UI面板是否预加载")]
        [EditorCache]
        public bool isPreLoad = false;

        [ToggleGroup("ProfileParam")]
        [Title("是否启用AB包加载面板游戏对象")]
        [EditorCache]
        public bool isAssetBundle = false;

        [ToggleGroup("ProfileParam")]
        [ShowIf("isAssetBundle")]
        [Title("UI面板预制体所在主包(AssetBundle)")]
        [EditorCache]
        public string abName;

        [ToggleGroup("ProfileParam")]
        [Title("ModelView键值对")]
        public List<UIItem> keyValues;
        #endregion

        #region 脚本额外信息
        [ToggleGroup("AddtionalParam", "脚本额外参数(选填)")]
        public bool AddtionalParam;

        [ToggleGroup("AddtionalParam")]
        [Title("脚本的作者")]
        [EditorCache]
        public string author = "";

        [ToggleGroup("AddtionalParam")]
        [Title("脚本的邮箱")]
        [EditorCache]
        public string email = "";

        [ToggleGroup("AddtionalParam")]
        [Title("脚本的功能描述")]
        [TextArea, EditorCache]
        public string description = "";

        #endregion

        #region 属性
        private EditorDict dict;
        public string createInfo = "生成助手：生成过程中有问题我在这里告诉你！";

        //后缀集合（比较时忽略大小写）
        //记法：①全称肯定没有问题，但是命名累；②首字母前3个（除TextMeshPro，ScrollView和 Bar 外）；③常用缩写形式（如img, sv）
        public readonly List<string> objSuffix = new List<string> { "go", "obj", "object", "gameobject" };
        public readonly List<string> buttonSuffix = new List<string> { "but", "btn",  "button" };
        public readonly List<string> textSuffix = new List<string> { "tex", "text", "txt" };
        public readonly List<string> imgSuffix = new List<string> { "ima", "img", "image" };
        public readonly List<string> inputSuffix = new List<string> { "inp", "input", "inputfield" };
        public readonly List<string> dropdownSuffix = new List<string> { "dd", "dro", "drop", "dropdown" };
        public readonly List<string> sliderSuffix = new List<string> { "sli", "slider" };
        public readonly List<string> svSuffix = new List<string> { "sv", "view", "scrollview" };
        public readonly List<string> sbSuffix = new List<string> { "sb", "bar", "scrollbar" };
        public readonly List<string> toggleSuffix = new List<string> { "tog", "toggle" };
        public readonly List<string> tmpSuffix = new List<string> { "tmp", "textmeshpro" };
        public readonly List<string> rectSuffix = new List<string> { "rec", "rect", "recttransform" };
        #endregion

        public UIAutoScriptEditor(EditorDict dict)
        {
            this.dict = dict;
            dict.InitFields(this);
            ReadKeyValues(dict);
        }

        [Button("保存设置", ButtonSizes.Large)]
        public void SaveBtn()
        {
            SaveKeyValues(dict);
            dict.SaveFields(this); //自带 AssetDatabase.Refresh();
        }

        [InfoBox("$createInfo", InfoMessageType = InfoMessageType.Info)]
        [DetailedInfoBox("支持的UI类型及后缀描述", "目前支持UI自动绑定的组件及后缀有(可忽略大小写)：\n" +
            "GameObject: go, obj, object, gameobject\n" +
            "Button: but, btn, button\n" +
            "Text: tex, text, txt\n" +
            "Image: ima, img, image\n" +
            "InputField: inp, input, inputfield \n" +
            "Dropdown: dd, dro, drop, dropdown\n" +
            "Slider: sli, slider\n" +
            "ScrollView: sv, view, scrollview (UnityEngine.UIElements) \n" +
            "ScrollBar: sb, bar, scrollbar\n" +
            "Toggle: tog, toggle\n" +
            "TextMeshProUGUI: tmp, textmeshpro (TMPro)\n" +
            "RectTransform: rec, rect, recttransform",
            InfoMessageType = InfoMessageType.None)]
        [Button("生成", ButtonSizes.Large)]
        public void CreateScriptsAndProfile()
        {
            if (this.CreateScriptsAndProfile_loader())
            {
                SaveKeyValues(dict);
                dict.SaveFields(this); //自带 AssetDatabase.Refresh();
            }
        }

        private int clickCount;
        //清空List
        [ToggleGroup("ProfileParam")]
        [GUIColor(0.929f, 0.353f, 0.396f)]
        [Button("一键清空Key-Values", ButtonSizes.Large)]
        public void ClearList()
        {
            if (++clickCount >= 2)
            {
                createInfo = "Key-Values已清空！";
                keyValues.Clear();
                SaveKeyValues(dict);
                dict.WriteData();
                clickCount = 0;
            }
            else
            {
                createInfo = "您真的要清空Key-Values吗？";
            }
        }

        #region 辅助函数
        //修改Resources路径
        private void OnResourcesDirChanged()
        {
            resourcesDir = EditorHelper.GetResourcesPath(resourcesDir);
        }
        //读取键值对缓存
        private void ReadKeyValues(EditorDict dict)
        {
            string key = GetType().Name + "_keyValues";
            if (dict.editorDict.ContainsKey(key))
            {
                keyValues = JsonConvert.DeserializeObject<List<UIItem>>(dict.editorDict[key]);
                foreach (var item in keyValues)
                    item.ModifyTitle();
            }
        }
        //缓存键值对
        private void SaveKeyValues(EditorDict dict)
        {
            string key = GetType().Name + "_keyValues";
            string value = JsonConvert.SerializeObject(keyValues);
            if (dict.editorDict.ContainsKey(key))
            {
                dict.editorDict[key] = value;
            }
            else
            {
                dict.editorDict.Add(key, value);
            }
        }

        #endregion
    }
}
