namespace PureMVC.Patterns
{
    using System;
    using System.Collections.Generic;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    public class SimpleCommand : ICommand, INotifier
    {
        public virtual void Execute(INotification notification)
        {

        }

        public void SendNotification(int id, object param, object extra)
        {
            Facade.NotifyObservers(new Notification(id, param, extra));
        }


        public IProxy RetrieveProxy(string proxyName)
        {
            return Facade.RetrieveProxy(proxyName);
        }

    }
}
