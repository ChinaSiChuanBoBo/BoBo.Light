namespace PureMVC.Interfaces
{
    using System;

    internal interface IController
    {
        void RegisterCommand(int id, Type commandType);

        void ExecuteCommand(INotification notification);

        void RemoveCommand(int id);

        bool HasCommand(int id);
    }
}
