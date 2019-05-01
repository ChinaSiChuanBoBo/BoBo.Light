namespace BoBo.Light.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    public enum State
    {
        Debug = 1,               //实时输出所有级别日志内容
        Deploy,                  //只缓存输出Error、Fatal级别日志
        Off                      //不输出任何日志信息
    };

    public interface ILog
    {
        void Warn(string content);

        void Info(string content);

        void Error(string content);

        void Fatal(string content);

        void Close();
    }

    static class LogTips
    {
        public const string Warning = "Warning------";
        public const string Import = "@Important !!!------";
        public const string Error = "Error!!!------";
        public const string Info = "Info------";
    }

    public static class SimpleLog
    {
        class LogEntity : ILog
        {
            public void Close()
            {
                if (State.Off <= SimpleLog.m_logState)
                {                   
                    return;
                }
                Fatal("日志结束");
                SyncLogCatchToFile();            
                try
                {
                    lock (m_lock)
                    {
                        RemoveLogEntity(this.Key);
                        if (null != m_fileStream)
                        {
                            m_fileStream.Flush();
                            m_fileStream.Dispose();
                            m_fileStream.Close();
                            m_fileStream = null;
                            m_contentBuffer.Clear();
                            m_contentBuffer = null;
                        }
                    }
                }
                catch
                {

                }
            }

            //只在Debug级别输出
            public void Warn(string content)
            {
                if (State.Off <= SimpleLog.m_logState)
                    return;

                if (State.Debug >= SimpleLog.m_logState)
                    WriteContent(LogTips.Warning + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");             
            }
            //只在Debug级别输出
            public void Info(string content)
            {
                if (State.Off <= SimpleLog.m_logState)
                    return;

                if (State.Debug >= SimpleLog.m_logState)
                    WriteContent(LogTips.Info + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");
                
            }
            //Off级别以下都输出
            public void Error(string content)
            {
                if (State.Off <= SimpleLog.m_logState)
                    return;

                if (State.Debug < SimpleLog.m_logState)
                    WriteBuffer(LogTips.Error + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");
                else
                    WriteContent(LogTips.Error + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");
            }
            //Off级别以下都输出
            public void Fatal(string content)
            {
                if (State.Off <= SimpleLog.m_logState)
                    return;

                if (State.Debug < SimpleLog.m_logState)
                    WriteBuffer(LogTips.Import + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");
                else
                    WriteContent(LogTips.Import + content + "------" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒")+"\r\n");
            }

            public string FullName
            {
                get;
                protected set;
            }

            public string Key
            {
                get;
                protected set;
            }

            public LogEntity(string fullName, System.Text.Encoding encode, int capacity, string key)
            {
                if (State.Off <= SimpleLog.m_logState)
                    return;

                m_maxCapacity = capacity;
                this.FullName = fullName;
                m_fileStream = new FileStream(fullName, FileMode.Create);
                m_encoding = encode;
                m_contentBuffer = new List<string>();
                m_lock = new object();
                this.Key = key;
                Fatal("日志开始");
            }
            //写入底层实现
            protected void WriteBytes(byte[] bytes)
            {
                try
                {
                    m_curFlows += bytes.Length;          
                    if (m_curFlows > m_maxCapacity)
                    {
                        lock (m_lock)
                        {
                            if (m_curFlows > m_maxCapacity)
                            {
                                m_fileStream.Flush();
                                m_fileStream.Dispose();
                                m_fileStream.Close();
                                m_fileStream = null;
                                m_curFlows -= m_maxCapacity;
                                //
                                m_fileStream = new FileStream(FullName + "+"+m_newSuffixes++, FileMode.Create);
                            }
                            else
                            {
                                m_curFlows += bytes.Length;
                            }
                        }
                    }
                    m_fileStream.Write(bytes, 0, bytes.Length);
                    m_fileStream.Flush();
                }
                catch
                {

                }
            }

            protected void WriteContent(string content)
            {            
                byte[] bytes = m_encoding.GetBytes(content);
                WriteBytes(bytes);
            }

            protected void WriteBuffer(string content)
            {
                m_contentBuffer.Add(content);
            }
            /// <summary>
            /// 同步缓存数据信息到实体文件中
            /// </summary>
            protected void SyncLogCatchToFile()
            {
                if (m_contentBuffer.Count > 0)
                {
                    foreach (string content in m_contentBuffer)
                        WriteContent(content);
                    m_contentBuffer.Clear();
                }
            }

            private FileStream m_fileStream;

            private Encoding m_encoding;

            private List<string> m_contentBuffer;

            private int m_maxCapacity = 1024 * 1024;  //默认1MB

            private int m_curFlows = 0;

            private int m_newSuffixes = 2;

            private readonly object m_lock;
        }

        public static string LogState                  //日志部署模式字符描述
        {
            get
            {
                switch (m_logState)
                {
                    case State.Debug: return "debug";
                    case State.Off: return "off";
                    case State.Deploy: return "deploy";
                    default: return "off";
                }
            }

            private set
            {
                switch (value)
                {
                    case "debug": m_logState = State.Debug; break;
                    case "off": m_logState = State.Off; break;
                    case "deploy": m_logState = State.Deploy; break;
                    default: m_logState = State.Off; break;
                }

            }
        }
        private static State m_logState = State.Off;

        public static string LogCoding
        {
            get
            {
                return m_logCoding;
            }

            private set
            {
                switch (value)
                {
                    case "utf-8": m_logCoding = "utf-8"; break;
                    case "ascii": m_logCoding = "ascii"; break;
                    default: m_logCoding = "utf-8"; break;
                }
            }
        }
        private static string m_logCoding = "";

        public static bool IsDated
        {
            get
            {
                return m_isDated;
            }
        }
        private static bool m_isDated;

        public static string FolderPath
        {
            get
            {
                return m_folderPath;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                    m_folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                else
                    m_folderPath = value;
            }
        }
        private static string m_folderPath = "";

        public static void InitLog(string logConfig)
        {         
            m_logEntitys = new Dictionary<string, ILog>();
            //读取配置
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(logConfig);
            XmlElement root = xDoc.DocumentElement;           
            LogState = root.SelectSingleNode("State").InnerText.ToLower();
            LogCoding = root.SelectSingleNode("Coding").InnerText.ToLower();
            FolderPath = root.SelectSingleNode("RootFolder").InnerText;
            if("default"==FolderPath)
                FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //获得日志根文件夹
            if ("true" == root.SelectSingleNode("Dated").InnerText.ToLower())
            {
                m_isDated = true;
                FolderPath += "/Log/" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
            else
            {
                m_isDated = false;
                FolderPath += "/Log";
            }

            if (State.Off > m_logState)
            {
                try
                {
                   
                    Directory.CreateDirectory(FolderPath);
                }
                catch   //如果在配置中指定的目录没法创建，那么默认在桌面创建
                {
                    FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (m_isDated)
                        FolderPath += "/Log/" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
                    else
                        FolderPath += "/Log";
                    Directory.CreateDirectory(FolderPath);
                }
            }

            XmlNodeList entityNodeList = root.SelectNodes("LogEntity");

            foreach (XmlNode entityNode in entityNodeList)
            {
                string logKey = entityNode.SelectSingleNode("Key").InnerText;
                string filePath;

                filePath = FolderPath + "/" + logKey + ".log";

                int capacity = 1;

                if (int.TryParse(entityNode.SelectSingleNode("Capacity").InnerText.ToLower().Replace("mb", ""), out capacity))
                {
                    capacity = capacity * 1024 * 1024;
                }
                else
                {
                    capacity = 1024 * 1024;
                }

                m_logEntitys.Add(logKey, new LogEntity(filePath, Encoding.GetEncoding(m_logCoding), capacity, logKey));
            }
        }

        public static void InitLog(string rootPath, string logState, string coding, bool isDated)
        {
            SimpleLog.FolderPath = rootPath;
            SimpleLog.LogState = logState;
            SimpleLog.LogCoding = coding;
            SimpleLog.m_isDated = isDated;

            if ("default" == FolderPath)
                FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (SimpleLog.m_isDated)
                FolderPath += "/Log/" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            else
                FolderPath += "/Log";

            if (State.Off > SimpleLog.m_logState)
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch
                {
                    FolderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (isDated)
                        FolderPath += "/Log/" + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
                    else
                        FolderPath += "/Log";
                    Directory.CreateDirectory(FolderPath);
                }
            }
            m_logEntitys = new Dictionary<string, ILog>();
        }

        public static void CloseLog()
        {
            ILog[] logInstanceArray = new ILog[m_logEntitys.Values.Count];
            m_logEntitys.Values.CopyTo(logInstanceArray, 0);

            foreach (LogEntity entity in logInstanceArray)
                entity.Close();
        }

        private static void RemoveLogEntity(string key)
        {
            m_logEntitys.Remove(key);
        }

        public static ILog GetLogEntity(string key)
        {
            ILog logInstance = null;
            m_logEntitys.TryGetValue(key, out logInstance);
            return logInstance;
        }

        public static ILog CreateLogEntity(string logKey, string capacity)
        {
            string filePath;

            filePath = FolderPath + "/" + logKey + ".log";

            int capacity2 = 1;

            if (int.TryParse(capacity.ToLower().Replace("mb", ""), out capacity2))
            {
                capacity2 = capacity2 * 1024 * 1024;
            }
            else
            {
                capacity2 = 1024 * 1024;
            }

            m_logEntitys.Add(logKey, new LogEntity(filePath, Encoding.GetEncoding(m_logCoding), capacity2, logKey));
            return m_logEntitys[logKey];
        }

        private static Dictionary<string, ILog> m_logEntitys;
    }
}