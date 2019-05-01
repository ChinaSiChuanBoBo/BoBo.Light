namespace PureMVC.Interfaces
{
    using System;
    using System.Collections.Generic;

    internal interface IMediator
    {
        string MediatorName { get; }

        IList<int> ListNotificationInterests();

        void HandleNotification(INotification notification);

        void OnRegister();

        void OnRemove();
    }
}
