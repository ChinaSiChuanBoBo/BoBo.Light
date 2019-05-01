namespace PureMVC.Interfaces
{
    using System;

    public interface IProxy
    {
        string ProxyName { get; }

        object Data { get; set; }

        void OnRegister();

        void OnRemove();
    }
}
