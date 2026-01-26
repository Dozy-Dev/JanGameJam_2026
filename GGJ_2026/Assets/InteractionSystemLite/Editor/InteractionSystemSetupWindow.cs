#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace InteractionSystemLite.Editor
{
    public class InteractionSystemSetupWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Interaction System Lite – Setup";

        [MenuItem("Tools/Interaction System Lite/Setup Wizard - old")]
        public static void OpenWindow()
        {
            var window = GetWindow<InteractionSystemSetupWindow>(true, WINDOW_TITLE);
            window.minSize = new Vector2(420, 520);
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label("Interaction System Lite – Setup Wizard", EditorStyles.boldLabel);
            GUILayout.Space(6);

            EditorGUILayout.HelpBox(
                "Quickly initialize Interaction System Lite in your scene. " +
                "Use the buttons below to add components or create demo scenes.",
                MessageType.Info);

            GUILayout.Space(10);

            DrawPlayerSetupSection();
            GUILayout.Space(10);

            DrawUISpawnerSection();
            GUILayout.Space(10);

            DrawDemoSceneSection();
            GUILayout.Space(10);

            DrawDocumentationSection();
        }

        // -------------------------------
        // PLAYER SETUP
        // -------------------------------
        private void DrawPlayerSetupSection()
        {
            GUILayout.Label("Player Setup", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Interaction Controller to Selected Object", GUILayout.Height(32)))
            {
                AddInteractionController();
            }

            if (GUILayout.Button("Add Raycast Detector to Selected Object", GUILayout.Height(28)))
            {
                AddRaycastDetector();
            }

            if (GUILayout.Button("Add Proximity Detector to Selected Object", GUILayout.Height(28)))
            {
                AddProximityDetector();
            }
        }

        private void AddInteractionController()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<InteractionSystemLite.InteractionController>())
                    Undo.AddComponent<InteractionSystemLite.InteractionController>(obj);
            }
        }

        private void AddRaycastDetector()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<InteractionSystemLite.RaycastDetector>())
                    Undo.AddComponent<InteractionSystemLite.RaycastDetector>(obj);
            }
        }

        private void AddProximityDetector()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<InteractionSystemLite.ProximityDetector>())
                    Undo.AddComponent<InteractionSystemLite.ProximityDetector>(obj);
            }
        }

        // -------------------------------
        // UI SETUP
        // -------------------------------
        private void DrawUISpawnerSection()
        {
            GUILayout.Label("UI Setup", EditorStyles.boldLabel);

            if (GUILayout.Button("Add uGUI Interaction Prompt to Scene", GUILayout.Height(28)))
            {
                CreateUGUIPrompt();
            }
        }

        private void CreateUGUIPrompt()
        {
            // Ensure Canvas exists
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas", typeof(Canvas));
                canvas = canvasObj.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // Create Prompt UI
            GameObject promptObj = new GameObject("InteractionPromptUI");
            Undo.RegisterCreatedObjectUndo(promptObj, "Create Interaction Prompt UI");

            promptObj.transform.SetParent(canvas.transform, false);

            var cg = promptObj.AddComponent<CanvasGroup>();

            var textObj = new GameObject("PromptText");
            textObj.transform.SetParent(promptObj.transform, false);

            var text = textObj.AddComponent<TextMeshProUGUI>();
            text.fontSize = 28;
            text.alignment = TextAlignmentOptions.Center;
            text.text = "[E] Interact";

            RectTransform rt = text.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.2f);
            rt.anchorMax = new Vector2(0.5f, 0.2f);
            rt.anchoredPosition = Vector2.zero;

            var promptComponent = promptObj.AddComponent<InteractionSystemLite.InteractionPromptUI>();
            promptComponent.canvasGroup = cg;
            promptComponent.promptLabel = text;

            // Try to auto-assign the Interaction Controller
            var controller = Object.FindFirstObjectByType<InteractionSystemLite.InteractionController>();
            if (controller != null)
            {
                promptComponent.interactionController = controller;
            }
        }

        // -------------------------------
        // DEMO SCENES
        // -------------------------------
        private void DrawDemoSceneSection()
        {
            GUILayout.Label("Demo Scenes", EditorStyles.boldLabel);

            if (GUILayout.Button("Import Button Interactable Demo Scene", GUILayout.Height(32)))
            {
                DemoSceneImporter.ImportButtonDemo();
            }

            if (GUILayout.Button("Import Dialogue Interactable Demo Scene", GUILayout.Height(32)))
            {
                DemoSceneImporter.ImportDialogueDemo();
            }

            if (GUILayout.Button("Import Door Interactable Demo Scene", GUILayout.Height(32)))
            {
                DemoSceneImporter.ImportDoorDemo();
            }

        }

        // -------------------------------
        // DOCUMENTATION
        // -------------------------------
        private void DrawDocumentationSection()
        {
            GUILayout.Label("Documentation", EditorStyles.boldLabel);

            if (GUILayout.Button("Open Online Documentation", GUILayout.Height(28)))
            {
                Application.OpenURL("https://your-documentation-url-here.com");
            }
        }
    }
}
#endif
