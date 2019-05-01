namespace BoBo.Light.Base
{
    using UnityEngine;
    using System.Collections;
    using System.Reflection;
    using System;

    /// <summary>
    /// 单例模板类
    /// </summary>
    /// <typeparam name="T">参数是类类型，并且只能使用无参构造来实例化</typeparam>
    public abstract class SingletonObject<T> where T : class
    {
        protected static T m_instance = null;
        private static object m_lockSingleton = new object();

        protected SingletonObject()
        {
            if (null != m_instance)
                throw new System.Exception("this " +
                    (typeof(T)).ToString() + "singleton Instance constructed more once !!!");
            Init();
        }

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
                            ConstructorInfo ci
                                = typeof(T).GetConstructor(BindingFlags.NonPublic |
                                BindingFlags.Instance, null, new Type[0], null);
                            if (ci == null)
                            { throw new InvalidOperationException("class must contain a private constructor"); }
                            m_instance = (T)ci.Invoke(null);
                        }
                    }
                }
                return m_instance;
            }
        }

        protected virtual void Init() { }
    }
}
