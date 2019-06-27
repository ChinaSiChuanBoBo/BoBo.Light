using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.UI;

public class TestUIFacade : UIFacade
{



    protected override void OnInit()
    {
        RegisterCommand(EventID.ChangeColor, typeof(TestUI_Command));
        RegisterProxy(new TestProxy());
    }

    protected override void OnDestroy()
    {
        RemoveCommand(EventID.ChangeColor);
        RemoveProxy(TestProxy.NAME);
    }


    public override IList<string> ViewNames
    {
        get
        {
            return new string[] { "TestUI" };
        }
    }
}
