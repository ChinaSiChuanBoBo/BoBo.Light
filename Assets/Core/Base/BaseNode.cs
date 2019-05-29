namespace BoBo.Light.Base
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class BaseNode : MonoBehaviour
    {
        public abstract int NodeLevel
        {
            get;
        }

        public static void Broadcast(int cmdLevel, int eventID, object param = null, object extra = null)
        {
            Messenger.Broadcast<int, object, object>(cmdLevel, eventID, eventID, param, extra);
        }

        public void Init()
        {
            var notifications = ListeningNotifications();
            if (null != notifications && notifications.Count > 0)
            {
                for (int i = 0; i < notifications.Count; ++i)
                {
                    Messenger.AddListener<int, object, object>(NodeLevel, notifications[i], MessageHandler);
                }
            }

            OnInit();
        }

        public void Clearup()
        {
            var notifications = ListeningNotifications();
            if (null != notifications && notifications.Count > 0)
            {
                for (int i = 0; i < notifications.Count; ++i)
                {
                    Messenger.RemoveListener<int, object, object>(NodeLevel, notifications[i], MessageHandler);
                }
            }

            OnClearup();
        }

        public virtual void OnInit()
        {

        }

        public virtual void OnClearup()
        {

        }


        public virtual IList<int> ListeningNotifications()
        {
            return null;
        }


        public virtual void MessageHandler(int eventID, object param, object extra)
        {

        }
    }
}
