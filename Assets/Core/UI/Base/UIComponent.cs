namespace BoBo.Light.UI
{
    using PureMVC.Patterns;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class UIComponent : MonoBehaviour, IObserver
    {
        public void SendNotification(int id, object param = null, object extra = null)
        {
            Facade.NotifyObservers(new Notification(id, param, extra));
        }

        public bool NotifyActive
        {
            get
            {
                return this.gameObject.activeInHierarchy;
            }
        }

        public virtual void HandleNotify(int id, object param, object extra)
        {

        }

        public virtual IList<int> NotifierInterests()
        {
            return null;
        }
    }
}
