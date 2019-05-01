namespace PureMVC.Interfaces
{
    using System;

    internal interface ICommand
    {
        void Execute(INotification notification);
    }
}
