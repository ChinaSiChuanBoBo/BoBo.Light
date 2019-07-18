namespace BoBo.Light.Base
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    //
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
    //
    internal static class Messenger
    {
        private static Dictionary<int, Dictionary<int, Delegate>> m_eventTable
            = new Dictionary<int, Dictionary<int, Delegate>>();

        //    #region  异常检查
        private static void OnListenerAdding(int cmdLevel, int eventID, Delegate listenerBeingAdded)
        {
            if (!m_eventTable.ContainsKey(cmdLevel))
            {
                var eventList = new Dictionary<int, Delegate>();
                m_eventTable.Add(cmdLevel, eventList);
            }


            Delegate d;
            m_eventTable[cmdLevel].TryGetValue(eventID, out d);
            if (null == d)
            {
                m_eventTable[cmdLevel].Add(eventID, null);
            }
            else if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format(
                 @"Attempting to add listener with inconsistent signature for event type {0}.
                  Current listeners have type {1} and listener being added has type {2}",
                    eventID, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }

        }

        private static bool OnListenerRemoving(int cmdLevel, int eventID, Delegate listenerBeingRemoved)
        {
            if (m_eventTable.ContainsKey(cmdLevel))
            {
                Delegate d;
                m_eventTable[cmdLevel].TryGetValue(eventID, out d);
                if (null == d)
                    return false;
                else
                    if (d.GetType() != listenerBeingRemoved.GetType())
                    {
                        throw new ListenerException(string.Format(
                           @"Attempting to remove listener with inconsistent signature for event type {0}.
                  Current listeners have type {1} and listener being removed has type {2}"
                           , eventID, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                    }
                return true;
            }
            return false;
        }

        private static void OnListenerRemoved(int cmdLevel, int eventID)
        {
            if (m_eventTable[cmdLevel][eventID] == null)
            {
                m_eventTable[cmdLevel].Remove(eventID);
                if (m_eventTable[cmdLevel].Count <= 0)
                    m_eventTable.Remove(cmdLevel);
            }
        }

        private static bool OnBroadcasting(int cmdLevel, int eventID)
        {
            return (m_eventTable.ContainsKey(cmdLevel) && m_eventTable[cmdLevel].ContainsKey(eventID));
        }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }

        #region AddListener
        //No parameters
        static public void AddListener(int cmdLevel, int eventID, Callback handler)
        {
            OnListenerAdding(cmdLevel, eventID, handler);
            m_eventTable[cmdLevel][eventID] = (Callback)m_eventTable[cmdLevel][eventID] + handler;
        }

        //Single parameter
        static public void AddListener<T>(int cmdLevel, int eventID, Callback<T> handler)
        {
            OnListenerAdding(cmdLevel, eventID, handler);
            m_eventTable[cmdLevel][eventID] = (Callback<T>)m_eventTable[cmdLevel][eventID] + handler;
        }

        //Two parameters
        static public void AddListener<T, U>(int cmdLevel, int eventID, Callback<T, U> handler)
        {
            OnListenerAdding(cmdLevel, eventID, handler);
            m_eventTable[cmdLevel][eventID] = (Callback<T, U>)m_eventTable[cmdLevel][eventID] + handler;
        }

        //Three parameters
        static public void AddListener<T, U, V>(int cmdLevel, int eventID, Callback<T, U, V> handler)
        {
            OnListenerAdding(cmdLevel, eventID, handler);
            m_eventTable[cmdLevel][eventID] = (Callback<T, U, V>)m_eventTable[cmdLevel][eventID] + handler;
        }
        #endregion

        #region RemoveListener
        //No parameters
        static public void RemoveListener(int cmdLevel, int eventID, Callback handler)
        {
            if (OnListenerRemoving(cmdLevel, eventID, handler))
            {
                m_eventTable[cmdLevel][eventID] = (Callback)m_eventTable[cmdLevel][eventID] - handler;
                OnListenerRemoved(cmdLevel, eventID);
            }
        }
        //Single parameter
        static public void RemoveListener<T>(int cmdLevel, int eventID, Callback<T> handler)
        {
            if (OnListenerRemoving(cmdLevel, eventID, handler))
            {
                m_eventTable[cmdLevel][eventID] = (Callback<T>)m_eventTable[cmdLevel][eventID] - handler;
                OnListenerRemoved(cmdLevel, eventID);
            }
        }
        //Two parameters
        static public void RemoveListener<T, U>(int cmdLevel, int eventID, Callback<T, U> handler)
        {
            if (OnListenerRemoving(cmdLevel, eventID, handler))
            {
                m_eventTable[cmdLevel][eventID] = (Callback<T, U>)m_eventTable[cmdLevel][eventID] - handler;
                OnListenerRemoved(cmdLevel, eventID);
            }
        }
        //Three parameters
        static public void RemoveListener<T, U, V>(int cmdLevel, int eventID, Callback<T, U, V> handler)
        {
            if (OnListenerRemoving(cmdLevel, eventID, handler))
            {
                m_eventTable[cmdLevel][eventID] = (Callback<T, U, V>)m_eventTable[cmdLevel][eventID] - handler;
                OnListenerRemoved(cmdLevel, eventID);
            }
        }
        #endregion

        #region Broadcast
        //No parameters
        static public void Broadcast(int cmdLevel, int eventID)
        {
            if (OnBroadcasting(cmdLevel, eventID))
            {
                Callback callback = m_eventTable[cmdLevel][eventID] as Callback;
                if (null != callback)
                {
                    callback();
                }
            }
        }
        //Single parameter
        static public void Broadcast<T>(int cmdLevel, int eventID, T arg1)
        {
            if (OnBroadcasting(cmdLevel, eventID))
            {
                Callback<T> callback = m_eventTable[cmdLevel][eventID] as Callback<T>;
                if (null != callback)
                {
                    callback(arg1);
                }
            }
        }
        //Two parameters
        static public void Broadcast<T, U>(int cmdLevel, int eventID, T arg1, U arg2)
        {
            if (OnBroadcasting(cmdLevel, eventID))
            {
                Callback<T, U> callback = m_eventTable[cmdLevel][eventID] as Callback<T, U>;
                if (null != callback)
                {
                    callback(arg1, arg2);
                }
            }
        }
        //Three parameters
        static public void Broadcast<T, U, V>(int cmdLevel, int eventID, T arg1, U arg2, V arg3)
        {
            if (OnBroadcasting(cmdLevel, eventID))
            {
                Callback<T, U, V> callback = m_eventTable[cmdLevel][eventID] as Callback<T, U, V>;
                if (null != callback)
                {
                    callback(arg1, arg2, arg3);
                }
            }
        }
        #endregion
    }
}