#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using InteractionSystemLite;

[CustomEditor(typeof(ProximityDetector))]
public class ProximityDetectorGizmo : Editor
{
    private void OnSceneGUI()
    {
        ProximityDetector detector = (ProximityDetector)target;
        SphereCollider sphere = detector.GetComponent<SphereCollider>();
        if (sphere == null) return;

        // Determine world center
        Vector3 center = sphere.transform.position + sphere.center;

        // Determine actual world radius (accounts for scaling)
        float radius = sphere.radius * Mathf.Max(
            sphere.transform.lossyScale.x,
            sphere.transform.lossyScale.y,
            sphere.transform.lossyScale.z
        );

        // Set gizmo color
        Handles.color = new Color(0f, 0.6f, 1f, 1f);

        // Draw simple wireframe sphere
        Handles.DrawWireDisc(center, Vector3.up, radius);
        Handles.DrawWireDisc(center, Vector3.right, radius);
        Handles.DrawWireDisc(center, Vector3.forward, radius);
    }
}
#endif
