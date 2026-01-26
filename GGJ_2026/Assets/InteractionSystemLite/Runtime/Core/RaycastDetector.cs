using System;
using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Performs a forward raycast from a camera (e.g., FPS/TPS) to find IInteractable targets.
    /// </summary>
    public class RaycastDetector : MonoBehaviour
    {
        [Header("Raycast Settings")]
        public Camera sourceCamera;
        [SerializeField] private float maxDistance = 30f;
        [SerializeField] private LayerMask interactableLayers = ~0;
        [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;

        private IInteractable currentInteractable;

        public event Action<IInteractable> OnInteractableFound;
        public event Action<IInteractable> OnInteractableLost;

        private void Awake()
        {
            if (sourceCamera == null)
            {
                sourceCamera = Camera.main;
            }
        }

        private void Update()
        {
            IInteractable hitInteractable = RaycastForInteractable();

            if (!ReferenceEquals(hitInteractable, currentInteractable))
            {
                // Lost previous
                if (currentInteractable != null)
                {
                    OnInteractableLost?.Invoke(currentInteractable);
                }

                currentInteractable = hitInteractable;

                // Found new
                if (currentInteractable != null)
                {
                    OnInteractableFound?.Invoke(currentInteractable);
                }
            }
        }

        private IInteractable RaycastForInteractable()
        {
            if (sourceCamera == null) return null;

            Ray ray = new Ray(sourceCamera.transform.position, sourceCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayers, triggerInteraction))
            {
                // Look for IInteractable on the hit object or its parents
                return hit.collider.GetComponentInParent<IInteractable>();
            }

            return null;
        }

        public IInteractable GetCurrentInteractable()
        {
            return currentInteractable;
        }
    }
}
