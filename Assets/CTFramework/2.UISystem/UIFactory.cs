/****************************************************
  文件：UIFactory.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/15 21:26:15
  功能：
*****************************************************/
using CT.UISys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.UISys
{
    public static class UIFactory
    {
        //缓存池
        private static List<BasePanel> cachePool;
        //单个缓存
        private static BasePanel cache;
        //UI总配置文件
        private static UIProfile uiProfile;

        //生产UIMgr
        public static UIMgr Build(ScriptableObject profile)
        {
            UIMgr mgr = null;
            if (profile != null)
            {
                mgr = new UIMgr();
                UIProfile uiProfile = profile as UIProfile;
                if (uiProfile != null)
                {
                    UIFactory.uiProfile = uiProfile;
                    if (!uiProfile.isSingleCache)
                    {
                        cachePool = new List<BasePanel>(uiProfile.maxCache);
                    }
                    mgr.SetLoader(new PanelLoader());
                }
            }
            return mgr;
        }

        #region 生产PanelProfile

        //生产PanelProfile
        //参数①：加载目录或者AB包
        //参数②：加载PanelProfile名称
        private static PanelProfile CreatePanelProfile(string resDir, string panelName)
        {
            PanelProfile profile = null;
            if (uiProfile.isABLoad) //AB包加载
            {
                profile = _CT.ResMgr.LoadRes<PanelProfile>(resDir, panelName);
                if (profile == null)
                    CTLogger.Log("没有找到PanelProfile，请检查AB包是否存在：" + resDir, SysEnum.UISystem);
            }
            else //Resources加载
            {
                if (!panelName.EndsWith("Profile"))
                    panelName += "Profile";
                string path = Path.Combine(resDir, panelName);
                profile = Resources.Load<PanelProfile>(path);
                if (profile == null)
                    CTLogger.Log("没有找到PanelProfile，请检查路径：" + path, SysEnum.UISystem);
            }
            return profile;
        }

        //生产PanelProfile，根据UIProfile配置选择加载路径
        private static PanelProfile CreatePanelProfile(string panelName)
        {
            if (uiProfile.isABLoad)
                return CreatePanelProfile(uiProfile.abName, panelName);
            else
                return CreatePanelProfile(uiProfile.profileDir, panelName);
        }
        #endregion

        #region 生产BasePanel

        //生成UI面板对象（配置文件生成）
        private static BasePanel CreateBasePanel(PanelProfile profile)
        {
            BasePanel panel = null;
            //如果使用单缓存技术且缓存存在
            if (uiProfile.isSingleCache)
            {
                if (cache != null && cache.panelName.Equals(profile.panelName))
                {
                    panel = cache;
                    cache = null;
                    return panel;
                }
            }
            else //如果使用缓存池技术且缓存存在
            {
                foreach(BasePanel item in cachePool)
                {
                    if (item.panelName.Equals(profile.panelName))
                    {
                        panel = item;
                        cachePool.Remove(item);
                        return panel;
                    }
                }
            }

            try
            {
                //没有缓存过，就new出来
                panel = new BasePanel(profile);
            }
            catch (NullReferenceException ex)
            {
                CTLogger.Warning("加载GameObject失败，请检查路径: " +
                                 (profile.isAssetBundle
                                     ? $"{profile.abName} {profile.panelName}"
                                     : $"{profile.resourcesDir} {profile.panelName}"));
                CTLogger.Warning(ex.ToString());
            } catch (Exception ex)
            {
                CTLogger.Warning(ex.ToString());
            }
            return panel;
        }

        //生成UI面板对象
        //参数①：加载目录或者AB包
        //参数②：加载PanelProfile名称
        public static BasePanel CreateBasePanel(string resDir, string panelName)
        {
            return CreateBasePanel(CreatePanelProfile(resDir, panelName));
        }

        //根据UIProfile配置选择加载路径，生成UI面板对象（面板名称生成）
        public static BasePanel CreateBasePanel(string panelName)
        {
            return CreateBasePanel(CreatePanelProfile(panelName));
        }

        //异步生成UI面板对象（仅适用于预加载时）
        #endregion

        #region 生产GameObject

        //同步生成UI游戏对象
        public static GameObject CreateUIGo(PanelProfile profile)
        {
            if (profile == null)
            {
                CTLogger.Error("错误！没有找到PanelProfile");
                return null;
            }
            //获取当前场景的Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                CTLogger.Error("错误！没有找到Canvas", SysEnum.UISystem);
                return null;
            }
            //根据配置文件生成
            GameObject template;
            if (profile.isAssetBundle)
            {
                //AB包加载
                template = _CT.ResMgr.LoadRes<GameObject>(profile.abName, profile.panelName);
            }
            else
            {
                //Resources加载
                template = Resources.Load<GameObject>(profile.GetResourcesPath());
            }
            if (template != null)
            {
                return GameObject.Instantiate(template, canvas.transform);
            }
            else
            {
                CTLogger.Log("加载UI游戏对象失败！找不到相关资源", SysEnum.UISystem);
                return null;
            }
        }

        //同步生成UI游戏对象（仅Resources）
        //参数①：完整的资源路径
        public static GameObject CreateUIGo(string path)
        {
            //获取当前场景的Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                CTLogger.Error("错误！没有找到Canvas", SysEnum.UISystem);
                return null;
            }
            //根据配置文件生成
            GameObject template;
            //Resources加载
            template = Resources.Load<GameObject>(path);
            if (template != null)
            {
                return GameObject.Instantiate(template, canvas.transform);
            }
            else
            {
                CTLogger.Log("加载UI游戏对象失败！找不到相关资源", SysEnum.UISystem);
                return null;
            }
        }

        //异步生成UI游戏对象
        public static void CreateUIGoAsync(PanelProfile profile, UnityAction<GameObject> callback)
        {
            if (profile == null)
            {
                CTLogger.Error("错误！没有找到PanelProfile");
                return;
            }
            //获取当前场景的Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                CTLogger.Error("错误！没有找到Canvas");
                return;
            }
            //根据配置文件生成
            if (profile.isAssetBundle)
            {
                //AB包加载
                _CT.ResMgr.LoadResAsync(profile.abName, profile.panelName, callback);
            }
            else
            {
                //Resources加载
                _CT.ResMgr.LoadResAsync(profile.GetResourcesPath(), callback);
            }
        }

        #endregion

        #region 缓存管理

        //出栈时调用，缓存该面板且控制面板的释放
        public static void CacheAndDestroyUl(BasePanel panel)
        {
            if (panel == null)
            {
                CTLogger.Log("BasePanel不存在，无法缓存", SysEnum.UISystem);
                return;
            }
            //如果面板不使用缓存技术，就直接释放
            if (!panel.isCache)
            {
                ReleaseTool.TryRelease(panel);
                return;
            }
            //如果缓存，执行缓存策略
            //单缓存技术
            if (uiProfile.isSingleCache)
            {
                //如果之前有其他缓存，就清理掉
                if (cache != null)
                {
                    ReleaseTool.TryRelease(panel); //销毁掉面板
                }
                cache = panel; //缓存该面板
            }
            else //缓存池
            {
                //缓存池塞得下，就直接缓存即可
                if (cachePool.Count < uiProfile.maxCache)
                {
                    cachePool.Add(panel);
                }
                else
                {
                    //释放掉最早缓存的面板
                    BasePanel first = cachePool[0];
                    ReleaseTool.TryRelease(first);
                    cachePool.RemoveAt(0);
                    //缓存当前面板
                    cachePool.Add(panel);
                }
            }
        }

        //销毁指定缓存的面板
        public static void ClearCache(string panelName)
        {
            if (uiProfile.isSingleCache)
            {
                if (cache != null && cache.panelName.Equals(panelName))
                {
                    ReleaseTool.TryRelease(cache);
                    cache = null;
                }
            }
            else
            {
                foreach(var item in cachePool)
                {
                    if (item.panelName.Equals(panelName))
                    {
                        ReleaseTool.TryRelease(item);
                        cachePool.Remove(item);
                        return;
                    }
                }
            }
        }

        //清空所有缓存对象，场景切换时调用
        public static void ClearAllCache()
        {
            //单缓存技术
            if (uiProfile.isSingleCache)
            {
                ReleaseTool.TryRelease(cache);
                cache = null;
            }
            else //缓存池技术
            {
                foreach (BasePanel item in cachePool)
                {
                    ReleaseTool.TryRelease(item);
                }
                cachePool.Clear();
            }
        }

        #endregion
    }
}