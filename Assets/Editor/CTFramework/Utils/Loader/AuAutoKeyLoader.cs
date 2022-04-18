/****************************************************
  文件：AuAutoKeyEditorLoader.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：DateTime
  功能：
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CT.Editor
{
    public static class AuAutoKeyLoader
    {
        //一键生成所有Key
        //返回成功与否，不成功打印原因
        public static bool CreateScript_Loader(this AuAutoKeyEditor self)
        {
            string tp = CTEditorConstant.EDITOR_TEMPLATE_AUKEY; //AuKeyTemplate的路径
            string fileName = self.profile.name.Replace("Profile", "");
            string file = FileTool.ReadTextFromDisk(tp); //读取模板文件
            file = file.Replace("#Content#", self.ReplaceString(file, fileName));
            FileTool.WriteTextToDisk(Path.Combine(self.output, fileName + "KeyEnum.cs"), file); //输出到目标路径
            return true;
        }

        private static string ReplaceString(this AuAutoKeyEditor self, string file, string filename)
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            sb.Append("\t//背景音乐的Key").AppendLine();
            if (self.profile.bgs != null)
            {
                string filename_suffix = filename.ToUpper();
                if (filename.Contains("AuGroup"))
                    filename_suffix = filename.ToUpper().Substring(7); //如果有前缀就去前缀
                foreach (var item in self.profile.bgs)
                {
                    sb.Append("\tpublic const string ")
                        .Append(filename_suffix)
                        .Append("_")
                        .Append(item.clipKey.ToUpper())
                        .Append("_BG")
                        .Append(" = \"")
                        .Append(item.clipKey)
                        .Append("\";")
                        .AppendLine();
                }
            }
            sb.Append("\t//音效的Key").AppendLine();
            if (self.profile.effects != null)
            {
                foreach (var item in self.profile.effects)
                {
                    sb.Append("\tpublic const string ")
                        .Append(filename.ToUpper())
                        .Append("_")
                        .Append(item.clipKey.ToUpper())
                        .Append("_EF")
                        .Append(" = \"")
                        .Append(item.clipKey)
                        .Append("\";")
                        .AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}