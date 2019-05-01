namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;

    public class UIException : System.Exception
    {
        public UIException(string msg)
            : base(msg)
        {

        }
    }
}
