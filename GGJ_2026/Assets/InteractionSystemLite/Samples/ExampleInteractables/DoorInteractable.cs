using UnityEngine;

namespace InteractionSystemLite.Samples
{
    public class DoorInteractable : InteractableBase
    {
        [Header("Door Settings")]
        [SerializeField] private Transform doorTransform;
        [SerializeField] private Vector3 openRotation = new Vector3(0, 90, 0);
        [SerializeField] private float openCloseSpeed = 5f;

        private bool _isOpen;
        private Quaternion _closedRotation;
        private Quaternion _openRotation;

        private void Awake()
        {
            if (doorTransform == null)
                doorTransform = transform;

            _closedRotation = doorTransform.localRotation;
            _openRotation = Quaternion.Euler(openRotation) * _closedRotation;
        }

        private void Update()
        {
            Quaternion target = _isOpen ? _openRotation : _closedRotation;
            doorTransform.localRotation = Quaternion.Lerp(
                doorTransform.localRotation,
                target,
                Time.deltaTime * openCloseSpeed);
        }

        public override void Interact(GameObject interactor)
        {
            _isOpen = !_isOpen;
        }
    }
}
