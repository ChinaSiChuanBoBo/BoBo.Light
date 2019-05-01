namespace BoBo.Light.Base
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
   
    /// <summary>
    /// MonoBehaviour单例模板类
    /// </summary>
    /// <typeparam name="T">约束条件告诉编译器这是一个继承自MonoBehaviour的类 不然AddComponent<T>编译不过</typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_instance = null;

        private static object m_lockSingleton = new object();

        private static GameObject m_root = null;

        public static T Instance
        {
            get
            {
                if (null == m_instance)
                {
                    lock (m_lockSingleton)
                    {
                        if (null == m_instance)
                        {
                            if (null == m_root)
                            {
                                m_root = new GameObject("MonoSingleton");
                                DontDestroyOnLoad(m_root);
                            }
                            m_instance = m_root.GetOrAddComponent<T>();
                            m_instance.Init();
                        }
                    }
                }
                return m_instance;
            }
        }

        protected virtual void Init() { }

        void OnApplicationQuit()
        {
            m_instance = null;
        }
    }
}
