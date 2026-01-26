#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractionSystemLite.Editor
{
    public class SetupWizard : EditorWindow
    {
        [MenuItem("Tools/Interaction System Lite/Setup Wizard")]
        public static void OpenWindow()
        {
            var wnd = GetWindow<SetupWizard>();
            wnd.titleContent = new GUIContent("Interaction System Setup");
            wnd.minSize = new Vector2(480, 520);
        }
        public void CreateGUI()
        {
            string uxmlPath = "Assets/InteractionSystemLite/Editor/SetupUI/SetupWizard.uxml";
            string ussPath = "Assets/InteractionSystemLite/Editor/SetupUI/SetupWizard.uss";

            // Load UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            if (visualTree == null)
            {
                Debug.LogError("SetupWizard: Failed to load UXML at path: " + uxmlPath);
                EditorUtility.DisplayDialog("Setup Wizard Error",
                    "Could not load SetupWizard.uxml.\n\nCheck that the file exists at:\n" + uxmlPath,
                    "OK");
                return;
            }

            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            if (styleSheet == null)
            {
                Debug.LogError("SetupWizard: Failed to load USS at path: " + ussPath);
                EditorUtility.DisplayDialog("Setup Wizard Error",
                    "Could not load SetupWizard.uss.\n\nCheck that the file exists at:\n" + ussPath,
                    "OK");
                return;
            }

            // Load layout
            var treeInstance = visualTree.Instantiate();
            rootVisualElement.Add(treeInstance);

            // Apply style
            rootVisualElement.styleSheets.Add(styleSheet);

            // ------------------------------
            // BUTTON HOOKUP
            // ------------------------------
            rootVisualElement.Q<Button>("AddInteractionControllerBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.AddInteractionControllerToSelected();
            });

            rootVisualElement.Q<Button>("AddPromptUIBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.CreateUGUIPrompt();
            });

            rootVisualElement.Q<Button>("AddRaycastBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.AddRaycastDetector();
            });

            rootVisualElement.Q<Button>("AddProximityBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.AddProximityDetector();
            });

            rootVisualElement.Q<Button>("CreateButtonPrefabBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.CreateButtonPrefab();
            });

            rootVisualElement.Q<Button>("CreateDoorPrefabBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.CreateDoorPrefab();
            });

            rootVisualElement.Q<Button>("CreatePickupPrefabBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                SetupActions.CreatePickupPrefab();
            });

            rootVisualElement.Q<Button>("ImportButtonDemoBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                DemoSceneImporter.ImportButtonDemo();
            });

            rootVisualElement.Q<Button>("ImportDialogueDemoBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                DemoSceneImporter.ImportDialogueDemo();
            });

            rootVisualElement.Q<Button>("OpenDocsBtn")?.RegisterCallback<ClickEvent>(evt =>
            {
                Application.OpenURL("https://yourdocs.com");
            });
        }

    }
}
#endif
