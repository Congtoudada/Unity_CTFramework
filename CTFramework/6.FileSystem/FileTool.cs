/****************************************************
  文件：FileTool.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/30 15:23:27
  功能：Nothing
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace CT.FileSys
{
    public class FileTool
    {
        #region 字符流
        //读取文本文件
        public static string ReadString(string path)
        {
            if(File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                DebugMgr.Log("无法读取文本文件，请检查路径: " + path, SystemEnum.FileSystem);
                return "";
            }
        }

        //从网络读取文本文件
        public IEnumerator ReadStringURI(string uri, UnityAction<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();
            if(request.isDone)
            {
                callback(request.downloadHandler.text);
            }
        }

        //读取文本文件转对象
        public T ReadObj<T>(string path, IFileLoader fileLoader)
        {
            string content = ReadString(path);
            return fileLoader.GetObj<T>(content);
        }

        //写文本文件（字符串 -> 硬盘）
        public static void WriteString(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }

        //写对象（对象 -> 字符串 -> 硬盘）
        public void WriteObj<T>(string path, T obj, IFileLoader fileLoader)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            string content = fileLoader.GetString<T>(obj);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }

        #endregion

        #region 字节流
        //参考链接：https://www.cnblogs.com/vaevvaev/p/6804852.html
        //读字节（硬盘 -> 字节）
        public static byte[] ReadBytes(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                int len = (int)fs.Length;
                byte[] bytes = new byte[len];
                int r = fs.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        //写字节（字节 -> 硬盘）
        public static void WriteBytes(string dirPath, string fileName, byte[] bytes)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = Path.Combine(dirPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fs = File.Create(filePath, 1024);
            fs.Write(bytes, 0, bytes.Length);
            DebugMgr.Log("文件写入成功: " + filePath, SystemEnum.FileSystem);

            fs.Flush();     //文件写入存储到硬盘
            fs.Close();     //关闭文件流对象
            fs.Dispose();   //销毁文件对象
        }
        
        public static void WriteBytes(string path, byte[] bytes)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            string dir = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path);
            WriteBytes(dir, file, bytes);
        }
        #endregion

        #region 字节与字符转换
        //字节数组 -> 字符串
        public static string GetString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes, 0, bytes.Length);
        }
        //字符串 -> 字节数组
        public static byte[] GetBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        #endregion
    }
}
