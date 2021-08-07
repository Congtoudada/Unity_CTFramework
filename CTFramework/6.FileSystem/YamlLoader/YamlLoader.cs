/****************************************************
  文件：YamlLoader.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/29 12:54:08
  功能：操作Yaml文件，需要引入YamlDotNet插件
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CT.FileSys
{
    public class YamlLoader : IFileLoader, IRelease
    {
        private IDeserializer _deserializer;
        private ISerializer _serializer;
      
        private IDeserializer deserializer
        {
            get
            {
                if (_deserializer == null)
                {
                    _deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new CamelCaseNamingConvention()) //驼峰命名匹配
                    .IgnoreUnmatchedProperties()    //忽略不匹配属性
                    .Build();
                }
                return _deserializer;
            }
        }
        private ISerializer serializer
        {
            get
            {
                if (_serializer == null)
                    _serializer = new SerializerBuilder().Build();
                return _serializer;
            }
        }

        #region 实现接口
        //将字符串转换为对象
        public T GetObj<T>(string content)
        {
            return deserializer.Deserialize<T>(content);
        }

        //将对象转字符串
        public string GetString<T>(T obj)
        {
            string yaml = serializer.Serialize(obj);
            return yaml;
        }

        public void Release()
        {
            _deserializer = null;
            _serializer = null;
        }
        #endregion
    }
}
