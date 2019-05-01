namespace BoBo.Light.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PureMVC.Patterns;
    using System;

    public abstract class UIFacade : Facade
    {
        public abstract IList<string> ViewNames
        {
            get;
        }

        public UIFacade()
        {

        }


        protected virtual void OnInit()
        {

        }

        protected virtual void OnDestroy()
        {

        }

        internal void Init(object param, object extra)
        {
            for (int i = 0; i < ViewNames.Count; ++i)
            {
                UIRes.Instance.OpenUI(ViewNames[i], param, extra);
            }
            OnInit();
        }

        internal void Destroy()
        {
            for (int i = 0; i < ViewNames.Count; ++i)
            {
                UIRes.Instance.CloseUI(ViewNames[i]);
            }
            OnDestroy();
        }

        public static void NewFacade<T>(object param = null, object extra = null) where T : UIFacade
        {
            System.Type type = typeof(T);
            if (!uiFacades.ContainsKey(type))
            {
                //通过反射，创建实例
                UIFacade uiFacade = Activator.CreateInstance(type, true) as UIFacade;
                uiFacade.Init(param, extra);
                uiFacades.Add(type, uiFacade);
            }
        }

        public static bool FacadeIsExist(System.Type facadeType)
        {
            if (uiFacades.ContainsKey(facadeType))
                return true;
            else
                return false;
        }

        public static void RemoveFacade<T>() where T : UIFacade
        {
            System.Type type = typeof(T);
            UIFacade uiFacade;
            uiFacades.TryGetValue(type, out uiFacade);
            if (null != uiFacade)
            {
                uiFacade.Destroy();
                uiFacades.Remove(type);
                uiFacade = null;
            }
        }

        public static void RemoveAllFacade()
        {
            if (uiFacades.Count > 0)
            {
                foreach (UIFacade facadeItem in uiFacades.Values)
                {
                    facadeItem.Destroy();
                }
                uiFacades.Clear();
            }
        }

        public static void TouchUIAtPresent(int layerMask)
        {
            UIRes.Instance.TouchUIAtPresent(layerMask);
        }

        public static Vector3 ScreenPointToWorldPointInRectangle(Vector2 screenPos)
        {
            return UIRes.Instance.ScreenPointToWorldPointInRectangle(screenPos);
        }

        protected static Dictionary<Type, UIFacade> uiFacades = new Dictionary<Type, UIFacade>();
    }
}

