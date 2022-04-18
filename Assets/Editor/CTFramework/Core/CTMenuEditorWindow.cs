/****************************************************
  文件：CTMenuEditorWindow.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 15:42:48
  功能：Nothing
*****************************************************/
using CT.UISys;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CT.Editor
{
    public class CTMenuEditorWindow : OdinMenuEditorWindow
    {
        public EditorDict dict; //用于缓存编辑器数据

        [MenuItem("聪头框架/工具箱")]
        private static void OpenWindow()
        {
            GetWindow<CTMenuEditorWindow>("聪头百宝箱").Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            dict = new EditorDict();

            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;

            tree.Add("UI脚本一键生成", new UIAutoScriptEditor(dict));
            tree.Add("AssetBundle资源清单", new ABUtilityEditor(dict));
            tree.Add("AuKey一键生成", new AuAutoKeyEditor(dict));
            return tree;
            
        }
    }
}
