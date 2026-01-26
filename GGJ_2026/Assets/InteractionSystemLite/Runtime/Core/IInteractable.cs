using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Core interface for anything that can be interacted with.
    /// Implement this on your components or derive from InteractableBase.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Name shown in the UI (e.g., "Open Door", "Talk", "Pick Up").
        /// </summary>
        string GetInteractionName();

        /// <summary>
        /// Returns whether this object can be interacted with right now.
        /// </summary>
        bool CanInteract(GameObject interactor);

        /// <summary>
        /// Executes the interaction.
        /// </summary>
        void Interact(GameObject interactor);
    }
}
