namespace BoBo.Light.Base
{
    using UnityEngine;
    using System.Collections;

    public class MonoModule : MonoBehaviour
    {
        private static GameObject m_root = null;

        private static object m_lockRoot = new object();

        public static void LoadModule<T>() where T : MonoModule
        {
            lock (m_lockRoot)
            {
                if (null == m_root)
                {
                    m_root = new GameObject("ModuleGameObject");
                    DontDestroyOnLoad(m_root);
                }
                var module = m_root.GetComponent<T>();
                if (null == module)
                {
                    m_root.AddComponent<T>().OnInit();
                }
            }
        }

        public static void UnLoadModule<T>() where T : MonoModule
        {
            lock (m_lockRoot)
            {
                if (null == m_root)
                    return;

                var module = m_root.GetComponent<T>();
                if (null != module)
                {
                    module.OnRelease();
                    GameObject.Destroy(module);
                }
            }
        }

        public static T Find<T>() where T : MonoModule
        {
            if (null != m_root)
            {
                return m_root.GetComponent<T>();
            }
            return null;
        }

        //模块加载时调用
        protected virtual void OnInit() { }
        //模块卸载时回调
        protected virtual void OnRelease() { }
    }
}
