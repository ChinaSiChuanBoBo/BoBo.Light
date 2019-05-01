namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    public class UIFindWnd : EditorWindow
    {
        void OnEnable()
        {

            try
            {
                m_uiInfos = Resources.LoadAll<UIAssetInfo>("UI/UIAssetInfos");
            }
            catch
            {

            }
            if (null == m_uiInfos)
                Debug.LogWarning("没有UI数据!!!");
        }

        void OnGUI()
        {
            m_uiName = EditorGUILayout.TextField("UI Name:", m_uiName);
            //
            if (GUILayout.Button("查找UI"))
            {
                if (null == m_uiInfos)
                    return;

                UIAssetInfo uiInfo = null;
                foreach (UIAssetInfo info in m_uiInfos)
                {
                    if (info.uiID == m_uiName)
                    {
                        uiInfo = info;
                        break;
                    }
                }
                if (null == uiInfo)
                {
                    EditorUtility.DisplayDialog("提示", "查无此UI", "确定");
                    return;
                }

                GameObject uiInstance =
                    GameObject.Instantiate(Resources.Load("UI/Prefabs/" + uiInfo.uiID))
                    as GameObject;
                GameObject canvas = UIToolsEditor.GetCanvas();
                uiInstance.transform.SetParent(canvas.transform, false);
                uiInstance.GetComponent<RectTransform>().sizeDelta = uiInfo.sizeDelta;
                uiInstance.GetComponent<RectTransform>().anchoredPosition3D = uiInfo.anchoredPosition3D;
                uiInstance.name = uiInfo.uiID;
                Debug.LogWarning("克隆成功");
            }
            //
            for (int i = 0; i < m_uiInfos.Length; ++i)
            {
                var infoItem = m_uiInfos[i];
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name=" + infoItem.uiID);
                if (GUILayout.Button("Load"))
                {
                    GameObject uiInstance =
                    GameObject.Instantiate(Resources.Load("UI/Prefabs/" + infoItem.uiID))
                    as GameObject;
                    GameObject canvas = UIToolsEditor.GetCanvas();
                    uiInstance.transform.SetParent(canvas.transform, false);
                    uiInstance.GetComponent<RectTransform>().sizeDelta = infoItem.sizeDelta;
                    uiInstance.GetComponent<RectTransform>().anchoredPosition3D = infoItem.anchoredPosition3D;
                    uiInstance.name = infoItem.uiID;
                    Debug.LogWarning("克隆成功");
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private string m_uiName = "";
        private UIAssetInfo[] m_uiInfos = null;

    }
}
