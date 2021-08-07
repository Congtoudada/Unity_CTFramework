/****************************************************
  文件：UITool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 16:42:26
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CT.Tools
{
    public class UniTool
    {
        #region 查找类工具
        //深搜：根据名称查找第一个子对象
        //参数①：从哪开始查找（根对象）②：对象名称 ③：是否打印测试
        public static GameObject FindChildObj(GameObject obj, string name, bool isDebug = false)
        {
            if(isDebug)
            {
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                
                GameObject result =  FindChildGameObject(obj, name, sb);
                if (result != null)
                {
                    sb.Remove(sb.ToString().LastIndexOf("-"), 2);
                    DebugMgr.Log(sb.ToString(), SystemEnum.Tools);
                 }
                return result;
            }
            else
            {
                return FindChildGameObject(obj, name, null);
            }
        }

        private static GameObject FindChildGameObject(GameObject obj, string name, StringBuilder sb)
        {
            GameObject result = null;
            //如果找到，则返回
            if (obj.name.Equals(name))
                return obj;
            //如果不是，则遍历所有子项
            foreach (Transform item in obj.transform)
            {
                result = FindChildGameObject(item.gameObject, name, sb);
                if (result != null)
                {
                    if(sb != null)
                        sb.Insert(0, item.name + "->");
                    return result; //如果找到，直接返回，节省性能
                }
            }
            return null;
        }

        //根据名称获取一个子对象的组件，传入子对象obj可以节省遍历时间
        //参数①：从哪开始查找（根对象）②：对象名称 ③：如果没有是否添加
        public static T FindComponent<T>(GameObject obj, string name, bool isAdd = false, bool isDebug = false) where T : Component
        {
            GameObject child = FindChildObj(obj, name, isDebug);
            if (child)
            {
                if (child.GetComponent<T>() == null)
                {
                    if(isAdd)
                        child.AddComponent<T>();
                    else
                        DebugMgr.Log($"{name}里没有找到 { typeof(T) } 的组件", SystemEnum.Tools);
                }  
                return child.GetComponent<T>();
            }
            else
            {
                DebugMgr.Log($"{obj.name}里找不到名为{name}的子对象", SystemEnum.Tools);
                return null;
            }
        }
        #endregion

        #region 赋值类工具
        //给对象的Text组件赋值文本
        public static void SetText(GameObject obj, string content)
        {
            obj.GetComponent<Text>().text = content;
        }
        //给对戏的Image组件赋值Sprite
        public static void SetSprite(GameObject obj, Sprite sprite)
        {
            obj.GetComponent<Image>().sprite = sprite;
        }

        #endregion
    }
}
