namespace BoBo.Light.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using UnityEngine;
    using System.Runtime.InteropServices;
    using System.Text;


    public class XmlHelper : IDisposable
    {
        public XmlHelper()
        {
            m_doc = new XmlDocument();
        }
        public XmlHelper(string path)
        {
            m_doc = new XmlDocument();
            m_doc.Load(path);
        }
        public XmlNode FindNode(XmlNode node, string xpath)
        {
            return node.SelectSingleNode(xpath);
        }
        public XmlNode FindNode(string xpath)
        {
            return m_doc.SelectSingleNode(xpath);
        }

        public XmlNodeList FindNodes(string xpath)
        {
            return m_doc.SelectNodes(xpath);

        }

        public XmlNodeList FindNodes(XmlNode node, string xpath)
        {
            return node.SelectNodes(xpath);
        }

        public XmlNode FindChildNode(XmlNode parent, string name)
        {
            if (parent.HasChildNodes)
            {
                foreach (XmlNode n in parent.ChildNodes)
                {
                    if (n.Name == name)
                        return n;
                }
                return null;
            }
            else
                return null;
        }


        public int ReadAttributeInt(XmlNode node, string name)
        {
            return Convert.ToInt32(((XmlElement)node).GetAttribute(name));
        }

        public float ReadAttributeFloat(XmlNode node, string name)
        {
            return (float)Convert.ToDouble(((XmlElement)node).GetAttribute(name));
        }
        public double ReadAttributeDouble(XmlNode node, string name)
        {

            return Convert.ToDouble(((XmlElement)node).GetAttribute(name));
        }

        public string ReadAttributeString(XmlNode node, string name)
        {
            return ((XmlElement)(node)).GetAttribute(name);
        }


        public bool ReadNodeBooleanMatchString(XmlNode node, string matchStr)
        {
            if (matchStr.Equals(ReadNodeString(node)))
                return true;

            return false;

        }

        public bool ReadNodeBoolean(XmlNode node)
        {
            string str = ReadNodeString(node).ToLower();
            if ("true".Equals(str))
                return true;
            else
                return false;
        }

        public int ReadNodeInt(XmlNode node)
        {

            return Convert.ToInt32(node.InnerText);
        }



        public float ReadNodeFloat(XmlNode node)
        {
            return (float)Convert.ToDouble(node.InnerText);
        }


        public string ReadNodeString(XmlNode node, string xpath)
        {

            return ReadNodeString(FindNode(node, xpath));
        }

        public string ReadNodeString(XmlNode node)
        {
            return node.InnerText;
        }

        public Vector4 ReadNodeVector4(XmlNode node)
        {
            string[] temp = node.InnerText.Split(new char[] { ',' });
            return new Vector4(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3]));
        }


        public Vector3 ReadNodeVector3(XmlNode parent, string xpath)
        {
            return ReadNodeVector3(FindNode(parent, xpath));
        }

        public Vector3 ReadNodeVector3(XmlNode node)
        {
            string[] temp = node.InnerText.Split(new char[] { ',' });
            return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
        }

        public Vector2 ReadNodeVector2(XmlNode node)
        {
            string[] temp = node.InnerText.Split(new char[] { ',' });
            return new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
        }

        public XmlNode CreateChildNode(XmlNode parent, string name, string[] attibutes, string[] values)
        {
            XmlElement newNode = m_doc.CreateElement(name);
            for (int i = 0; i < attibutes.Length; ++i)
            {
                newNode.SetAttribute(attibutes[i], values[i]);
            }
            parent.AppendChild(newNode);
            return newNode;
        }
        public XmlNode CreateChildNode(XmlNode parent, string name, string value)
        {
            XmlElement newNode = m_doc.CreateElement(name);
            newNode.InnerText = value;
            parent.AppendChild(newNode);
            return newNode;
        }

        public XmlNode CreateChildNode(XmlNode parent, string name)
        {
            XmlElement newNode = m_doc.CreateElement(name);
            parent.AppendChild(newNode);
            return newNode;

        }

        public XmlElement GetRoot()
        {

            return m_doc.DocumentElement;
        }
        public void Save(string filename)
        {

            m_doc.Save(filename);
        }

        public void Load(string path)
        {
            m_doc.Load(path);
        }

        public void SetNodeAttribute(XmlNode node, string attributeName, string value)
        {
            XmlElement xe = (XmlElement)node;
            xe.SetAttribute(attributeName, value);
        }
        public void SetNodeValue(XmlNode node, string value)
        {
            node.InnerText = value;


        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool isDisposing)
        {
            if (!m_isDisposed)
            {
                if (isDisposing)
                {
                    //释放托管资源
                    m_doc = null;
                    GC.Collect();
                }

                //释放非托管资源
                m_isDisposed = true;
            }
        }


        ~XmlHelper()
        {
            Dispose(false);

        }

        private XmlDocument m_doc;
        protected bool m_isDisposed;
    }

    public static class FileFind
    {
        /// <summary>
        /// 从根目录开始查找指定文件名(含后缀)的文件，并返回其全路径
        /// </summary>
        public static string GetFileFullName(string rootDir, string fileName)
        {
            string fullName = string.Empty;
            DirectoryInfo root = new DirectoryInfo(rootDir);

            FileInfo[] childFInfos = root.GetFiles();
            foreach (FileInfo item in childFInfos)
            {
                if (item.Name.Equals(fileName))
                    return item.FullName;
            }

            DirectoryInfo[] childDirs = root.GetDirectories();
            foreach (DirectoryInfo item in childDirs)
            {
                fullName = GetFileFullName(item.FullName, fileName);
                if (!string.IsNullOrEmpty(fullName))
                    return fullName;
            }

            return fullName;
        }
        /// <summary>
        /// 从根目录开始查找指定文件夹，并返回其全路径
        /// </summary>
        public static string GetDirectoryFullName(string rootDir, string dirName)
        {
            string fullName = string.Empty;
            DirectoryInfo[] childDirs = new DirectoryInfo(rootDir).GetDirectories();
            foreach (DirectoryInfo item in childDirs)
            {
                if (item.Name.Equals(dirName))
                    return item.FullName;

                fullName = GetDirectoryFullName(item.FullName, dirName);
                if (!string.IsNullOrEmpty(fullName))
                    return fullName;
            }

            return fullName;
        }
    }

    public class IniFileHelper
    {
        private string m_fileName;

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName
            );

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">Ini文件路径</param>
        public IniFileHelper(string fileName)
        {
            this.m_fileName = fileName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IniFileHelper()
        { }

        /// <summary>
        /// [扩展]读Int数值
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public int ReadInt(string section, string name, int def)
        {
            return GetPrivateProfileInt(section, name, def, this.m_fileName);
        }


        public bool ReadBoolean(string section, string name)
        {
            string result = ReadString(section, name, "false");
            result = result.ToLower();
            if ("false".Equals(result))
                return false;
            else
                return true;
        }

        /// <summary>
        /// [扩展]读取string字符串
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public string ReadString(string section, string name, string def)
        {
            StringBuilder vRetSb = new StringBuilder(2048);
            GetPrivateProfileString(section, name, def, vRetSb, 2048, this.m_fileName);
            return vRetSb.ToString();
        }

        /// <summary>
        /// [扩展]写入Int数值，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="Ival">写入值</param>
        public void WriteInt(string section, string name, int Ival)
        {

            WritePrivateProfileString(section, name, Ival.ToString(), this.m_fileName);
        }

        /// <summary>
        /// [扩展]写入String字符串，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="strVal">写入值</param>
        public void WriteString(string section, string name, string strVal)
        {
            WritePrivateProfileString(section, name, strVal, this.m_fileName);
        }

        /// <summary>
        /// 删除指定的 节
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section)
        {
            WritePrivateProfileString(section, null, null, this.m_fileName);
        }

        /// <summary>
        /// 删除全部 节
        /// </summary>
        public void DeleteAllSection()
        {
            WritePrivateProfileString(null, null, null, this.m_fileName);
        }

        /// <summary>
        /// 读取指定 节-键 的值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string IniReadValue(string section, string name)
        {
            StringBuilder strSb = new StringBuilder(256);
            GetPrivateProfileString(section, name, "", strSb, 256, this.m_fileName);
            return strSb.ToString();
        }

        /// <summary>
        /// 写入指定值，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void IniWriteValue(string section, string name, string value)
        {
            WritePrivateProfileString(section, name, value, this.m_fileName);
        }
    }
}
