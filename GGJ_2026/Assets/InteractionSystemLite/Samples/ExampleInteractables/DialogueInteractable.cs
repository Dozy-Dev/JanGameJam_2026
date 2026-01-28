using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystemLite.Samples
{
    public class DialogueInteractable : InteractableBase
    {
        [TextArea]
        [SerializeField] private string dialogueText = "Hello there!";

        [Header("Events")]
        public UnityEvent<string> onDialogueStarted;

        public override void Interact(GameObject interactor)
        {
            onDialogueStarted?.Invoke(dialogueText);
        }
    }
}
