namespace PureMVC.Core
{
    using System;
    using System.Collections.Generic;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    internal class View : IView
    {
        #region 单例

        public static IView Instance
        {
            get
            {
                if (null == m_instance)
                {
                    lock (m_lockSingleton)
                    {
                        if (null == m_instance)
                            m_instance = new View();
                    }
                }
                return m_instance;
            }
        }

        static View()
        {
        }

        protected View()
        {
            m_mediatorMap = new Dictionary<string, IMediator>();
            m_observerMap = new Dictionary<int, IList<IObserver>>();
            InitializeView();
        }
        #endregion

        #region 实现的接口

        #region 观察者模式的消息机制
        //消息注册，消息名和IObserver观察者绑定
        public virtual void RegisterObserver(int id, IObserver observer)
        {
            lock (m_lockMap)
            {
                if (!m_observerMap.ContainsKey(id))
                {
                    m_observerMap[id] = new List<IObserver>();
                }
                m_observerMap[id].Add(observer);
            }
        }
        //派发消息
        public virtual void NotifyObservers(INotification notification)
        {
            IList<IObserver> observers = null;
            lock (m_lockMap)
            {
                if (m_observerMap.ContainsKey(notification.ID))
                {
                    IList<IObserver> observers_ref = m_observerMap[notification.ID];
                    observers = new List<IObserver>(observers_ref);
                }
            }

            if (observers != null)
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    IObserver observer = observers[i];
                    observer.NotifyObserver(notification);
                }
            }
        }
        //取消观察者对消息的监听
        public virtual void RemoveObserver(int id, object notifyContext)
        {
            lock (m_lockMap)
            {
                if (m_observerMap.ContainsKey(id))
                {
                    IList<IObserver> observers = m_observerMap[id];
                    for (int i = 0; i < observers.Count; i++)
                    {
                        if (observers[i].CompareNotifyContext(notifyContext))
                        {
                            observers.RemoveAt(i);
                            break;
                        }
                    }
                    if (observers.Count == 0)
                    {
                        m_observerMap.Remove(id);
                    }
                }
            }
        }
        #endregion

        public virtual void RegisterMediator(IMediator mediator)
        {
            lock (m_lockMap)
            {
                if (m_mediatorMap.ContainsKey(mediator.MediatorName))
                    return;
                m_mediatorMap[mediator.MediatorName] = mediator;
                IList<int> interests = mediator.ListNotificationInterests();
                if (null != interests && interests.Count > 0)
                {
                    IObserver observer = new Observer("handleNotification", mediator);
                    for (int i = 0; i < interests.Count; i++)
                    {
                        RegisterObserver(interests[i], observer);
                    }
                }
            }
            mediator.OnRegister();
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            lock (m_lockMap)
            {
                if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
                return m_mediatorMap[mediatorName];
            }
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            IMediator mediator = null;

            lock (m_lockMap)
            {
                if (!m_mediatorMap.ContainsKey(mediatorName))
                    return null;
                mediator = m_mediatorMap[mediatorName];
                IList<int> interests = mediator.ListNotificationInterests();
                if (null != interests && interests.Count > 0)
                {
                    for (int i = 0; i < interests.Count; i++)
                    {
                        RemoveObserver(interests[i], mediator);
                    }
                }
                m_mediatorMap.Remove(mediatorName);
            }
            if (mediator != null)
                mediator.OnRemove();
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            lock (m_lockMap)
            {
                return m_mediatorMap.ContainsKey(mediatorName);
            }
        }
        #endregion

        protected virtual void InitializeView()
        {
        }

        protected IDictionary<string, IMediator> m_mediatorMap;

        protected IDictionary<int, IList<IObserver>> m_observerMap;

        protected static volatile IView m_instance;

        protected readonly object m_lockMap = new object();

        protected static readonly object m_lockSingleton = new object();
    }
}
