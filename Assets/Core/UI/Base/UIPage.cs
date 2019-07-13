namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using BoBo.Light.Base;
    public abstract class UIPage : UIComponent
    {

        public BaseView ViewRoot
        {
            get
            {
                return uiRoot;
            }
        }

        internal BaseView uiRoot;

        internal void Pop(object param, object extra)
        {
            this.gameObject.SetActive(true);
            OnPopup(param, extra);
        }

        internal void Hide()
        {
            OnHide();
            this.gameObject.SetActive(false);
        }



        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(m_pageName))
                    m_pageName = this.gameObject.name.ToLower();    //转换为小写
                return m_pageName;
            }
        }


        public bool PopPage(string pageName, object param = null, object extra = null, bool hidePre = true)
        {
            return uiRoot.PopPage(pageName, param, extra, hidePre);
        }

        public bool HidePage(string pageName)
        {
            return uiRoot.HidePage(pageName);
        }

        protected virtual void OnPopup(object param, object extra)
        {

        }

        protected virtual void OnHide()
        {

        }

        private string m_pageName = "";
    }
}
