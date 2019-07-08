using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestButton : MonoBehaviour
{
    [Button("ShowFadeInFunc")]
    public Image ShowFadeinImages;
    public void ShowFadeInFunc()
    {
        //foreach (var imageItem in fadeinElements)
        //{
        //    var tempColor = imageItem.color;
        //    imageItem.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1f);
        //}
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
