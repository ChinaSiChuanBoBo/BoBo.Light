using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoBo.Light.UI;
namespace TestUIKit
{
    public class TestPage : UIPage
    {
        protected override void OnPopup(object param, object extra)
        {
            Debug.Log(typeof(TestPage).Name);
        }

    }
}