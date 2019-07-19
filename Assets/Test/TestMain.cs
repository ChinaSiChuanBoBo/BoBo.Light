using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.UI;
using BoBo.Light.Base;
using UnityEngine.UI;




public class TestMain : BaseNode
{
    public Image imageObject;

    void Start()
    {
        NodeInit();
    }

    void OnDestroy()
    {
        NodeClearup();
    }

    void OnGUI()
    {
        if (GUILayout.Button("加载TestUI"))
        {
            UIFacade.NewFacade<TestUIFacade>();
        }

        if (GUILayout.Button("移除TestUI"))
        {
            UIFacade.RemoveFacade<TestUIFacade>();
        }
    }

    public override IList<int> ListeningNotifications()
    {
        return new int[] { EventID.ChangeColor };
    }

    public override void MessageHandler(int eventID, object param, object extra)
    {
        switch (eventID)
        {
            case EventID.ChangeColor:
                {
                    imageObject.color = (Color)param;
                } break;
            default: break;
        }
    }

    public override int NodeLevel
    {
        get { return 0; }
    }
}
