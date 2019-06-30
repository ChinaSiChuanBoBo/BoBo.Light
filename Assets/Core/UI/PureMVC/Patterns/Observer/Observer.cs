namespace PureMVC.Patterns
{
    using System;
    using System.Reflection;
    using PureMVC.Interfaces;

    /// <summary>
    /// 观察者对象
    /// </summary>
    internal class Observer : IObserver
    {
        public Observer(string notifyMethod, object notifyContext)
        {
            this.m_notifyMethod = notifyMethod;
            this.m_notifyContext = notifyContext;
        }

        #region 实现的接口

        //消息通知到观察者
        public virtual void NotifyObserver(INotification notification)
        {
            object context;
            string method;

            lock (m_lockContext)
            {
                context = NotifyContext;
                method = NotifyMethod;
            }

            Type t = context.GetType();
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public |BindingFlags.NonPublic| BindingFlags.IgnoreCase;
            MethodInfo mi = t.GetMethod(method, f);
            mi.Invoke(context, new object[] { notification });
        }
        //判断对象是不是观察者对象
        public virtual bool CompareNotifyContext(object obj)
        {
            lock (m_lockContext)
            {
                return NotifyContext.Equals(obj);
            }
        }
        //观察者处理消息的方法
        public virtual string NotifyMethod
        {
            private get
            {
                return m_notifyMethod;
            }
            set
            {
                m_notifyMethod = value;
            }
        }
        //观察者实体(上下文)
        public virtual object NotifyContext
        {
            private get
            {
                return m_notifyContext;
            }
            set
            {
                m_notifyContext = value;
            }
        }
        #endregion

        private string m_notifyMethod;
        private object m_notifyContext;
        protected readonly object m_lockContext = new object();
    }
}
