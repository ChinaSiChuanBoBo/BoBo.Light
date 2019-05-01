namespace PureMVC.Interfaces
{
    using System;

    internal interface IView
    {
        void RegisterObserver(int id, IObserver observer);

        void RemoveObserver(int id, object notifyContext);

        void NotifyObservers(INotification note);

        void RegisterMediator(IMediator mediator);

        IMediator RetrieveMediator(string mediatorName);

        IMediator RemoveMediator(string mediatorName);

        bool HasMediator(string mediatorName);
    }
}
