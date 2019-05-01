namespace BoBo.Light.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal interface IObserver
    {
        bool NotifyActive { get; }

        void HandleNotify(int id, object param, object extra);
        IList<int> NotifierInterests();
    }
}
