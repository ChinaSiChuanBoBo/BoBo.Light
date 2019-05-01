namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using BoBo.Light.Base;
    public abstract class UIPage : UIComponent
    {
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


        //public bool PageActivity
        //{
        //    get
        //    {
        //        return this.gameObject.activeSelf;
        //    }
        //}

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(m_pageName))
                    m_pageName = this.gameObject.name.ToLower();    //转换为小写
                return m_pageName;
            }
        }

        //public virtual void SetPage(int key, object data, object extra)
        //{

        //}

        //public void SetPage(string pageName, int key, object data, object extra)
        //{
        //    uiRoot.SetPage(pageName, key, data, extra);
        //}

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
