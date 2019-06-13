using UnityEngine;
using System.Collections;
using System;

//这个特性只能用于unity序列化的字段
[AttributeUsage(AttributeTargets.Field,AllowMultiple=false)]
public class ShowOnlyAttribute : PropertyAttribute
{


}

