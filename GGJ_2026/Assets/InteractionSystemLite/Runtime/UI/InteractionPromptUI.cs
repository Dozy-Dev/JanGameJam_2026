using TMPro;
using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Simple prompt UI using uGUI + TextMeshPro.
    /// Shows the current interactable name and key hint.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        [Header("References")]
        public InteractionController interactionController;
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI promptLabel;

        [Header("Appearance")]
        [SerializeField] private string format = "[E] {0}";
        [SerializeField] private float fadeSpeed = 10f;

        private float _targetAlpha;

        private void Reset()
        {
            canvasGroup = GetComponentInChildren<CanvasGroup>();
            promptLabel = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            if (interactionController == null)
            {
                interactionController = FindFirstObjectByType<InteractionController>();
            }

            if (interactionController != null)
            {
                interactionController.OnInteractableFound += OnFound;
                interactionController.OnInteractableLost += OnLost;
            }

            SetVisible(false, true);
        }

        private void OnDisable()
        {
            if (interactionController != null)
            {
                interactionController.OnInteractableFound -= OnFound;
                interactionController.OnInteractableLost -= OnLost;
            }
        }

        private void Update()
        {
            if (canvasGroup == null) return;

            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, _targetAlpha, fadeSpeed * Time.deltaTime);
        }

        private void OnFound(IInteractable interactable)
        {
            if (promptLabel != null && interactable != null)
            {
                string name = interactable.GetInteractionName();
                promptLabel.text = string.Format(format, name);
            }

            SetVisible(true);
        }

        private void OnLost(IInteractable _)
        {
            SetVisible(false);
        }

        private void SetVisible(bool visible, bool instant = false)
        {
            _targetAlpha = visible ? 1f : 0f;

            if (canvasGroup != null && instant)
            {
                canvasGroup.alpha = _targetAlpha;
            }
        }
    }
}
