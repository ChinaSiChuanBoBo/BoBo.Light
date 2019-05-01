namespace BoBo.Light.UI
{
    #region using指令
    using UnityEngine;
    using System.Collections;
    using UnityEngine.EventSystems;
    #endregion

    /// <summary>
    /// 触摸事件类型
    /// </summary>
    public enum TouchType
    {
        OnClick = 1,
        OnDoubleClick,
        TouchDown,
        TouchUp,
        TouchEnter,
        TouchExit,
        OnSelect,
        OnUpdateSelect,
        OnDeSelect,
        OnDrag,
        OnDragEnd,
        OnDrop,
        OnScroll,
        OnMove,
        OnLongPress
    }

    //点控操作委托
    public delegate void TouchHandleDelegate(GameObject sender, AbstractEventData eventData, params object[] extras);

    public class EventTriggerListener : MonoBehaviour,
                                       IPointerClickHandler,
                                       IPointerDownHandler,
                                       IPointerEnterHandler,
                                       IPointerExitHandler,
                                       IPointerUpHandler,
                                       ISelectHandler,
                                       IUpdateSelectedHandler,
                                       IDeselectHandler,
                                       IDragHandler,
                                       IEndDragHandler,
                                       IDropHandler,
                                       IScrollHandler,
                                       IMoveHandler
    {

        class TouchHandler
        {
            #region 构造

            public TouchHandler(TouchHandleDelegate handle, params object[] paramArray)
            {
                SetHandle(handle, paramArray);
            }

            public TouchHandler()
            {

            }
            #endregion

            #region 成员方法

            public void SetHandle(TouchHandleDelegate handle, params object[] extras)
            {
                RemoveHandle();
                m_eventHandle += handle;
                this.m_extras = extras;
            }

            public void InvokeHandle(GameObject sender, AbstractEventData args)
            {
                if (null != m_eventHandle)
                {
                    m_eventHandle(sender, args, m_extras);
                }
            }

            public void RemoveHandle()
            {
                if (null != m_eventHandle)
                {
                    m_eventHandle -= m_eventHandle;                
                    m_eventHandle = null;
                }
                m_extras = null;
            }
            #endregion

            #region  Inner 成员变量

            private event TouchHandleDelegate m_eventHandle = null;

            private object[] m_extras;
            #endregion
        }

        public static EventTriggerListener Get(GameObject gameObj)
        {
            return gameObj.GetOrAddComponent<EventTriggerListener>();
        }

        public void AddHandle(TouchType touchType, TouchHandleDelegate handle, params object[] extras)
        {
            switch (touchType)
            {
                case TouchType.OnClick:
                    {
                        if (null == m_onClick)
                            m_onClick = new TouchHandler();
                        m_onClick.SetHandle(handle, extras);
                    } break;

                case TouchType.OnDoubleClick:
                    {
                        if (null == m_onDoubleClick)
                            m_onDoubleClick = new TouchHandler();
                        m_onDoubleClick.SetHandle(handle, extras);
                    } break;
                case TouchType.TouchUp:
                    {
                        if (null == m_onUp)
                            m_onUp = new TouchHandler();
                        m_onUp.SetHandle(handle, extras);
                    } break;
                case TouchType.TouchDown:
                    {
                        if (null == m_onDown)
                            m_onDown = new TouchHandler();
                        m_onDown.SetHandle(handle, extras);
                    } break;
                case TouchType.TouchEnter:
                    {
                        if (null == m_onEnter)
                            m_onEnter = new TouchHandler();
                        m_onEnter.SetHandle(handle, extras);
                    } break;
                case TouchType.TouchExit:
                    {
                        if (null == m_onExit)
                            m_onExit = new TouchHandler();
                        m_onExit.SetHandle(handle, extras);
                    } break;
                case TouchType.OnDrag:
                    {
                        if (null == m_onDrag)
                            m_onDrag = new TouchHandler();
                        m_onDrag.SetHandle(handle, extras);
                    } break;
                case TouchType.OnDragEnd:
                    {
                        if (null == m_onDragEnd)
                            m_onDragEnd = new TouchHandler();
                        m_onDragEnd.SetHandle(handle, extras);
                    } break;
                case TouchType.OnSelect:
                    {
                        if (null == m_onSelect)
                            m_onSelect = new TouchHandler();
                        m_onSelect.SetHandle(handle, extras);
                    } break;
                case TouchType.OnUpdateSelect:
                    {
                        if (null == m_onUpdateSelect)
                            m_onUpdateSelect = new TouchHandler();
                        m_onUpdateSelect.SetHandle(handle, extras);
                    } break;
                case TouchType.OnDeSelect:
                    {
                        if (null == m_onDeSelect)
                            m_onDeSelect = new TouchHandler();
                        m_onDeSelect.SetHandle(handle, extras);
                    } break;
                case TouchType.OnMove:
                    {
                        if (null == m_onMove)
                            m_onMove = new TouchHandler();
                        m_onMove.SetHandle(handle, extras);
                    } break;
                case TouchType.OnScroll:
                    {
                        if (null == m_onScroll)
                            m_onScroll = new TouchHandler();
                        m_onScroll.SetHandle(handle, extras);
                    } break;
                case TouchType.OnDrop:
                    {
                        if (null == m_onDrop)
                            m_onDrop = new TouchHandler();
                        m_onDrop.SetHandle(handle, extras);
                    } break;
            }
        }

        public void RemoveAllHandle()
        {
            RemoveHandle(m_onClick);
            RemoveHandle(m_onDeSelect);
            RemoveHandle(m_onDoubleClick);
            RemoveHandle(m_onDown);
            RemoveHandle(m_onDrag);
            RemoveHandle(m_onDragEnd);
            RemoveHandle(m_onDrop);
            RemoveHandle(m_onEnter);
            RemoveHandle(m_onExit);
            RemoveHandle(m_onMove);
            RemoveHandle(m_onScroll);
            RemoveHandle(m_onSelect);
            RemoveHandle(m_onUp);
            RemoveHandle(m_onUpdateSelect);
        }

        #region 实现的Touch接口

        public void OnPointerClick(PointerEventData eventData)
        {
            if (null != m_onClick)
                m_onClick.InvokeHandle(this.gameObject, eventData);
        }

        public void OnDoubleClick(PointerEventData eventData)
        {
            if (null != m_onDoubleClick)
                m_onDoubleClick.InvokeHandle(this.gameObject, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (null != m_onDown)
                m_onDown.InvokeHandle(this.gameObject, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != m_onEnter)
                m_onEnter.InvokeHandle(this.gameObject, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != m_onExit)
                m_onExit.InvokeHandle(this.gameObject, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (null != m_onUp)
                m_onUp.InvokeHandle(this.gameObject, eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (null != m_onSelect)
                m_onSelect.InvokeHandle(this.gameObject, eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (null != m_onUpdateSelect)
                m_onUpdateSelect.InvokeHandle(this.gameObject, eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (null != m_onDeSelect)
                m_onDeSelect.InvokeHandle(this.gameObject, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (null != m_onDrag)
                m_onDrag.InvokeHandle(this.gameObject, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (null != m_onDragEnd)
                m_onDragEnd.InvokeHandle(this.gameObject, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (null != m_onDrop)
                m_onDrop.InvokeHandle(this.gameObject, eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (null != m_onScroll)
                m_onScroll.InvokeHandle(this.gameObject, eventData);
        }

        public void OnMove(AxisEventData eventData)
        {
            if (null != m_onMove)
                m_onMove.InvokeHandle(this.gameObject, eventData);
        }
        #endregion

        #region  Unity 回调

        void OnDestroy()
        {
            RemoveAllHandle();
        }
        #endregion

        #region Inner Variable

        private TouchHandler m_onClick;
        private TouchHandler m_onDoubleClick;
        private TouchHandler m_onDown;
        private TouchHandler m_onEnter;
        private TouchHandler m_onExit;
        private TouchHandler m_onUp;
        private TouchHandler m_onSelect;
        private TouchHandler m_onUpdateSelect;
        private TouchHandler m_onDeSelect;
        private TouchHandler m_onDrag;
        private TouchHandler m_onDragEnd;
        private TouchHandler m_onDrop;
        private TouchHandler m_onScroll;
        private TouchHandler m_onMove;
        #endregion

        #region Inner Function

        private void RemoveHandle(TouchHandler handle)
        {
            if (null != handle)
            {
                handle.RemoveHandle();
                handle = null;
            }
        }
        #endregion
    }
}

