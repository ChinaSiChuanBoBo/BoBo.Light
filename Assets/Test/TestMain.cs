using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.UI;
using UnityEngine.UI;




public class TestMain : MonoBehaviour
{


    public Image imageObject;



    [ShowOnly]
    [SerializeField]
    protected int num;


    void OnGUI()
    {
        if (GUILayout.Button("加载TestUI"))
        {
            num = 12;
            //  UIFacade.NewFacade<TestUIFacade>();
        }

        if (GUILayout.Button("移除TestUI"))
        {
            num = 13;
            // UIFacade.RemoveFacade<TestUIFacade>();
        }
    }
}
