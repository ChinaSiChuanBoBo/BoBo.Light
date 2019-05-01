namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.EventSystems;

    public class UIDragComponent : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        // 鼠标起点  
        private Vector2 m_mouseStartPoint;
        // 面板起点  
        private Vector3 m_PanelStartPoint;
        // 当前"面板"  
        private RectTransform m_panelRect;
        //顶层BaseUI或者就是自身
        private RectTransform m_layoutRect;
        // Canvas Root
        private RectTransform m_rootRect;



        void Awake()
        {
            UIPage uiPage;
            BaseView uiBase;
            //自身或者父节点是UIPage
            if ((uiPage = this.GetComponentInParent<UIPage>()) == null ? false : true)
            {
                m_panelRect = uiPage.transform as RectTransform;
                m_layoutRect = this.GetComponentInParent<BaseView>().transform as RectTransform;
            }//自身或者父节点是BaseView
            else if ((uiBase = this.GetComponentInParent<BaseView>()) == null ? false : true)
            {
                m_panelRect = uiBase.transform as RectTransform;
                m_layoutRect = uiBase.transform as RectTransform;
            }
            else
            {
                m_panelRect = this.transform as RectTransform;
                m_layoutRect = this.transform as RectTransform;
            }
            m_rootRect = UIRes.Instance.CavasObject.transform as RectTransform;
        }

        // 鼠标按下  
        public void OnPointerDown(PointerEventData data)
        {
            m_layoutRect.transform.SetAsLastSibling();
            // 记录当前面板起点  
            m_PanelStartPoint = m_panelRect.localPosition;
            // 通过屏幕中的鼠标点，获取在父节点中的鼠标点  
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rootRect,
                data.position, data.pressEventCamera, out m_mouseStartPoint);
        }
        // 拖动  
        public void OnDrag(PointerEventData data)
        {
            if (m_panelRect == null || m_rootRect == null)
                return;

            Vector2 localPointerPosition;
            // 获取本地鼠标位置  
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rootRect,
                data.position, data.pressEventCamera, out localPointerPosition))
            {

                // 移动位置 = 本地鼠标当前位置 - 本地鼠标起点位置  
                Vector3 offsetToOriginal = localPointerPosition - m_mouseStartPoint;
                // 当前面板位置 = 面板起点 + 移动位置  
                m_panelRect.localPosition = m_PanelStartPoint + offsetToOriginal;
            }
            ClampToWindow();
        }

        // 限制当前面板在父节点中的区域位置  
        protected void ClampToWindow()
        {
            Vector3 pos = m_panelRect.localPosition;
            Vector3 minPosition = m_rootRect.rect.min - m_panelRect.rect.min;
            Vector3 maxPosition = m_rootRect.rect.max - m_panelRect.rect.max;
            pos.x = Mathf.Clamp(m_panelRect.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(m_panelRect.localPosition.y, minPosition.y, maxPosition.y);
            m_panelRect.localPosition = pos;
        }
    }
}
