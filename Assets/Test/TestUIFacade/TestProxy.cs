using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;

public class TestProxy : Proxy
{
    public new static string NAME = "TestProxy";

    public TestProxy()
        : base(NAME)
    {

    }


    public void SetColor()
    {
        Debug.Log("ssssssssssss");
        SendNotification(EventID.DisplayColor);
    }

}
