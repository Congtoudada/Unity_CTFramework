/****************************************************
  文件：UIAutoScriptEditor.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 14:22:45
  功能：UI自动脚本
*****************************************************/
using CT.FileSys;
using CT.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CT.UISys
{
    public class UIAutoScriptEditor
    {
        [BoxGroup("基本参数 (必填)")]
        [DetailedInfoBox("说明", "按规范，执行生成后，最终会生成三个文件 xxxPanel.cs, xxxViewComponent.cs和xxxPanelProfile.asset")]
        [Title("面板根对象", "命名最好以 Panel 结尾比较符合规范")]
        [Required]
        public GameObject panelRoot;

        [BoxGroup("基本参数 (必填)")]
        [Title("输出路径")]
        [FolderPath, Required]
        public string outputPath = "";

        [BoxGroup("基本参数 (必填)")]
        [Title("是否使用纯净版(无UI自动绑定)")]
        public bool isPure = false;

        [ToggleGroup("ModelView")]
        public bool ModelView;

        [DetailedInfoBox("说明", "ModelView作为前后端数据传递的载体，可以在这里预先设定好键值对，" +
            "也可以在脚本中动态绑定。建议根据实际用途放到相应字典中，可以减少不必要的性能消耗。" +
            "且不同字典中的键可以重复，不影响。" +
            "\n默认生成在Assets/Resources/*下")]
        [ToggleLeft]
        [ToggleGroup("ModelView")]
        [LabelText("修改ModelView生成路径")]
        public bool isModify = false;

        [LabelText("默认输出路径（相对Resources目录）")]
        [EnableIf("isModify")]
        [ToggleGroup("ModelView")]
        public string modelviewPath = "";

        [ToggleGroup("ModelView")]
        [LabelText("ModelView键值对(不同类型Key可以重复")]
        public ModelViewInfo mvInfo;

        [ToggleGroup("AddtionInfo")]
        public bool AddtionInfo;

        [ToggleGroup("AddtionInfo")]
        public AdditionInfo addition;

        private UICoreProfile profile;
        private EditorDict dict;

        public UIAutoScriptEditor(EditorDict dict)
        {
            this.dict = dict;
            profile = Resources.Load<UICoreProfile>(CTConstant.UI_CORE_PROFILE_PATH);
            addition = new AdditionInfo();
            mvInfo = new ModelViewInfo();

            if (dict != null)
            {
                outputPath = dict.editorDict[GetType() + "_outputPath"];
                isPure = Convert.ToBoolean(dict.editorDict[GetType() + "_isPure"]);
                addition.noExample = Convert.ToBoolean(dict.editorDict[GetType() + "_noExample"]);
                addition.author = dict.editorDict[GetType() + "_author"];
                addition.email = dict.editorDict[GetType() + "_email"];
                modelviewPath = dict.editorDict[GetType() + "_modelviewPath"];
                if (string.IsNullOrEmpty(modelviewPath))
                    modelviewPath = profile.modelviewPath;
            }
        }

        [Button("生成", ButtonSizes.Large)]
        public void CreateScripts() 
        {
            if(panelRoot != null && !string.IsNullOrEmpty(outputPath))
            {
                if(dict != null)
                {
                    dict.editorDict[GetType() + "_outputPath"] = outputPath;
                    dict.editorDict[GetType() + "_isPure"] = isPure.ToString();
                    dict.editorDict[GetType() + "_noExample"] = addition.noExample.ToString();
                    dict.editorDict[GetType() + "_author"] = addition.author;
                    dict.editorDict[GetType() + "_email"] = addition.email;
                    dict.editorDict[GetType() + "_modelviewPath"] = modelviewPath;
                    dict.WriteData();
                }
     
                ///ModelPanel派生类
                string modelPath = profile.modelPath;
                if (!addition.noExample)
                {
                    modelPath = modelPath.Replace(Path.GetFileNameWithoutExtension(modelPath), Path.GetFileNameWithoutExtension(modelPath) + "_eg");
                }
                string modelFile = FileTool.ReadString(modelPath);
                modelFile = ReplaceModel(modelFile); //替换信息
                FileTool.WriteString(Path.Combine(outputPath, panelRoot.name + ".cs"), modelFile);

                ///ViewComponent派生类
                string viewPath = profile.viewPath;
                if (!addition.noExample)
                {
                    viewPath = viewPath.Replace(Path.GetFileNameWithoutExtension(viewPath), Path.GetFileNameWithoutExtension(viewPath) + "_eg");
                }
                string viewClass = panelRoot.name.Replace("Panel", "ViewComponent");
                string viewFile = FileTool.ReadString(viewPath);
                viewFile = ReplaceView(viewFile, viewClass); //替换信息
                FileTool.WriteString(Path.Combine(outputPath, viewClass + ".cs"), viewFile);

                ///ModelView配置文件
                UIModelView mv = SerializedScriptableObject.CreateInstance<UIModelView>();
                ReplaceModelView(mv);
                string mvPath = Path.Combine("Assets/Resources/", modelviewPath, panelRoot.name + "Profile.asset");
                if (!Directory.Exists(Path.GetDirectoryName(mvPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(mvPath));
                }
                AssetDatabase.CreateAsset(mv, mvPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                DebugMgr.Log("脚本生成成功，请检查: " + outputPath);
                DebugMgr.Log("配置生成成功，请检查: " + mvPath);
            }
            else
            {
                DebugMgr.Log("生成失败，请确保面板对象和路径正确", SystemEnum.UISystem);
            }
        }

        //替换Model文件
        private string ReplaceModel(string str)
        {
            return ReplaceAddtion(str).Replace("#SCRIPTNAME#", panelRoot.name);
        }

        //替换View文件
        private string ReplaceView(string str, string className)
        {
            //自动生成基本信息
            str = ReplaceAddtion(str).Replace("#SCRIPTNAME#", className);

            //自动生成UI组件
            if (panelRoot != null)
            {
                StringBuilder bindUI = new StringBuilder(string.Empty);
                StringBuilder findUI = new StringBuilder(string.Empty);
                if(!isPure)
                {
                    Stack<string> stack = new Stack<string>();
                    Queue<string> queue = new Queue<string>();
                    foreachUI(bindUI, findUI, panelRoot.transform, stack); //PS: 根面板不算在内
                }
                str = str.Replace("#BindingUI#", bindUI.ToString());
                str = str.Replace("#FindUI#", findUI.ToString());
            }
            return str;
        }

        //深度搜索所有UI
        private void foreachUI(StringBuilder bindUI, StringBuilder findUI, Transform parent, Stack<string> stack)
        {
            stack.Push(parent.name);
            foreach (Transform item in parent)
            {
                //判断item是否满足条件，满足则记录
                if (item.name.Contains("_"))
                {
                    string type = item.name.Split('_')[1];
                    if (!string.IsNullOrEmpty(type))
                    {
                        //符合规范
                        //bindUI
                        if(!isPure)
                        {
                            bindUI.Append("\t[HideInInspector]");
                            bindUI.AppendLine();
                        }
                        bindUI.Append("\tpublic ");
                        //findUI
                        findUI.Append("\t\t").Append(item.name).Append(" = ").Append("transform");
                        string[] array = stack.ToArray();
                        Array.Reverse(array);
                        for (int i = 1; i < array.Length; i++)
                        {
                            findUI.Append(".Find(\"").Append(array[i]).Append("\")");
                        }
                        findUI.Append(".Find(\"").Append(item.name).Append("\")");
                        findUI.Append(".GetComponent<");

                        //根据后缀取得类型
                        switch (type)
                        {
                            case "button":
                                bindUI.Append("Button");
                                findUI.Append("Button");
                                break;
                            case "image":
                                bindUI.Append("Image");
                                findUI.Append("Image");
                                break;
                            case "text":
                                bindUI.Append("Text");
                                findUI.Append("Text");
                                break;
                            case "dropdown":
                                bindUI.Append("Dropdown");
                                findUI.Append("Dropdown");
                                break;
                            case "slider":
                                bindUI.Append("Slider");
                                findUI.Append("Slider");
                                break;
                            case "scrollview":
                                bindUI.Append("ScrowView");
                                findUI.Append("ScrowView");
                                break;
                            case "toggle":
                                bindUI.Append("Toggle");
                                findUI.Append("Toggle");
                                break;
                        }

                        //bindUI
                        bindUI.Append(" ").Append(item.name + ";");
                        bindUI.AppendLine();
                        //findUI
                        findUI.Append(">()").Append(";").AppendLine();
                    }
                }

                //如果该item还有孩子，继续从这里遍历
                if (item.childCount > 0)
                {
                    foreachUI(bindUI, findUI, item, stack);
                }
            }
            stack.Pop();
        }

        //替换额外信息
        private string ReplaceAddtion(string str)
        {
            str = str.Replace("#Author#", addition.author)
                .Replace("#Email#", addition.email)
                .Replace("#DateTime#", DateTime.UtcNow.AddHours(8).ToString())
                .Replace("#Info#", addition.info);
            return str;
        }

        //绑定ModelView数据
        private void ReplaceModelView(UIModelView mv)
        {
            foreach(string key in mvInfo.stringDict.Keys)
            {
                mv.SetUIString(key, mvInfo.stringDict[key], true);
            }
            foreach (string key in mvInfo.floatDict.Keys)
            {
                mv.SetUIFloat(key, mvInfo.floatDict[key], true);
            }
            foreach (string key in mvInfo.intDict.Keys)
            {
                mv.SetUIInt(key, mvInfo.intDict[key], true);
            }
            foreach (string key in mvInfo.boolDict.Keys)
            {
                mv.SetUIBool(key, mvInfo.boolDict[key], true);
            }
        }

        [Serializable]
        public class AdditionInfo
        {
            [Title("是否含示例")]
            public bool noExample = false;

            [Title("作者")]
            public string author = "";

            [Title("邮箱")]
            public string email = "";

            [Title("功能描述")]
            [TextArea]
            public string info = "";
        }

        [Serializable]
        public class ModelViewInfo
        {
            [ToggleLeft]
            [LabelText("激活重置功能")]
            public bool isCanReset;

            [TabGroup("string键值对")]
            [ShowInInspector]
            [LabelText("string键值对")]
            public Dictionary<string, string> stringDict = new Dictionary<string, string>();

            [TabGroup("float键值对")]
            [ShowInInspector]
            [LabelText("flaot键值对")]
            public Dictionary<string, float> floatDict = new Dictionary<string, float>();

            [TabGroup("int键值对")]
            [ShowInInspector]
            [LabelText("int键值对")]
            public Dictionary<string, int> intDict = new Dictionary<string, int>();

            [TabGroup("bool键值对")]
            [ShowInInspector]
            [LabelText("bool键值对")]
            public Dictionary<string, bool> boolDict = new Dictionary<string, bool>();

            [Button(Name = "重置")]
            [ShowIf("isCanReset")]
            [TabGroup("string键值对")]
            [GUIColor(0.9294f, 0.3176f, 0.149f)]
            public void ResetString()
            {
                stringDict.Clear();
            }

            [Button(Name = "重置")]
            [ShowIf("isCanReset")]
            [TabGroup("float键值对")]
            [GUIColor(0.9294f, 0.3176f, 0.149f)]
            public void ResetFloat()
            {
                floatDict.Clear();
            }

            [Button(Name = "重置")]
            [ShowIf("isCanReset")]
            [TabGroup("int键值对")]
            [GUIColor(0.9294f, 0.3176f, 0.149f)]
            public void ResetInt()
            {
                intDict.Clear();
            }

            [Button(Name = "重置")]
            [ShowIf("isCanReset")]
            [TabGroup("bool键值对")]
            [GUIColor(0.9294f, 0.3176f, 0.149f)]
            public void ResetBool()
            {
                boolDict.Clear();
            }
        }
    }
}
