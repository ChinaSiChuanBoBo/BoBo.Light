namespace PureMVC.Patterns
{
    using System;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    public class Proxy :IProxy, INotifier
    {
        public static string NAME = "Proxy";

        #region 构造

        public Proxy()
            : this(NAME, null)
        {
        }


        public Proxy(string proxyName)
            : this(proxyName, null)
        {
        }

        public Proxy(string proxyName, object data)
        {
            this.m_proxyName = (proxyName != null) ? proxyName : NAME;
            if (data != null)
                this.m_data = data;
        }
        #endregion


        #region 实现的接口

        //IProxy
        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }

        public virtual string ProxyName
        {
            get { return m_proxyName; }
        }

        public virtual object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }


        #endregion

        protected string m_proxyName;
        protected object m_data;

        public void SendNotification(int id, object param=null, object extra=null)
        {
            Facade.NotifyObservers(new Notification(id, param, extra));
        }
    }
}
