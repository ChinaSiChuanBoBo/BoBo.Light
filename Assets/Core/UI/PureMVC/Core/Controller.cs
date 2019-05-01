namespace PureMVC.Core
{
    using System;
    using System.Collections.Generic;

    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    internal class Controller : IController
    {
        #region 单例

        public static IController Instance
        {
            get
            {
                if (null == m_instance)
                {
                    lock (m_lockSingleton)
                    {
                        if (null == m_instance)
                            m_instance = new Controller();
                    }
                }

                return m_instance;
            }
        }

        protected Controller()
        {
            m_commandMap = new Dictionary<int, Type>();
            InitializeController();
        }

        static Controller()
        {

        }
        #endregion

        #region 实现的接口

        public virtual void ExecuteCommand(INotification note)
        {
            Type commandType = null;

            lock (m_lockMap)
            {
                if (!m_commandMap.ContainsKey(note.ID))
                    return;
                commandType = m_commandMap[note.ID];
            }

            //反射执行，反射在.net3.5下面效率不够高
            object commandInstance = Activator.CreateInstance(commandType);

            if (commandInstance is ICommand)
            {
                ((ICommand)commandInstance).Execute(note);
            }
        }

        public virtual void RegisterCommand(int id, Type commandType)
        {
            lock (m_lockMap)
            {
                if (!m_commandMap.ContainsKey(id))
                {
                    m_view.RegisterObserver(id, new Observer("executeCommand", this));
                }
                m_commandMap[id] = commandType;
            }
        }

        public virtual bool HasCommand(int id)
        {
            lock (m_lockMap)
            {
                return m_commandMap.ContainsKey(id);
            }
        }

        public virtual void RemoveCommand(int id)
        {
            lock (m_lockMap)
            {
                if (m_commandMap.ContainsKey(id))
                {
                    m_view.RemoveObserver(id, this);
                    m_commandMap.Remove(id);
                }
            }
        }
        #endregion

        protected virtual void InitializeController()
        {
            m_view = View.Instance;
        }

        protected IView m_view;

        protected IDictionary<int, Type> m_commandMap;

        protected static volatile IController m_instance;

        protected readonly object m_lockMap = new object();

        protected static readonly object m_lockSingleton = new object();
    }
}
