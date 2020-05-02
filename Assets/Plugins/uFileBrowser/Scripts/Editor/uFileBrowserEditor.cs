using UnityEngine;
using UnityEditor;

namespace uFileBrowser
{
    [CustomEditor(typeof(FileBrowser))]
    public class uFileBrowserEditor : Editor
    {
        [SerializeField]
        bool show = false;
        [SerializeField]
        bool uiFoldout = false;
        [SerializeField]
        bool iconFoldout = false;

        public override void OnInspectorGUI()
        {
            Color original = GUI.color;
            serializedObject.Update();
            FileBrowser browser = serializedObject.targetObject as FileBrowser;

            if (browser.window)
                show = browser.window.activeInHierarchy;

            EditorGUILayout.Space();
            GUI.color = new Color(0.85f, 1f, 0.85f) * original;
            if (GUILayout.Button("Show/Hide", GUILayout.MaxWidth(120f)))
            {
                show = !show;
                if (browser.overlay)
                    browser.overlay.SetActive(show);
                if (browser.window)
                    browser.window.SetActive(show);
            }
            GUI.color = original;
            EditorGUILayout.Space();

            DrawProperty("defaultPath", "Default Path:");
            GUILayout.Space(8f);
            Rect r = GUILayoutUtility.GetRect(150, 20, GUIStyle.none);
            GUI.Label(r, new GUIContent("Mode:", "Selection mode"));
            string modeText = browser.selectDirectory == false ? "File" : "Directory";
            if (GUI.Button(new Rect(65, r.y - 2f, 70, 20), modeText))
            {
                BeginProperty("selectDirectory");
                browser.selectDirectory = !browser.selectDirectory;
                EndProperty();
            }
            if (browser.selectDirectory)
            {
                DrawProperty("showFiles", "Show Files", "Show files in directory selection mode?");
            }
            if (browser.showFiles || !browser.selectDirectory)
            {
                DrawProperty("fileFormat", "Format filter:", "Example: png|jpg|gif");
            }
            DrawProperty("canCancel", "Can Cancel", "Can the user cancel selection?");
            DrawProperty("searchCaseSensitive", "Search is case sensitive");
            EditorGUILayout.Space();
            uiFoldout = EditorGUILayout.Foldout(uiFoldout, "UI Elements");
            if (uiFoldout)
            {
                DrawProperty("overlay", "Overlay");
                DrawProperty("window", "Window");
                DrawProperty("fileButtonPrefab", "File Button Prefab");
                DrawProperty("directoryButtonPrefab", "Directory Button Prefab");
                DrawProperty("fileContent", "Files Scroll Content");
                DrawProperty("dirContent", "Directory Tree Content");
                DrawProperty("currentPathField", "Current Directory Field");
                DrawProperty("searchField", "Search Field");
                DrawProperty("searchCancelButton", "Search Cancel Button");
                DrawProperty("cancelButton", "CancelButton");
            }
            EditorGUILayout.Space();
            iconFoldout = EditorGUILayout.Foldout(iconFoldout, "Icons");
            if (iconFoldout)
            {
                DrawProperty("folderIcon", "Folder Icon:");
                DrawProperty("defaultIcon", "Default File Icon:");
                EditorGUI.indentLevel++;
                DrawProperty("fileIcons", "Extension Icons");
            }
        }

        void DrawProperty(string name, string label, string tooltip = null)
        {
            SerializedProperty prop = serializedObject.FindProperty(name);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(prop, new GUIContent(label, tooltip), true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            //EditorGUIUtility.LookLikeControls();
        }

        void BeginProperty(string name)
        {
            EditorGUI.BeginChangeCheck();
        }
        void EndProperty()
        {
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}