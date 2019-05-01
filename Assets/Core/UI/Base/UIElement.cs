namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using System.Xml;

    internal class UIElement
    {
        public UIElement(UIAssetInfo info)
        {
            BaseInfo = info;
            ActiveTime = BaseInfo.defaultActiveTime;
        }


        public UIAssetInfo BaseInfo
        {
            get;
            protected set;
        }

        public string UiId
        {
            get
            {
                return BaseInfo.uiID;
            }       
        }

        public float ActiveTime
        {
            get
            {
                return m_activeTime;
            }
            set
            {
                if (value <= BaseInfo.maxActiveTime)
                    m_activeTime = value;
                else
                    m_activeTime = BaseInfo.maxActiveTime;
            }
        }
        private float m_activeTime;

        public BaseView UiInstance
        {
            get;
            set;
        }

        internal UIRes.AssetInformation UiAsset
        {
            get;
            set;
        }
        /// <summary>
        ///  UI GameObject销毁倒计时
        /// </summary>
        public IEnumerator Destroy()
        {
            float timeCount = 0;
            while (true)
            {
                yield return null;
                if (UIState.Closed == UiInstance.State)
                {
                    timeCount += Time.deltaTime;
                    if (timeCount >= ActiveTime)
                    {
                        //销毁UiInstance
                        UiInstance.Destroy();
                        UiInstance = null;
                        ActiveTime = BaseInfo.defaultActiveTime;
                        UiAsset.RefCount--;
                        if (UiAsset.RefCount <= 0)
                        {
                            //开启ui asset销毁倒计时
                            UIRes.Instance.StartCoroutine(UiAsset.Destroy());
                            UiAsset = null;
                        }
                        yield break;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

 
    }
}
