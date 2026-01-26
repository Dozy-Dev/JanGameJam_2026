#if UNITY_6000_0_OR_NEWER || UNITY_2021_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractionSystemLite
{
    /// <summary>
    /// Simple prompt UI using UI Toolkit.
    /// Requires a UIDocument with a Label named "InteractionPromptLabel".
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class InteractionPromptUITK : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InteractionController interactionController;

        [Header("Appearance")]
        [SerializeField] private string format = "[E] {0}";

        private UIDocument _uiDocument;
        private Label _label;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            if (interactionController == null)
            {
                interactionController = FindFirstObjectByType<InteractionController>();
            }

            var root = _uiDocument.rootVisualElement;
            _label = root.Q<Label>("InteractionPromptLabel");

            if (_label != null)
                _label.style.display = DisplayStyle.None;

            if (interactionController != null)
            {
                interactionController.OnInteractableFound += OnFound;
                interactionController.OnInteractableLost += OnLost;
            }
        }

        private void OnDisable()
        {
            if (interactionController != null)
            {
                interactionController.OnInteractableFound -= OnFound;
                interactionController.OnInteractableLost -= OnLost;
            }
        }

        private void OnFound(IInteractable interactable)
        {
            if (_label == null || interactable == null) return;

            string name = interactable.GetInteractionName();
            _label.text = string.Format(format, name);
            _label.style.display = DisplayStyle.Flex;
        }

        private void OnLost(IInteractable _)
        {
            if (_label == null) return;
            _label.style.display = DisplayStyle.None;
        }
    }
}
#endif
