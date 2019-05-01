
using System;
using System.Reflection;
using PureMVC.Patterns;

namespace PureMVC.Interfaces
{
    //观察者
    internal interface IObserver
    {
        //消息的处理实体名(方法名)
        string NotifyMethod { set; }
        //消息的监听对象
        object NotifyContext { set; }
        //消息派发
        void NotifyObserver(INotification notification);
        //对象实例是不是消息的监听对象
        bool CompareNotifyContext(object obj);
    }
}
