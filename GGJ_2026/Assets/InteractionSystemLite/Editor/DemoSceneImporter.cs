#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

namespace InteractionSystemLite.Editor
{
    /// <summary>
    /// Imports curated demo scenes from the Samples folder into the user's Assets
    /// and opens them.
    /// </summary>
    public static class DemoSceneImporter
    {
        // Your current development layout:
        // Assets/InteractionSystemLite/Samples/ButtonDemo/ButtonDemo.unity
        private const string DevSamplesRoot = "Assets/InteractionSystemLite/Samples/";

        // Optional: future UPM-style roots for when this becomes a package
        private static readonly string[] UpmRoots =
        {
            "Packages/com.interactionsystemlite/Samples~/",
            "Packages/com.interactionsystemlite/Samples/",
            "Packages/com.yourcompany.interactionsystemlite/Samples~/",
            "Packages/com.yourcompany.interactionsystemlite/Samples/"
        };

        // --------------------------------------------------------------------
        // Public entry points (called from Setup Wizard)
        // --------------------------------------------------------------------
        public static void ImportButtonDemo()
        {
            ImportScene("ButtonDemo/ButtonDemo.unity");
        }

        public static void ImportDialogueDemo()
        {
            ImportScene("DialogueDemo/DialogueDemo.unity");
        }

        public static void ImportDoorDemo()
        {
            ImportScene("DoorDemo/DoorDemo.unity");
        }

        // --------------------------------------------------------------------
        // Core import logic
        // --------------------------------------------------------------------
        private static void ImportScene(string relativePath)
        {
            string sourcePath = FindScene(relativePath);

            if (string.IsNullOrEmpty(sourcePath))
            {
                Debug.LogError("Could not locate sample scene: " + relativePath);
                return;
            }

            // Folder where imported copies go in the user's project
            const string targetFolder = "Assets/InteractionSystemLite_ImportedSamples/";

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            string fileName = Path.GetFileName(relativePath);
            string targetPath = Path.Combine(targetFolder, fileName);

            File.Copy(sourcePath, targetPath, true);
            AssetDatabase.Refresh();

            var scene = EditorSceneManager.OpenScene(targetPath, OpenSceneMode.Single);
            EditorSceneManager.MarkSceneDirty(scene);

            Debug.Log("Imported sample scene successfully: " + targetPath);
        }

        // --------------------------------------------------------------------
        // Path resolution
        // --------------------------------------------------------------------
        /// <summary>
        /// Attempts to locate the scene file by trying known roots, then falls
        /// back to a deep search.
        /// </summary>
        private static string FindScene(string relativePath)
        {
            // 1) Your current dev layout (this is the one you confirmed)
            string devPath = Path.Combine(DevSamplesRoot, relativePath);
            if (File.Exists(devPath))
                return devPath;

            // 2) Future UPM-style package locations
            foreach (var root in UpmRoots)
            {
                string path = Path.Combine(root, relativePath);
                if (File.Exists(path))
                    return path;
            }

            // 3) Fallback: deep search in Assets
            string fileName = Path.GetFileName(relativePath);
            string[] matches = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);

            if (matches.Length > 0)
                return matches[0];

            return null;
        }
    }
}
#endif
