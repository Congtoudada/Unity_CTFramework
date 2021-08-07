/****************************************************
  文件：SerializableDictionary.cs
  作者：聪头
  邮箱：1322080797@qq.com
  日期：2021/7/31 20:40:22
  功能：Nothing
*****************************************************/
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CT.Tools
{
    [Serializable]
    public class SerializableDictionary<K, V> : ISerializationCallbackReceiver
    {
        [HideInInspector]
        [SerializeField]
        private List<K> m_keys;
        [HideInInspector]
        [SerializeField]
        private List<V> m_values;
        
        [ShowInInspector]
        public Dictionary<K, V> m_Dictionary = new Dictionary<K, V>();

        [HideInInspector]
        [SerializeField]
        public List<K> Keys = new List<K>();
        [HideInInspector]
        [SerializeField]
        public List<V> Values = new List<V>();

        public int Count { get => m_Dictionary.Count; }

        public V this[K key]
        {
            get
            {
                if (!m_Dictionary.ContainsKey(key))
                    return default(V);
                return m_Dictionary[key];
            }
            set
            {
                if (!m_Dictionary.ContainsKey(key))
                    m_Dictionary.Add(key, value);
                else
                    m_Dictionary[key] = value;
            }
        }

        public void Add(K key, V value)
        {
            m_Dictionary.Add(key, value);
            Keys.Add(key);
            Values.Add(value);
        }

        public void Remove(K k)
        {
            if (m_Dictionary.ContainsKey(k))
            {
                Keys.Remove(k);
                Values.Remove(m_Dictionary[k]);
                m_Dictionary.Remove(k);
            }
                
        }

        public bool ContainsKey(K k)
        {
            return m_Dictionary.ContainsKey(k);
        }

        public void Clear()
        {
            m_Dictionary.Clear();
        }

        public bool TryGetValue(K k, out V v)
        {
            return m_Dictionary.TryGetValue(k, out v);
        }

        public void Write(string path)
        {
            if(!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            string json = JsonUtility.ToJson(this, true);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }

        public SerializableDictionary<K, V> Read(string path)
        {
            //Debug.Log(path);
            if(File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string json = sr.ReadToEnd();
                    return JsonUtility.FromJson<SerializableDictionary<K, V>>(json);
                }
            }
            else
            {
                DebugMgr.Log("无法读取序列化字典，请检查路径: " + path, SystemEnum.Tools);
                return null;
            }
        }

        public void OnAfterDeserialize()
        {
            int length = m_keys.Count;
            m_Dictionary = new Dictionary<K, V>();
            for(int i = 0; i < length; i++)
            {
                m_Dictionary[m_keys[i]] = m_values[i];
            }
            m_keys = null;
            m_values = null;
        }

        public void OnBeforeSerialize()
        {
            m_keys = new List<K>();
            m_values = new List<V>();

            foreach(var item in m_Dictionary)
            {
                m_keys.Add(item.Key);
                m_values.Add(item.Value);
            }
        }
    }
}
