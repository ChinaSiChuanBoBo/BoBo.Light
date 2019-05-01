using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.UI;
using UnityEngine.UI;




public class TestMain : MonoBehaviour
{


    public Image imageObject;

 


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
}
