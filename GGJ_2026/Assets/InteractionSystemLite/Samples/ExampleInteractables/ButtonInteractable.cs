using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystemLite.Samples
{
    public class ButtonInteractable : InteractableBase
    {
        [Header("Button Events")]
        public UnityEvent onInteract;

        public override void Interact(GameObject interactor)
        {
            onInteract?.Invoke();
        }
    }
}
