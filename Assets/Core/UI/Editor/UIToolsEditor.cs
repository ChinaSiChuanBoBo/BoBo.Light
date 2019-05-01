namespace BoBo.Light.UI
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using UnityEditor.SceneManagement;
    public class UIToolsEditor : Editor
    {
        [MenuItem("BoBo.Light/UITools/生成Cavas模板")]
        private static void CreateCavasTemplate()
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (null == canvas)
            {
                canvas = new GameObject("Canvas");
                int uiLayer = LayerMask.NameToLayer("UI");
                canvas.layer = uiLayer;
                Canvas canvasComponent = canvas.AddComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;
                //
                Camera uiCamera = new GameObject("UICamera").AddComponent<Camera>();
                uiCamera.transform.SetParent(canvas.transform, false);
                uiCamera.gameObject.layer = canvas.layer;
                canvasComponent.worldCamera = uiCamera;
                //设置UI相机
                uiCamera.transform.Translate(Vector3.back * 100);
                uiCamera.clearFlags = CameraClearFlags.Depth;
                uiCamera.cullingMask = 0x01 << uiLayer;
                uiCamera.orthographic = true;
                uiCamera.useOcclusionCulling = false;
                uiCamera.allowHDR = false;
                uiCamera.allowMSAA = false;
                //设置屏幕适配
                CanvasScaler scalerComponent = canvas.AddComponent<CanvasScaler>();
                scalerComponent.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scalerComponent.referenceResolution = new Vector2(Screen.width, Screen.height);
                scalerComponent.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scalerComponent.matchWidthOrHeight = 1;
                //
                canvas.AddComponent<GraphicRaycaster>();
                //
                GameObject eventObject = GameObject.Find("EventSystem");
                if (null == eventObject)
                {
                    eventObject = new GameObject("EventSystem");
                    eventObject.AddComponent<EventSystem>();
                    eventObject.AddComponent<StandaloneInputModule>();
                }
                //保存场景
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
        }
        [MenuItem("BoBo.Light/UITools/加入或移除UI")]
        private static void OpenBaseUIEditor()
        {
            EditorWindow.GetWindow<UIEditorWnd>(false, "加入或移除UI", true);
        }
        [MenuItem("BoBo.Light/UITools/查询UI")]
        private static void OpenFindUIWnd()
        {
            EditorWindow.GetWindow<UIFindWnd>(false, "查询UI", true);
        }

        public static GameObject GetCanvas()
        {
            CreateCavasTemplate();
            return GameObject.Find("Canvas");
        }

        public const string prefabPathAtEditor = "Assets/Resources/UI/Prefabs";
    }
}
