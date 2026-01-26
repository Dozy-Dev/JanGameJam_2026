using System;
using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Central interaction manager.
    /// Listens to one or more detectors and fires interact when input is pressed.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        [Header("Detector Sources")]
        [Tooltip("Optional raycast detector for FPS/TPS style interactions.")]
        [SerializeField] private RaycastDetector raycastDetector;

        [Tooltip("Optional proximity detector for trigger-based interactions.")]
        [SerializeField] private ProximityDetector proximityDetector;

        [Header("Interaction Settings")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [Tooltip("Optional: the GameObject considered as the 'interactor' (e.g., player root). " +
                 "If null, defaults to this GameObject.")]
        [SerializeField] private GameObject interactor;

        private IInteractable _current;

        public event Action<IInteractable> OnInteractableFound;
        public event Action<IInteractable> OnInteractableLost;
        public event Action<IInteractable> OnInteractPerformed;

        private InteractionHighlighter _currentHighlighter;


        private void Awake()
        {
            if (interactor == null)
                interactor = gameObject;
        }

        private void OnEnable()
        {
            if (raycastDetector != null)
            {
                raycastDetector.OnInteractableFound += HandleFound;
                raycastDetector.OnInteractableLost += HandleLost;
            }

            if (proximityDetector != null)
            {
                proximityDetector.OnInteractableFound += HandleFound;
                proximityDetector.OnInteractableLost += HandleLost;
            }
        }

        private void OnDisable()
        {
            if (raycastDetector != null)
            {
                raycastDetector.OnInteractableFound -= HandleFound;
                raycastDetector.OnInteractableLost -= HandleLost;
            }

            if (proximityDetector != null)
            {
                proximityDetector.OnInteractableFound -= HandleFound;
                proximityDetector.OnInteractableLost -= HandleLost;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                TriggerInteract();
            }
        }

        /// <summary>
        /// Call this from the new Input System or UI button to trigger interaction.
        /// </summary>
        public void TriggerInteract()
        {
            if (_current == null || interactor == null)
                return;

            if (_current.CanInteract(interactor))
            {
                _current.Interact(interactor);
                OnInteractPerformed?.Invoke(_current);
            }
        }

        private void HandleFound(IInteractable interactable)
        {
            // If it's the same one, do nothing
            if (ReferenceEquals(_current, interactable))
                return;

            // Turn off highlight on previous
            if (_currentHighlighter != null)
            {
                _currentHighlighter.SetHighlighted(false);
                _currentHighlighter = null;
            }

            _current = interactable;

            // Try to find a highlighter on the new interactable
            if (_current is Component comp && comp != null)
            {
                _currentHighlighter = comp.GetComponentInChildren<InteractionHighlighter>();
                if (_currentHighlighter != null)
                {
                    _currentHighlighter.SetHighlighted(true);
                }
            }

            OnInteractableFound?.Invoke(_current);
        }

        private void HandleLost(IInteractable interactable)
        {
            // Only react if we’re losing the *current* interactable
            if (!ReferenceEquals(_current, interactable))
                return;

            // Turn off highlight
            if (_currentHighlighter != null)
            {
                _currentHighlighter.SetHighlighted(false);
                _currentHighlighter = null;
            }

            OnInteractableLost?.Invoke(_current);
            _current = null;
        }

        /// <summary>
        /// Returns the interactable currently in focus (may be null).
        /// </summary>
        public IInteractable GetCurrentInteractable()
        {
            return _current;
        }
    }
}
