namespace PureMVC.Core
{
    using System;
    using System.Collections.Generic;
    using PureMVC.Interfaces;

    internal class Model : IModel
    {
        #region 单例

        public static IModel Instance
        {
            get
            {
                if (null == m_instance)
                {
                    lock (m_lockSingleton)
                    {
                        if (null == m_instance)
                            m_instance = new Model();
                    }
                }
                return m_instance;
            }
        }

        static Model()
        {
        }

        protected Model()
        {
            m_proxyMap = new Dictionary<string, IProxy>();
            InitializeModel();
        }
        #endregion

        #region 实现的接口

        public virtual void RegisterProxy(IProxy proxy)
        {
            lock (m_lockMap)
            {
                m_proxyMap[proxy.ProxyName] = proxy;
            }
            proxy.OnRegister();
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            lock (m_lockMap)
            {
                if (!m_proxyMap.ContainsKey(proxyName))
                    return null;
                return m_proxyMap[proxyName];
            }
        }

        public virtual bool HasProxy(string proxyName)
        {
            lock (m_lockMap)
            {
                return m_proxyMap.ContainsKey(proxyName);
            }
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy = null;

            lock (m_lockMap)
            {
                if (m_proxyMap.ContainsKey(proxyName))
                {
                    proxy = RetrieveProxy(proxyName);
                    m_proxyMap.Remove(proxyName);
                }
            }

            if (null != proxy)
                proxy.OnRemove();
            return proxy;
        }
        #endregion

        protected virtual void InitializeModel()
        {

        }

        protected IDictionary<string, IProxy> m_proxyMap;

        protected static volatile IModel m_instance;

        protected readonly object m_lockMap = new object();

        protected static readonly object m_lockSingleton = new object();
    }
}
