namespace PureMVC.Interfaces
{
    using System;

    //消息
    public interface INotification
    {
        int ID { get; }


        object Param { get; set; }


        object Extra { get; set; }

        string ToString();
    }
}
