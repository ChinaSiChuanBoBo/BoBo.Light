namespace PureMVC.Patterns
{
    using System;
    using System.Collections.Generic;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    internal class Mediator :IMediator,INotifier
    {
        public const string NAME = "Mediator";

        #region 构造

        public Mediator()
            : this(NAME, null)
        {

        }

        public Mediator(string mediatorName)
            : this(mediatorName, null)
        {
        }


        public Mediator(string mediatorName, object viewComponent)
        {
            this.m_mediatorName = (mediatorName != null) ? mediatorName : NAME;
            this.m_viewComponent = viewComponent;
        }
        #endregion

        #region 实现的接口

        public virtual IList<int> ListNotificationInterests()
        {
            return new List<int>();
        }

        public virtual void HandleNotification(INotification notification)
        {

        }

        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }
        #endregion

        public virtual string MediatorName
        {
            get { return m_mediatorName; }
        }

        public virtual object ViewComponent
        {
            get { return m_viewComponent; }
            set { m_viewComponent = value; }
        }

        protected string m_mediatorName;

        protected object m_viewComponent;

        public void SendNotification(int id, object param, object extra)
        {
            Facade.NotifyObservers(new Notification(id, param, extra));
        }
    }
}
