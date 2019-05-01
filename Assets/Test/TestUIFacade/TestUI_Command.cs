using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;

public class TestUI_Command : SimpleCommand
{
    public override void Execute(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.ID)
        {
            case EventID.ChangeColor:
                {
                    var scriptInstance = Camera.main.GetComponent<TestMain>();
                    scriptInstance.imageObject.color = (Color)notification.Param;
                } break;
        }
    }
}
