using UnityEngine;

namespace InteractionSystemLite.Samples
{
    public class PickupInteractable : InteractableBase
    {
        [Header("Pickup Settings")]
        [SerializeField] private string itemId = "item";
        [SerializeField] private bool destroyOnPickup = true;

        public override void Interact(GameObject interactor)
        {
            // Example: just log a message.
            Debug.Log($"Picked up {itemId} by {interactor.name}");

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}
