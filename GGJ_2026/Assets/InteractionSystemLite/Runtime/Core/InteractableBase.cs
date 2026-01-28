using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Optional base class to make interactables easier to create.
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction")]
        [SerializeField] private string interactionName = "Interact";

        [Tooltip("Optional: if true, this interaction is currently disabled.")]
        [SerializeField] private bool isDisabled;

        public virtual string GetInteractionName()
        {
            return interactionName;
        }

        public virtual bool CanInteract(GameObject interactor)
        {
            return !isDisabled;
        }

        public abstract void Interact(GameObject interactor);

        /// <summary>
        /// Enable or disable the interactable at runtime.
        /// </summary>
        public virtual void SetDisabled(bool disabled)
        {
            isDisabled = disabled;
        }
    }
}
