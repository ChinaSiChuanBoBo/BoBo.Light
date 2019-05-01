using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 3D主相机根据高度适配
/// </summary>
public class MainCameraScale : MonoBehaviour
{

    public int manualWidth = 960;
    public int manualHeight = 640;
    void Start()
    {
        int adjustHeight;
        //当前屏幕的纵横比如果大于指定的纵横比，说明画面被拉伸。那么通过调整camera的fieldOfView降低画面中模型的拉伸观感
        if (System.Convert.ToSingle(Screen.height) / Screen.width >
            System.Convert.ToSingle(manualHeight) / manualWidth)
        {
            //四舍五入取整
            adjustHeight = Mathf.RoundToInt(System.Convert.ToSingle(manualWidth) / Screen.width * Screen.height);
        }
        else
            adjustHeight = manualHeight;

        Camera camera = GetComponent<Camera>();
        float scale = System.Convert.ToSingle(adjustHeight) / manualHeight;
        camera.fieldOfView *= scale;
    }
}

