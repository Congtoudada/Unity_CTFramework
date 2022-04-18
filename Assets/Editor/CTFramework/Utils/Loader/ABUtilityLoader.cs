/****************************************************
  文件：ABUtilityLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using CT.ResSys;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace CT.Editor
{
    public static class ABUtilityLoader
    {
        //生成资源清单，用于进行AssetBundle的校验
        //返回成功与否，不成功打印原因
        public static bool CreateVersionList_Loader(this ABUtilityEditor self)
        {
            //AB包配置文件
            //根据默认配置文件获取资源清单名称
            ResourceProfile profile = CTFactory.GetFinalProfile(ProfileEnum.Res) as ResourceProfile;
            if (profile != null)
            {
                //资源准备
                //新建资源清单，存储所有列表项
                ABLoader.VersionList versionList = new ABLoader.VersionList();
                DirectoryInfo dir = new DirectoryInfo(self.sourcePath);
                FileInfo[] files = dir.GetFiles();
                MD5 md5 = MD5.Create();

                //遍历该目录下所有文件，生成清单数据
                foreach (FileInfo file in files)
                {
                    if (file.Name.Equals(profile.abVersionList)) continue; //忽略Versionlist本身
                                                                           //Debug.Log(file.Name);
                                                                           //新建列表项（由文件名 + MD5码构成）
                    ABLoader.VersionItem item = new ABLoader.VersionItem();
                    item.fileName = file.Name;

                    string path = Path.Combine(self.sourcePath, file.Name);
                    string fileMd5 = string.Empty;
                    //读取文件并计算其MD5
                    using (FileStream fs = File.OpenRead(path))
                    {
                        byte[] fileMd5Bytes = md5.ComputeHash(fs);
                        fileMd5 = System.BitConverter.ToString(fileMd5Bytes).Replace("-", "").ToLower();
                    }
                    item.md5 = fileMd5; //更新md5
                    versionList.updateList.Add(item); //存入资源清单
                }
                //将资源清单转化为Json存储
                string json = JsonUtility.ToJson(versionList, true);

                //写到目标路径
                if (self.isSamePath)
                    self.outputPath = self.sourcePath;
                using (StreamWriter sw = new StreamWriter(Path.Combine(self.outputPath, profile.abVersionList)))
                {
                    sw.Write(json);
                }
                Debug.Log("versionList生成成功: " + self.outputPath);
                return true;
            }
            else
            {
                Debug.LogWarning("没有找到配置文件，请检查全局配置文件: " + CTConstant.DEFAULT_CONFIG_PATH);
                return false;
            }
        }
    }
}