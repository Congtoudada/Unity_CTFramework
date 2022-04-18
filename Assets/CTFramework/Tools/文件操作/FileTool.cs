/****************************************************
  文件：FileTool.cs
  作者：聪头
  邮箱:  1322080797@qq.com
  日期：2021/11/14 16:29:07
  功能：文件操作工具类
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace CT.Tools
{
    public class FileTool 
    {
        //#region 基于Yaml
        ////Yaml
        //private static IDeserializer _deserializer = null;
        //private static IDeserializer deserializer
        //{
        //    get
        //    {
        //        if (_deserializer == null)
        //        {
        //            _deserializer = new DeserializerBuilder()
        //            .IgnoreUnmatchedProperties()    //忽略不匹配属性
        //            .Build();
        //        }
        //        return _deserializer;
        //    }
        //}

        //private static ISerializer _serializer = null;
        //private static ISerializer serializer
        //{
        //    get
        //    {
        //        if (_serializer == null)
        //        {
        //            _serializer = new SerializerBuilder().Build();
        //        }
        //        return _serializer;
        //    }
        //}

        ////将Yaml字符串转换为对象
        ////①：Yaml字符串
        ////②：是否销毁转换器
        //public static T YamlToObject<T>(string content, bool isCache = false)
        //{
        //    T obj = deserializer.Deserialize<T>(content);
        //    if (!isCache) _deserializer = null;
        //    return obj;
        //}

        ////将对象转Yaml字符串
        ////①：对象
        ////②：是否销毁转换器
        //public static string ObjectToYaml<T>(T obj, bool isCache = false)
        //{
        //    string yaml = serializer.Serialize(obj);
        //    if (!isCache) _serializer = null;
        //    return yaml;
        //}
        //#endregion

        #region 基于文本
        //本地读取文本文件
        public static string ReadTextFromDisk(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                CTLogger.Log("无法读取文本文件，请检查路径: " + path, SysEnum.Tools);
                return "";
            }
        }

        //利用协程从网络读取文本文件
        public IEnumerator ReadTextFromURI(string uri, UnityAction<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                callback(request.downloadHandler.text);
            }
        }

        //写文本文件（文本 -> 硬盘）
        public static void WriteTextToDisk(string path, string content)
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
        #endregion

        #region 基于字节
        //参考链接：https://www.cnblogs.com/vaevvaev/p/6804852.html
        //读字节（硬盘 -> 字节）
        public static byte[] ReadBytesFromDisk(string path)
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
        public static void WriteBytesToDisk(string dirPath, string fileName, byte[] bytes)
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
            CTLogger.Log("文件写入成功: " + filePath, SysEnum.Tools);

            fs.Flush();     //文件写入存储到硬盘
            fs.Close();     //关闭文件流对象
            fs.Dispose();   //销毁文件对象
        }

        public static void WriteBytesToDisk(string path, byte[] bytes)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            string dir = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path);
            WriteBytesToDisk(dir, file, bytes);
        }
        #endregion

        #region 字节与字符转换
        //字节数组 -> 字符串
        public static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes, 0, bytes.Length);
        }
        //字符串 -> 字节数组
        public static byte[] StringToBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        #endregion
    }
}