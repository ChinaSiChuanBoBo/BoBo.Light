namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using BoBo.Light.Base;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    /// <summary>
    /// UI基类
    /// </summary>
    public abstract class BaseView : UIComponent, INotifier, IMediator
    {
        public UIState State
        {
            get
            {
                return m_state;
            }

            protected set
            {
                m_state = value;
            }
        }

        public System.Type ScriptType
        {
            get
            {
                if (null == m_scriptType)
                    m_scriptType = this.GetType();
                return m_scriptType;
            }
        }

        internal void Open(object param, object extra)
        {
            this.State = UIState.Initial;
            //
            m_openedPages = new List<UIPage>();
            m_pages = new Dictionary<string, UIPage>();
            m_notificationInterests = new List<int>();
            m_observerMap = new Dictionary<int, IList<IObserver>>();
            //
            UIComponent[] uiComponents = this.GetComponentsInChildren<UIComponent>(true);
            if (null != uiComponents && uiComponents.Length > 0)
            {
                for (int i = 0; i < uiComponents.Length; ++i)
                {
                    var uiComponentItem = uiComponents[i];
                    if (uiComponentItem is UIPage) //如果是Page
                    {
                        UIPage page = (UIPage)uiComponentItem;
                        page.uiRoot = this;
                        m_pages.Add(page.Name, page);
                    }
                    AddUIObserver(uiComponentItem);
                }
            }
            // 
            this.gameObject.SetActive(true);
            OnOpened(param, extra);
            this.State = UIState.Update;
            //接入MVC消息框架
            Facade.RegisterMediator(this);
        }

        internal void Close()
        {
            //移出MVC消息框架
            Facade.RemoveMediator(((IMediator)this).MediatorName);
            //
            this.State = UIState.Closed;
            //关闭子页面
            foreach (UIPage page in m_openedPages)
                page.Hide();
            m_openedPages.Clear();
            //
            ClearUIObserver();
            m_notificationInterests.Clear();
            m_pages.Clear();
            m_pages = null;
            m_openedPages = null;
            OnClosed();
            this.gameObject.SetActive(false);
        }

        internal void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }

        /// <summary>
        /// 每个BaseView都有唯一的一个UI标识
        /// </summary>
        /// <returns></returns>
        public abstract string GetUiID();

        #region 虚方法

        /// <summary>
        /// 打开UI时,调用
        /// </summary>
        protected virtual void OnOpened(object param, object extra)
        {

        }



        /// <summary>
        /// 关闭UI时调用
        /// </summary>
        protected virtual void OnClosed()
        {

        }

        protected virtual void OnUpdate(float deltatime)
        {

        }

        protected virtual void OnDestroyed()
        {

        }
        #endregion

        #region unity 回调

        void Update()
        {
            if (UIState.Update == this.State)
            {
                OnUpdate(Time.deltaTime);
                for (int i = 0; i < m_openedPages.Count; ++i)
                {
                    m_openedPages[i].OnUpdate(Time.deltaTime);
                }
            }
        }

        void OnDestroy()
        {
            OnDestroyed();
        }
        #endregion

        public void HideAllPage()
        {
            for (int i = 0; i < m_openedPages.Count; ++i)
            {
                m_openedPages[i].Hide();
            }
            m_openedPages.Clear();
        }

        public UIPage GetPage(string pageName)
        {
            UIPage pageItem = null;
            if (null != m_pages)
            {
                m_pages.TryGetValue(pageName.ToLower(), out pageItem);
            }
            return pageItem;
        }

        public bool PopPage(string pageName, object param = null, object extra = null, bool hidePre = true)
        {
            UIPage page;
            if (null != m_pages && m_pages.TryGetValue(pageName.ToLower(), out page))
            {
                if (!m_openedPages.Contains(page))
                {
                    page.Pop(param, extra);
                    m_openedPages.Add(page);
                    if (hidePre)
                    {
                        if (m_openedPages.Count > 1) //不要把当前打开的包含进去
                        {
                            UIPage prePage = m_openedPages[m_openedPages.Count - 2];
                            m_openedPages.RemoveAt(m_openedPages.Count - 2);
                            prePage.Hide();
                        }
                    }
                }
            }
            else
                return false;
            return true;
        }

        public bool HidePage(string pageName)
        {
            UIPage page;
            if (null != m_pages && m_pages.TryGetValue(pageName.ToLower(), out page))
            {
                //打开列表中存在，表明该Page处于打开状态
                if (m_openedPages.Contains(page))
                {
                    m_openedPages.Remove(page);
                    page.Hide();
                }
            }
            else
                return false;

            return true;
        }


        private void AddUIObserver(IObserver observer)
        {
            IList<int> interests = observer.NotifierInterests();
            if (null != interests && interests.Count > 0)
            {
                for (int i = 0; i < interests.Count; ++i)
                {
                    int key = interests[i];
                    if (!m_notificationInterests.Contains(key))
                        m_notificationInterests.Add(key);

                    if (!m_observerMap.ContainsKey(key))
                    {
                        m_observerMap[key] = new List<IObserver>() { observer };
                    }
                    else
                    {
                        IList<IObserver> observerList = m_observerMap[key];
                        if (!observerList.Contains(observer))
                            observerList.Add(observer);
                    }
                }
            }
        }

        private void RemoveUIObserver(IObserver observer)
        {
            IList<int> interests = observer.NotifierInterests();
            if (null != interests && interests.Count > 0)
            {
                for (int i = 0; i < interests.Count; ++i)
                {
                    int key = interests[i];
                    if (m_observerMap.ContainsKey(key))
                    {
                        IList<IObserver> observerList = m_observerMap[key];
                        observerList.Remove(observer);
                        if (observerList.Count <= 0)
                            m_observerMap.Remove(key);
                    }
                }
            }
        }

        private void ClearUIObserver()
        {
            m_observerMap.Clear();
            m_observerMap = null;
        }

        protected UIState m_state = UIState.None;

        protected System.Type m_scriptType = null;

        //BaseView管理的UIPage
        private Dictionary<string, UIPage> m_pages = null;
        //打开的UIPage
        private List<UIPage> m_openedPages = null;
        //
        private List<int> m_notificationInterests;
        //
        private Dictionary<int, IList<IObserver>> m_observerMap;

        #region 实现IMediator接口

        IList<int> IMediator.ListNotificationInterests()
        {
            return m_notificationInterests;
        }

        void IMediator.OnRegister()
        {

        }

        void IMediator.OnRemove()
        {

        }

        string IMediator.MediatorName
        {
            get
            {
                return GetUiID();
            }
        }

        void IMediator.HandleNotification(INotification notification)
        {
            if (m_observerMap.ContainsKey(notification.ID))
            {
                IList<IObserver> observerList = m_observerMap[notification.ID];
                for (int i = 0; i < observerList.Count; ++i)
                {
                    IObserver observer = observerList[i];
                    if (observer.NotifyActive)
                    {
                        observer.HandleNotify(notification.ID, notification.Param, notification.Extra);
                    }
                }
            }
        }

        //用于反射调用
        protected void HandleNotification(INotification notification)
        {
            (this as IMediator).HandleNotification(notification);
        }


        #endregion
    }
}
