/****************************************************
  文件：UIAutoScriptLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CT.Tools;
using CT.UISys;
using UnityEditor;
using UnityEngine;

namespace CT.Editor
{
    public static class UIAutoScriptLoader
    {
        public static bool CreateScriptsAndProfile_loader(this UIAutoScriptEditor self)
        {
            if (self.Check()) //合法性检查
            {
                //View脚本
                string viewClassName = self.panelRoot.name.Replace("Panel", "View"); //获取View.cs的类名
                string tp = CTConstant.EDITOR_TEMPLATE_VIEWPATH; //tp是templatePath的简写
                //如果含有示例，则加载示例模板
                if (self.withExample)
                {
                    tp = tp.Replace(Path.GetFileNameWithoutExtension(tp), Path.GetFileNameWithoutExtension(tp) + "_eg");
                }
                string viewFile = FileTool.ReadTextFromDisk(tp); //读取模板文件
                viewFile = self.ReplaceView(viewFile, viewClassName); //模板内容替换
                FileTool.WriteTextToDisk(Path.Combine(self.scriptPath, viewClassName + ".cs"), viewFile); //输出到目标路径

                //Profile
                PanelProfile profile = ScriptableObject.CreateInstance<PanelProfile>();
                self.ReplaceProfile(profile);
                string profilePath = Path.Combine(self.profileDir, self.panelRoot.name + "Profile.asset");
                AssetDatabase.CreateAsset(profile, profilePath);

                //缓存信息
                AssetDatabase.SaveAssets();
                return true;
            }

            return false;
        }
        
        #region 脚本生成辅助函数
        
        //View.cs自动创建过程
        private static string ReplaceView(this UIAutoScriptEditor self, string content, string className)
        {
            StringBuilder sb = new StringBuilder(content); //生成临时StringBuilder
            //替换类名和标题注释
            sb.Replace("#SCRIPTNAME#", className)
                .Replace("#Author#", self.author)
                .Replace("#Email#", self.email)
                .Replace("#DateTime#", DateTime.UtcNow.AddHours(8).ToString())
                .Replace("#Info#", self.description)
                .Replace("#ClassName#", self.panelRoot.name);

            //生成常量Key
            sb.Replace("#ConstantKey#", self.GetConstantKey());

            //主要内容替换
            if (self.panelRoot != null)
            {
                StringBuilder declaredUI = new StringBuilder(string.Empty);
                StringBuilder findUI = new StringBuilder(string.Empty);
                //如果非纯净版，需要查找UI组件
                if (!self.isPure)
                {
                    Stack<string> stack = new Stack<string>();
                    self.ForeachUI(declaredUI, findUI, self.panelRoot.transform, stack); //PS: 根面板不算在内
                }
                sb.Replace("#DeclaredUI#", declaredUI.ToString());
                sb.Replace("#FindUI#", findUI.ToString());
            }
            return sb.ToString();
        }

        //生成常量Key
        private static string GetConstantKey(this UIAutoScriptEditor self)
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            //判断是否生成常量Key
            if (self.isConstKey)
            {
                sb.Append("\t//声明常量Key").AppendLine();
                //根据keyValues生成常量Key
                for(int i = 0; i < self.keyValues.Count; i++)
                {
                    string keyValue = "Key_" + self.keyValues[i].key;
                    if (self.isUpperKey)
                        keyValue = self.keyValues[i].key.ToUpper();//大写

                    sb.Append("\tpublic const string ")
                    .Append(keyValue)
                    .Append(" = \"")
                    .Append(self.keyValues[i].key)
                    .Append("\";")
                    .AppendLine();
                }
            }
            return sb.ToString();
        }

        //深度搜索所有UI并赋值
        private static void ForeachUI(this UIAutoScriptEditor self, StringBuilder declaredUI, StringBuilder findUI, Transform parent, Stack<string> stack)
        {
            stack.Push(parent.name);
            //遍历所有子对象（不包括自身）
            foreach (Transform item in parent)
            {
                //判断item是否满足条件，满足则记录( xx_xx )
                if (item.name.Contains("_"))
                {
                    string type = item.name.Split('_')[1].ToLower(); //拿到类型信息
                    if (!string.IsNullOrEmpty(type) && self.IsSupportUI(type))
                    {
                        //符合类型规范
                        string variableName = item.name.Split('_')[0]; //拿到变量名

                        declaredUI.Append("\tpublic "); //public声明

                        //findUI 左值语句
                        findUI.Append("\t\t").Append(variableName).Append(" = ");
                        //右值语句
                        StringBuilder findSentence = new StringBuilder();
                        findSentence.Append("transform");
                        string[] array = stack.ToArray();
                        Array.Reverse(array);
                        for (int i = 1; i < array.Length; i++) //从1开始遍历，排除自身
                        {
                            if (array[i].Contains("_")) //如果在遍历过程中发现符合规范的组件，可从该组件开始遍历Find
                            {
                                findSentence.Clear(); //清空，从此开始遍历
                                findSentence.Append(array[i].Split('_')[0]).Append(".transform");
                            }
                            else
                            {
                                findSentence.Append(".Find(\"").Append(array[i]).Append("\")");
                            }
                        }
                        findUI.Append(findSentence); //赋值部分右值
                        findUI.Append(".Find(\"").Append(item.name).Append("\")");
                        //到目前为止:
                        //DeclaredUI: public 
                        //findUI: transform.Find(xxx)...Find(自己)
                        //根据类型设置值
                        self.SetTypeString(type, declaredUI, findUI);
                        //declaredUI 收尾
                        declaredUI.Append(" ").Append(variableName + ";"); //声明变量名
                        declaredUI.AppendLine();
                        //findUI 收尾
                        findUI.Append(";").AppendLine();
                    }
                }

                //如果该item还有孩子，继续从这里遍历
                if (item.childCount > 0)
                {
                    self.ForeachUI(declaredUI, findUI, item, stack);
                }
            }
            stack.Pop();
        }

        //根据Type赋值
        private static void SetTypeString(this UIAutoScriptEditor self, string type, StringBuilder declaredUI, StringBuilder findUI)
        {
            if (self.objSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("GameObject");
                findUI.Append(".gameObject");
                return;
            }
            findUI.Append(".GetComponent<");
            if (self.buttonSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Button");
                findUI.Append("Button");
            }
            else if (self.textSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Text");
                findUI.Append("Text");
            }
            else if (self.imgSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Image");
                findUI.Append("Image");
            }
            else if (self.inputSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("InputField");
                findUI.Append("InputField");
            }
            else if (self.dropdownSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Dropdown");
                findUI.Append("Dropdown");
            }
            else if (self.sliderSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Slider");
                findUI.Append("Slider");
            }
            else if (self.svSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("ScrollView");
                findUI.Append("ScrollView");
            }
            else if (self.sbSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Scrollbar");
                findUI.Append("Scrollbar");
            }
            else if (self.toggleSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("Toggle");
                findUI.Append("Toggle");
            }
            else if (self.tmpSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("TextMeshProUGUI");
                findUI.Append("TextMeshProUGUI");
            }
            else if (self.rectSuffix.Any(v => v.Equals(type)))
            {
                declaredUI.Append("RectTransform");
                findUI.Append("RectTransform");
            }
            findUI.Append(">()");
        }

        //检查Type合法性
        private static bool IsSupportUI(this UIAutoScriptEditor self, string type)
        {
            return self.objSuffix
                    .Union(self.buttonSuffix)
                    .Union(self.textSuffix)
                    .Union(self.imgSuffix)
                    .Union(self.inputSuffix)
                    .Union(self.dropdownSuffix)
                    .Union(self.sliderSuffix)
                    .Union(self.svSuffix)
                    .Union(self.sbSuffix)
                    .Union(self.toggleSuffix)
                    .Union(self.tmpSuffix)
                    .Union(self.rectSuffix)
                    .Any(v => v.Equals(type));
        }

        //Profile赋值
        private static void ReplaceProfile(this UIAutoScriptEditor self, PanelProfile profile)
        {
            profile.panelName = self.panelRoot.name; //面板名称就是Prefab的名称
            profile.isCache = self.isCache; //是否缓存
            profile.isPreLoad = self.isPreLoad; //是否预加载
            profile.isAssetBundle = self.isAssetBundle; //是否AB包加载
            profile.resourcesDir = self.resourcesDir; //Resources加载路径
            profile.abName = self.abName; //AB包名称
            if (self.keyValues.Count > 0) profile.isCacheProfile = true;
            //keyValues浅拷贝？试试
            foreach (var editorItem in self.keyValues)
            {
                profile.keyValues.Add(editorItem);
            }
        }
        
        #endregion
        
        #region 辅助函数
        
        //空字段检查
        private static bool Check(this UIAutoScriptEditor self)
        {
            if (!EditorVerify.RequiredCheck(self, out self.createInfo))
            {
                return false;
            }
            //路径合法性检查
            if (!Directory.Exists(self.scriptPath) || !Directory.Exists(self.profileDir))
            {
                self.createInfo = "路径有问题，生成失败！请检查脚本生成路径和配置文件生成路径！";
                return false;
            }
            return true;
        }
        
        #endregion
    }
}