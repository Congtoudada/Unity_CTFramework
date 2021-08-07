/****************************************************
  文件：UILoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 18:01:14
  功能：Nothing
*****************************************************/
using CT.ResourceSys;
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UISys
{
    public class UILoader : IRelease
    {
        //储存所有ui游戏对象的字典
        private Dictionary<string, GameObject> _dicUI;

        public UILoader()
        {
            _dicUI = new Dictionary<string, GameObject>();
        }

        //创建一个GameObject并缓存
        public GameObject CreateUI(LoadBaseProfile profile)
        {
            GameObject parent = GameObject.Find("Canvas"); //获取当前场景的Canvas
            if (!parent)
            {
                DebugMgr.Error("Canvas不存在，请仔细查找有无这个对象", SystemEnum.UISystem);
                return null;
            }
            if (_dicUI.ContainsKey(profile.filename))
                return _dicUI[profile.filename];

            GameObject obj = Create(profile, parent);
            if(obj != null)
            {
                obj.name = profile.filename;
                _dicUI.Add(profile.filename, obj);
            }
            return obj;
        }

        //加载并创建一个UI GameObject
        private GameObject Create(LoadBaseProfile profile, GameObject parent)
        {
            GameObject template = profile.Load<GameObject>();
            if (template == null)
                return null;
            else
                return GameObject.Instantiate(template, parent.transform);
        }

        //摧毁一个ui对象
        public void DestroyUI(LoadBaseProfile profile)
        {
            if (_dicUI.ContainsKey(profile.filename))
            {
                //GameObject.Destroy(_dicUI[profile.filename]);
                _dicUI.Remove(profile.filename);
            }
        }

        //获得一个已加载的ui对象
        public GameObject GetUIObject(string name)
        {
            if (_dicUI.ContainsKey(name))
            {
                return _dicUI[name];
            }
            else
            {
                Debug.Log("没有找到面板对象: " + name);
                return null;
            }
        }

        public void Release()
        {
            _dicUI.Clear();
            _dicUI = null;
        }
    }
}
