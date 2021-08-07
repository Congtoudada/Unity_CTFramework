/****************************************************
  文件：EditorDict.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/8/1 13:45:46
  功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Tools
{
    public class EditorDict
    {
        public SerializableDictionary<string, string> editorDict;

        public EditorDict()
        {
            editorDict = new SerializableDictionary<string, string>();
        }

        public void ReadData()
        {
            editorDict = editorDict.Read(CTConstant.EDITOR_CACHE_PATH);
            if(editorDict == null)
            {
                editorDict = new SerializableDictionary<string, string>();
                editorDict["version"] = "v0.1";
            }
        }

        public void WriteData()
        {
            editorDict.Write(CTConstant.EDITOR_CACHE_PATH);
        }
    }
}
