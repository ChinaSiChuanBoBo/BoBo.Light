namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using BoBo.Light.Base;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    /// <summary>
    /// UI管理器
    /// </summary>
    internal sealed class UIRes : MonoSingleton<UIRes>
    {
        /// <summary>
        /// UI资源的底层封装类
        /// </summary>
        public class AssetInformation
        {
            public AssetInformation(string resID)
            {
                this.ResID = resID;
                RefCount = 0;
                Path = "UI/Prefabs/" + resID;
            }

            public string ResID
            {
                get;
                protected set;
            }

            public int RefCount
            {
                get;
                set;
            }

            public string Path
            {
                get;
                private set;
            }

            /// <summary>
            /// Asset 是否被加载;True表示被加载 False表示没有被加载
            /// </summary>
            public bool IsLoaded
            {
                get
                {
                    return null != m_assetObject;
                }
            }

            public UnityEngine.Object AssetObject
            {
                get
                {
                    return m_assetObject;
                }
            }
            /// <summary>
            /// 协程加载
            /// </summary>
            /// <param name="loaded">加载完成后的回调</param>
            public UnityEngine.Object AssetLoad()
            {
                if (null == m_assetObject)
                {
                    try
                    {
                        m_assetObject = Resources.Load(Path);
                    }
                    catch
                    {
                        throw new UIException("UI Load Failed" + Path);
                    }
                }

                return m_assetObject;
            }
            /// <summary>
            ///  UI Asset销毁倒计时
            /// </summary>
            public IEnumerator Destroy()
            {
                float timeCount = 0;
                while (true)
                {
                    yield return null;
                    timeCount += Time.deltaTime;
                    if (RefCount <= 0)
                    {
                        if (timeCount >= activeTime)
                        {
                            m_assetObject = null;
                            Resources.UnloadUnusedAssets();
                            UIRes.Instance.m_assets.Remove(this.ResID);
                            yield break;
                        }
                    }
                    else
                    {
                        yield break;
                    }
                }
            }

            private UnityEngine.Object m_assetObject;
            private const float activeTime = 60;
        }

        private UIRes() { }


        public GameObject CavasObject
        {
            get
            {
                return m_canvasObject;
            }
        }

        public Vector3 ScreenPointToWorldPointInRectangle(Vector2 screenPos)
        {
            Vector3 posInRectangle;
            Camera uiCamera = m_canvasObject.GetComponent<Canvas>().worldCamera;
            RectTransform rectTrans = m_canvasObject.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTrans, screenPos, uiCamera, out posInRectangle);
            return posInRectangle;
        }

        public BaseView OpenUI(string uiID, object param = null, object extra = null)
        {
            if (m_uiElements.ContainsKey(uiID))
            {
                UIElement element = m_uiElements[uiID];
                if (null == element.UiInstance)
                {
                    AssetInformation assetInfo;
                    var obj = (GameObject)GameObject.Instantiate(LoadImmediately(element.UiId, out assetInfo));
                    BaseView uiScript = obj.GetComponent<BaseView>();
                    element.UiAsset = assetInfo;
                    element.UiAsset.RefCount++;
                    element.UiInstance = uiScript;
                    element.UiInstance.transform.SetParent(m_canvasObject.transform, false);
                    element.UiInstance.transform.SetAsLastSibling();
                    element.UiInstance.GetComponent<RectTransform>().sizeDelta = element.BaseInfo.sizeDelta;
                    element.UiInstance.GetComponent<RectTransform>().anchoredPosition3D
                        = element.BaseInfo.anchoredPosition3D;
                    element.UiInstance.name = uiScript.GetUiID();
                    element.UiInstance.Open(param, extra);
                }
                else
                {
                    if (UIState.Update == element.UiInstance.State)
                    {

                    }
                    else if (UIState.Closed == element.UiInstance.State)
                    {
                        element.UiInstance.transform.SetAsLastSibling();
                        element.UiInstance.Open(param, extra);
                        element.ActiveTime += 1;
                    }
                }
                return element.UiInstance;
            }
            else
            {
                throw new UIException("UI Not Exit!!!" + uiID);
            }
        }

        public void CloseUI(string uiID)
        {
            CloseUI(new string[] { uiID });
        }

        public void CloseUI(string[] idArray)
        {
            for (int i = 0; i < idArray.Length; ++i)
            {
                UIElement element = null;
                m_uiElements.TryGetValue(idArray[i], out element);
                if (null != element && null != element.UiInstance)
                {
                    if (UIState.Update == element.UiInstance.State)
                    {
                        element.UiInstance.Close();
                        UIRes.Instance.StartCoroutine(element.Destroy());
                    }
                }
            }
        }

        public void CloseAll()
        {
            string[] idArray = new string[m_uiElements.Keys.Count];
            m_uiElements.Keys.CopyTo(idArray, 0);
            CloseUI(idArray);
        }

        //当前是不是触摸在UI上
        public bool TouchUIAtPresent(int layerMask=-1)
        {
            PointerEventData eventData = new PointerEventData(m_eventSystemComponent);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;
            List<RaycastResult> list = new List<RaycastResult>();
            GraphicRaycasterComponent.Raycast(eventData, list);
            foreach (var result in list)
            {
                if (layerMask != result.gameObject.layer)
                    return true;
            }
            return false;
        }

        private EventSystem EventSystemComponent
        {
            get
            {
                if (null == m_eventSystemComponent)
                    m_eventSystemComponent = m_eventSystemObject.GetComponent<EventSystem>();
                return m_eventSystemComponent;
            }
        }

        private GraphicRaycaster GraphicRaycasterComponent
        {
            get
            {
                if (null == m_graphicRaycaster)
                    m_graphicRaycaster = m_canvasObject.GetComponent<GraphicRaycaster>();
                return m_graphicRaycaster;
            }
        }

        private EventSystem m_eventSystemComponent;

        private GraphicRaycaster m_graphicRaycaster;

        #region Inner

        protected override void Init()
        {
            m_uiElements = new Dictionary<string, UIElement>();
            m_assets = new Dictionary<string, AssetInformation>();
            try
            {
                UIAssetInfo[] infos = Resources.LoadAll<UIAssetInfo>("UI/UIAssetInfos");
                for (int i = 0; i < infos.Length; ++i)
                {
                    UIAssetInfo info = infos[i];
                    if (!m_uiElements.ContainsKey(info.uiID))
                    {
                        m_uiElements.Add(info.uiID, new UIElement(info));
                    }
                }
            }
            catch
            {

            }

            m_canvasObject = GameObject.Find("Canvas");
            m_eventSystemObject = GameObject.Find("EventSystem");

            if (null == m_canvasObject)
            {
                #region 自动创建Cavas代码

                m_canvasObject = new GameObject("Canvas");
                int uiLayer = LayerMask.NameToLayer("UI");
                m_canvasObject.layer = uiLayer;
                Canvas canvasComponent = m_canvasObject.AddComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;
                //
                Camera uiCamera = new GameObject("UICamera").AddComponent<Camera>();
                uiCamera.transform.SetParent(m_canvasObject.transform, false);
                uiCamera.gameObject.layer = m_canvasObject.layer;
                canvasComponent.worldCamera = uiCamera;
                //设置UI相机
                uiCamera.transform.Translate(Vector3.back * 100);
                uiCamera.clearFlags = CameraClearFlags.Depth;
                uiCamera.cullingMask = 0x01 << uiLayer;
                uiCamera.orthographic = true;
                uiCamera.useOcclusionCulling = false;
#if Patch_5_6X
                uiCamera.allowHDR = false;
                uiCamera.allowMSAA = false;
#endif
                //设置屏幕适配
                CanvasScaler scalerComponent = m_canvasObject.AddComponent<CanvasScaler>();
                scalerComponent.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scalerComponent.referenceResolution = new Vector2(Screen.width, Screen.height);
                scalerComponent.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scalerComponent.matchWidthOrHeight = 1;
                //
                m_canvasObject.AddComponent<GraphicRaycaster>();
                //
                if (null == m_eventSystemObject)
                {
                    m_eventSystemObject = new GameObject("EventSystem");
                    m_eventSystemObject.AddComponent<EventSystem>();
                    m_eventSystemObject.AddComponent<StandaloneInputModule>();
                }
                #endregion
            }

            GameObject.DontDestroyOnLoad(m_canvasObject);
            GameObject.DontDestroyOnLoad(m_eventSystemObject);
        }

        private Dictionary<string, UIElement> m_uiElements;

        private GameObject m_canvasObject;
        private GameObject m_eventSystemObject;

        private Dictionary<string, AssetInformation> m_assets = null;


        private UnityEngine.Object LoadImmediately(string resID, out AssetInformation outAssetInfo)
        {
            outAssetInfo = GetAssetInformation(resID);
            return outAssetInfo.AssetLoad();
        }

        private AssetInformation GetAssetInformation(string resID)
        {
            AssetInformation assetInfo = null;
            if (!m_assets.TryGetValue(resID, out assetInfo))
            {
                assetInfo = new AssetInformation(resID);
                m_assets.Add(resID, assetInfo);
            }
            return assetInfo;
        }
        #endregion
    }
}
