#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InteractionSystemLite.Samples;

namespace InteractionSystemLite.Editor
{
    public static class SetupActions
    {
        // ------------------------------
        // 1. Add Interaction Controller
        // ------------------------------
        public static void AddInteractionControllerToSelected()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<InteractionController>())
                    Undo.AddComponent<InteractionController>(obj);
            }
        }

        // ------------------------------
        // 2. Add Detectors
        // ------------------------------
        public static void AddDetectorsToSelected()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<RaycastDetector>())
                    Undo.AddComponent<RaycastDetector>(obj);

                if (!obj.GetComponent<ProximityDetector>())
                    Undo.AddComponent<ProximityDetector>(obj);
            }
        }

        // ------------------------------
        // 3. Create Prompt UI (uGUI)
        // ------------------------------
        public static void CreateUGUIPrompt()
        {
            // Ensure canvas
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                var canvasGO = new GameObject("Canvas", typeof(Canvas));
                canvas = canvasGO.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            // Create PromptRoot object
            GameObject root = new GameObject("InteractionPromptUI");
            Undo.RegisterCreatedObjectUndo(root, "Create Prompt UI");
            root.transform.SetParent(canvas.transform, false);

            var cg = root.AddComponent<CanvasGroup>();

            // Create Text
            GameObject textGO = new GameObject("PromptText");
            textGO.transform.SetParent(root.transform, false);

            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 30;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.text = "[E] Interact";

            RectTransform t = tmp.GetComponent<RectTransform>();
            t.anchorMin = new Vector2(0.5f, 0.2f);
            t.anchorMax = new Vector2(0.5f, 0.2f);
            t.anchoredPosition = Vector2.zero;

            // Link runtime script if it exists
            var prompt = root.AddComponent<InteractionPromptUI>();
            prompt.canvasGroup = cg;
            prompt.promptLabel = tmp;

            var controller = Object.FindFirstObjectByType<InteractionController>();
            if (controller != null)
                prompt.interactionController = controller;

            EditorUtility.DisplayDialog("Interaction System Lite",
                "Prompt UI created successfully!", "OK");
        }
        public static void AddRaycastDetector()
        {
            foreach (var obj in Selection.gameObjects)
            {
                if (!obj.GetComponent<RaycastDetector>())
                    Undo.AddComponent<RaycastDetector>(obj);
            }
        }

        public static void AddProximityDetector()
        {
            foreach (var selected in Selection.gameObjects)
            {
                // Check if a ProximityDetector already exists
                var existing = selected.GetComponentInChildren<ProximityDetector>();
                if (existing != null)
                {
                    EditorUtility.DisplayDialog(
                        "Proximity Detector Exists",
                        "A ProximityDetector already exists under: " + selected.name,
                        "OK");
                    continue;
                }

                // Create child
                GameObject child = new GameObject("ProximityDetector");
                Undo.RegisterCreatedObjectUndo(child, "Create ProximityDetector");
                child.transform.SetParent(selected.transform, false);
                child.transform.localPosition = Vector3.zero;

                // 1. Add SphereCollider FIRST
                SphereCollider sphere = child.AddComponent<SphereCollider>();
                sphere.isTrigger = true;
                sphere.radius = 2f;
                Undo.RegisterCreatedObjectUndo(sphere, "Add SphereCollider");

                // 2. Flush Undo to finalize the collider before adding required component scripts
                Undo.FlushUndoRecordObjects();

                // 3. Now safely add the ProximityDetector
                var detector = child.AddComponent<ProximityDetector>();
                Undo.RegisterCreatedObjectUndo(detector, "Add ProximityDetector");

                Debug.Log($"[InteractionSystemLite] Added ProximityDetector under {selected.name}");
            }
        }


        private static void SavePrefab(GameObject go, string fileName)
        {
            string folder = "Assets/InteractionSystemLite/Prefabs/";
            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets/InteractionSystemLite", "Prefabs");

            string assetPath = folder + fileName;
            PrefabUtility.SaveAsPrefabAsset(go, assetPath);

            EditorUtility.DisplayDialog("Interaction System Lite",
                fileName + " created at:\n" + assetPath,
                "OK");
        }

        public static void CreateButtonPrefab()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "ButtonInteractable";

            Undo.RegisterCreatedObjectUndo(go, "Create Button Prefab");

            go.AddComponent<ButtonInteractable>();
            go.AddComponent<InteractionHighlighter>();

            SavePrefab(go, "ButtonInteractable.prefab");
        }
        public static void CreateDoorPrefab()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.localScale = new Vector3(1, 2, 0.2f);
            go.name = "DoorInteractable";

            Undo.RegisterCreatedObjectUndo(go, "Create Door Prefab");

            go.AddComponent<DoorInteractable>();
            go.AddComponent<InteractionHighlighter>();

            SavePrefab(go, "DoorInteractable.prefab");
        }
        public static void CreatePickupPrefab()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "PickupInteractable";

            Undo.RegisterCreatedObjectUndo(go, "Create Pickup Prefab");

            go.AddComponent<PickupInteractable>();
            go.AddComponent<InteractionHighlighter>();

            SavePrefab(go, "PickupInteractable.prefab");
        }

    }
}
#endif
