namespace PureMVC.Interfaces
{
    using System;
    internal interface INotifier
    {
        void SendNotification(int id, object param, object extra);
    }
}
