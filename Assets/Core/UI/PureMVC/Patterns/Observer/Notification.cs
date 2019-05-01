namespace PureMVC.Patterns
{
    using System;
    using PureMVC.Interfaces;

    internal class Notification : INotification
    {
        #region 构造

        public Notification(int Id, object param, object extra)
        {
            m_notificationID = Id;
            m_notificationBody = param;
            m_notificationExtra = extra;
        }

        #endregion

        public override string ToString()
        {
            string msg = "Notification ID: " + ID;
            msg += "\nBody:" + ((Param == null) ? "null" : Param.ToString());
            msg += "\nExtra:" + ((Extra == null) ? "null" : Extra.ToString());
            return msg;
        }

        public int ID
        {
            get
            {
                return m_notificationID;
            }
        }

        public object Param
        {
            get
            {
                return m_notificationBody;
            }
            set
            {
                m_notificationBody = value;
            }
        }

        public virtual object Extra
        {
            get
            {
                return m_notificationExtra;
            }
            set
            {
                m_notificationExtra = value;
            }
        }

        private int m_notificationID;

        private object m_notificationExtra;

        private object m_notificationBody;
    }
}
