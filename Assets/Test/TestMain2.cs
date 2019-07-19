using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.Base;

public class TestMain2 : BaseNode
{

    void Start()
    {
        NodeInit();
    }

    void OnDestroy()
    {
        NodeClearup();
    }


    public override void MessageHandler(int eventID, object param, object extra)
    {
        switch (eventID)
        {
            case EventID.ChangeColor:
                {
                    Debug.Log("TestMain2也监听了这个消息");
                } break;
        }
    }

    public override IList<int> ListeningNotifications()
    {
        return new int[] { EventID.ChangeColor };
    }

    public override int NodeLevel
    {
        get
        {
            return 0;
        }
    }
}
