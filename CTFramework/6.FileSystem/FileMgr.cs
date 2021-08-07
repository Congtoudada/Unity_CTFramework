/****************************************************
  文件：FileMgr.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/29 12:15:31
  功能：将对象和文件互相转换，加载器同时仅能存在一个
*****************************************************/
using CT.Tools;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace CT.FileSys
{
    public class FileMgr : IRelease
    {
        private IFileLoader _fileLoader;
        private FileTool _fileTool;

        private IFileLoader fileLoader
        {
            set {
                _fileLoader = value;
            }
            get
            {
                if (_fileLoader == null) 
                {
                    DebugMgr.Warning("没有加载fileLoader", SystemEnum.FileSystem);
                    return null;
                }
                return _fileLoader;
            }
        }
        private FileTool fileTool
        {
            set { _fileTool = value;  }
            get
            {
                if (_fileTool == null) {
                    _fileTool = new FileTool();
                }
                return _fileTool;
            }
        }

        #region 构建器

        //设置自定义加载器
        public FileMgr WithFileLoader(IFileLoader fileLoader)
        {
            ReleaseTool.TryRelease(fileLoader);
            this.fileLoader = fileLoader;
            return this;
        }

        //内置加载器
        public FileMgr WithYamlLoader()
        {
            if (!(_fileLoader is YamlLoader))
            {
                ReleaseTool.TryRelease(_fileLoader);
                _fileLoader = new YamlLoader();
            }
            return this;
        }

        public FileMgr WithJsonLoader()
        {
            if(!(_fileLoader is JsonLoader))
            {
                ReleaseTool.TryRelease(_fileLoader);
                _fileLoader = new JsonLoader();
            }   
            return this;
        }

        #endregion

        ///提供用户调用的方法

        #region 文本与对象间转换
        //根据字符串返回对象
        public T GetObj<T>(string content)
        {
            if (fileLoader != null)
                return fileLoader.GetObj<T>(content);
            else
                return default(T);
        }
        //根据对象返回字符串
        public string GetString<T>(T obj)
        {
            return fileLoader?.GetString<T>(obj);
        }
        #endregion

        #region 文本读写
        //从本地磁盘读取文本文件
        public string ReadString(string path)
        {
            return FileTool.ReadString(path);
        }

        //从网络读取文本文件
        public IEnumerator ReadStringURI(string uri, UnityAction<string> callback)
        {
            return fileTool?.ReadStringURI(uri, callback);
        }

        //读取文本文件转对象
        public T ReadObj<T>(string path)
        {
            if(fileLoader != null)
            {
                if (fileTool != null)
                    return fileTool.ReadObj<T>(path, fileLoader);
            }
            return default(T);
        }

        //写文本文件（字符串 -> 硬盘）
        public void WriteString(string path, string content)
        {
            FileTool.WriteString(path, content);
        }

        //写对象（对象 -> 字符串 -> 硬盘）
        public void WriteObj<T>(string path, T obj)
        {
            if(fileLoader != null)
            {
                fileTool?.WriteObj<T>(path, obj, fileLoader);
            }
        }

        #endregion

        #region 字节数组读写
        //读字节（硬盘 -> 字节）
        public byte[] ReadBytes(string path)
        {
            return FileTool.ReadBytes(path);
        }
        //写字节（字节 -> 硬盘）
        public void WriteBytes(string dirPath, string fileName, byte[] bytes)
        {
            FileTool.WriteBytes(dirPath, fileName, bytes);
        }
        public void WriteBytes(string path, byte[] bytes)
        {
            FileTool.WriteBytes(path, bytes);
        }
        #endregion

        #region 字节与字符转换
        //字节数组 -> 字符串
        public string GetString(byte[] bytes)
        {
            return FileTool.GetString(bytes);
        }
        //字符串 -> 字节数组
        public byte[] GetBytes(string str)
        {
            return FileTool.GetBytes(str);
        }
        #endregion

        public void Release()
        {
            ReleaseTool.TryRelease(fileLoader);
            fileLoader = null;

            ReleaseTool.TryRelease(fileTool);
            fileTool = null;
        }
    }
}
