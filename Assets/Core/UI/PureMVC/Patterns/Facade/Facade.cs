namespace PureMVC.Patterns
{
    using System;
    using PureMVC.Core;
    using PureMVC.Interfaces;
    using PureMVC.Patterns;

    /// <summary>
    /// PureMVC启动的入口
    /// </summary>
    public class Facade
    {
        public void RegisterProxy(IProxy proxy)
        {
            GetModel.RegisterProxy(proxy);
        }

        public IProxy RetrieveProxy(string proxyName)
        {
            return GetModel.RetrieveProxy(proxyName);
        }

        public IProxy RemoveProxy(string proxyName)
        {
            return GetModel.RemoveProxy(proxyName);
        }

        public bool HasProxy(string proxyName)
        {
            return GetModel.HasProxy(proxyName);
        }
        //控制器
        public void RegisterCommand(int id, Type commandType)
        {
            GetController.RegisterCommand(id, commandType);
        }

        public void RemoveCommand(int id)
        {
            GetController.RemoveCommand(id);
        }

        public bool HasCommand(int id)
        {
            return GetController.HasCommand(id);
        }
        //视图层
        internal static void RegisterMediator(IMediator mediator)
        {
            GetView.RegisterMediator(mediator);
        }

        internal static IMediator RetrieveMediator(string mediatorName)
        {
            return GetView.RetrieveMediator(mediatorName);
        }

        internal static IMediator RemoveMediator(string mediatorName)
        {
            return GetView.RemoveMediator(mediatorName);
        }

        internal static bool HasMediator(string mediatorName)
        {
            return GetView.HasMediator(mediatorName);
        }

        #region 消息机制

        //实现消息派发
        internal static void NotifyObservers(INotification notification)
        {
            GetView.NotifyObservers(notification);
        }
        #endregion

        #region Inner

        private static IController GetController
        {
            get
            {
                return Controller.Instance;
            }
        }

        private static IModel GetModel
        {
            get
            {
                return Model.Instance;
            }
        }

        private static IView GetView
        {
            get
            {
                return View.Instance;
            }
        }
        #endregion
    }
}
