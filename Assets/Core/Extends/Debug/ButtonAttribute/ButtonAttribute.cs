using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ButtonAttribute : PropertyAttribute
{
    //按钮高度
    public readonly float height = 16;
    //绑定的函数
    public readonly string[] funcNames = null;
    //运行时是否显示
    public bool showOnPlay = true;  //True:显示

    public ButtonAttribute(params string[] funcNames)
    {
        this.funcNames = funcNames;
    }

    public ButtonAttribute(bool showOnPlay, float height, params string[] funcNames)
    {
        this.showOnPlay = showOnPlay;
        this.height = height;
        this.funcNames = funcNames;
    }
}
