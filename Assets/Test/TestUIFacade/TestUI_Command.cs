using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using BoBo.Light.Base;

public class TestUI_Command : SimpleCommand
{
    public override void Execute(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.ID)
        {
            case EventID.ChangeColor:
                {
                    var proxy = RetrieveProxy(TestProxy.NAME) as TestProxy;
                    if (null != proxy)
                        proxy.SetColor();


                } break;
        }
    }
}
