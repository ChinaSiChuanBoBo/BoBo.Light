namespace BoBo.Light.Utility
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Serialization;
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;
    
    public sealed class XMLSerializeTool
    {
        /// <summary>
        /// 按指定编码序列化
        /// </summary>
        public static void Serialize<T>(string path, string encoding, System.Object obj)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.GetEncoding(encoding));
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(writer, obj);
                writer.Close();
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        public static T Deserialize<T>(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(fileStream);
            }
        }
    }

    public sealed class BinarySerializeTool
    {
        public static void Serialize(string path, System.Object obj)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, obj);
            }
        }

        public static T Deserialize<T>(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(fileStream);
            }
        }

    }

    public sealed class JsonSerializeTool
    {
        [Serializable]
        public class ListSerialization<T>
        {
            [SerializeField]
            List<T> target;

            public List<T> ToList() { return target; }

            public ListSerialization(List<T> target)
            {
                this.target = target;
            }
        }

        [Serializable]
        public class DictionarySerialization<TKey, TValue> : ISerializationCallbackReceiver
        {
            [SerializeField]
            List<TKey> keys;
            [SerializeField]
            List<TValue> values;

            Dictionary<TKey, TValue> target;
            public Dictionary<TKey, TValue> ToDictionary() { return target; }

            public DictionarySerialization(Dictionary<TKey, TValue> target)
            {
                this.target = target;
            }

            public void OnBeforeSerialize()
            {
                keys = new List<TKey>(target.Keys);
                values = new List<TValue>(target.Values);
            }

            public void OnAfterDeserialize()
            {
                var count = Math.Min(keys.Count, values.Count);
                target = new Dictionary<TKey, TValue>(count);
                for (var i = 0; i < count; ++i)
                {
                    target.Add(keys[i], values[i]);
                }
            }
        }

        public static void Serialize(string path, System.Object obj)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fileStream);
                writer.Write(JsonUtility.ToJson(obj));
                writer.Flush();
            }
        }

        public static T Deserialize<T>(string path)
        {
            T obj;
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(fileStream);
                obj = JsonUtility.FromJson<T>(reader.ReadToEnd());
            }
            return obj;
        }
    }

    
}