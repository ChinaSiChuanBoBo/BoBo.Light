namespace PureMVC.Interfaces
{
    using System;

    internal interface IModel
    {
        void RegisterProxy(IProxy proxy);

        IProxy RetrieveProxy(string proxyName);

        IProxy RemoveProxy(string proxyName);

        bool HasProxy(string proxyName);
    }
}
