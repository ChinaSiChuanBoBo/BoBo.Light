namespace BoBo.Light.UI
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public class UIEditorWnd : EditorWindow
    {
        void OnGUI()
        {
            m_uiInstance = EditorGUILayout.ObjectField("UI Instance", m_uiInstance, typeof(BaseView), true) as BaseView;

            if (GUILayout.Button("加入UI"))
            {
                #region 加入UI

                if (null == m_uiInstance)
                {
                    Debug.LogError("UI 为Null");
                    return;
                }
                Directory.CreateDirectory(Application.dataPath + "/Resources/UI/UIAssetInfos");
                UIAssetInfo[] infos = Resources.LoadAll<UIAssetInfo>("UI/UIAssetInfos");
                if (null != infos)
                {
                    foreach (UIAssetInfo info in infos)
                    {
                        if (info.uiID == m_uiInstance.GetUiID())
                        {
                            if (EditorUtility.DisplayDialog("提示", "是否更新UI", "OK"))
                                break;
                            return;
                        }
                    }
                }
                UIAssetInfo infoInstance = ScriptableObject.CreateInstance<UIAssetInfo>();
                infoInstance.uiID = m_uiInstance.GetUiID();
                RectTransform rectTrans = m_uiInstance.GetComponent<RectTransform>();
                infoInstance.anchoredPosition3D = rectTrans.anchoredPosition3D;
                infoInstance.sizeDelta = rectTrans.sizeDelta;
                AssetDatabase.CreateAsset(infoInstance, "Assets/Resources/UI/UIAssetInfos"
                     + "/" + infoInstance.uiID + ".asset");
                //调用导出回调
                var uiScriptType = m_uiInstance.GetType();
                MethodInfo[] methodInfos =
                  uiScriptType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (null != methodInfos || methodInfos.Length > 0)
                {
                    foreach (MethodInfo methodInfo_Item in methodInfos)
                    {
                        if (methodInfo_Item.IsDefined(typeof(ExportRecoverAttribute), false))
                        {
                            methodInfo_Item.Invoke(m_uiInstance, null);
                        }
                    }
                }

                UIPage[] pages = m_uiInstance.GetComponentsInChildren<UIPage>(true);
                if (null != pages && pages.Length > 0)
                {
                    foreach (UIPage pageInstance in pages)
                    {
                        var pageScriptType = pageInstance.GetType();
                        MethodInfo[] methodInfos2 =
                 pageScriptType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (null != methodInfos2 || methodInfos2.Length > 0)
                        {
                            foreach (MethodInfo methodInfo_Item in methodInfos2)
                            {
                                if (methodInfo_Item.IsDefined(typeof(ExportRecoverAttribute), false))
                                {
                                    methodInfo_Item.Invoke(pageInstance, null);
                                }
                            }
                        }
                        pageInstance.gameObject.name = pageScriptType.Name;
                        pageInstance.gameObject.SetActive(false);
                       
                    }
                }
                //
                m_uiInstance.name = m_uiInstance.GetUiID();
                m_uiInstance.gameObject.SetActive(false);
                //保存预设
                Directory.CreateDirectory(Application.dataPath + "/Resources/UI/Prefabs");
                PrefabUtility.CreatePrefab(
                    UIToolsEditor.prefabPathAtEditor + "/" +
                    m_uiInstance.GetUiID() + ".prefab", m_uiInstance.gameObject, ReplacePrefabOptions.Default);
                PrefabUtility.DisconnectPrefabInstance(m_uiInstance);
                AssetDatabase.Refresh();
                Debug.LogWarning("加入UI成功");

                #endregion
            }

            if (GUILayout.Button("移除UI"))
            {
                #region 移除UI

                if (null == m_uiInstance)
                {
                    Debug.LogError("UI 为Null");
                    return;
                }

                Directory.CreateDirectory(Application.dataPath + "/Resources/UI/UIAssetInfos");
                UIAssetInfo[] infos = Resources.LoadAll<UIAssetInfo>("UI/UIAssetInfos");
                if (null != infos)
                {
                    foreach (UIAssetInfo info in infos)
                    {
                        if (info.uiID == m_uiInstance.GetUiID())
                        {
                            AssetDatabase.DeleteAsset("Assets/Resources/UI/UIAssetInfos" + "/" + info.uiID + ".asset");
                            AssetDatabase.Refresh();
                            break;
                        }
                    }
                }

                try
                {
                    AssetDatabase.DeleteAsset(UIToolsEditor.prefabPathAtEditor
                        + "/" + m_uiInstance.GetUiID() + ".prefab");
                }
                catch { };
                AssetDatabase.Refresh();
                Debug.LogWarning("删除UI成功");
                #endregion
            }
        }

        private BaseView m_uiInstance = null;

    }
}
