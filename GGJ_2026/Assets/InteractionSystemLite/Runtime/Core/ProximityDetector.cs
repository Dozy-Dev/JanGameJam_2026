using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystemLite
{
    /// <summary>
    /// Uses a trigger collider to detect nearby IInteractable objects.
    /// Selects the closest valid one.
    /// </summary>
    //[RequireComponent(typeof(Collider))]
    public class ProximityDetector : MonoBehaviour
    {
        [Header("Proximity Settings")]
        [Tooltip("Optional: layer mask filter for interactables.")]
        [SerializeField] private LayerMask interactableLayers = ~0;

        private readonly List<IInteractable> _inside = new List<IInteractable>();
        private IInteractable _current;

        public event Action<IInteractable> OnInteractableFound;
        public event Action<IInteractable> OnInteractableLost;

        private void Reset()
        {
            Collider col = GetComponent<Collider>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsLayerAllowed(other.gameObject.layer)) return;

            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null && !_inside.Contains(interactable))
            {
                _inside.Add(interactable);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsLayerAllowed(collision.gameObject.layer)) return;

            IInteractable interactable = collision.GetComponentInParent<IInteractable>();
            if (interactable != null && !_inside.Contains(interactable))
            {
                _inside.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            if (interactable != null && _inside.Remove(interactable))
            {
                if (ReferenceEquals(interactable, _current))
                {
                    OnInteractableLost?.Invoke(_current);
                    _current = null;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            IInteractable interactable = collision.GetComponentInParent<IInteractable>();
            if (interactable != null && _inside.Remove(interactable))
            {
                if (ReferenceEquals(interactable, _current))
                {
                    OnInteractableLost?.Invoke(_current);
                    _current = null;
                }
            }
        }

        private void Update()
        {
            IInteractable best = FindClosestInteractable();
            if (!ReferenceEquals(best, _current))
            {
                if (_current != null)
                    OnInteractableLost?.Invoke(_current);

                _current = best;

                if (_current != null)
                    OnInteractableFound?.Invoke(_current);
            }
        }

        private IInteractable FindClosestInteractable()
        {
            if (_inside.Count == 0) return null;

            float bestSqrDist = float.MaxValue;
            IInteractable best = null;

            Vector3 origin = transform.position;

            for (int i = _inside.Count - 1; i >= 0; i--)
            {
                IInteractable it = _inside[i];
                if (it is Component comp && comp != null)
                {
                    float sqr = (comp.transform.position - origin).sqrMagnitude;
                    if (sqr < bestSqrDist)
                    {
                        bestSqrDist = sqr;
                        best = it;
                    }
                }
                else
                {
                    // Clean nulls
                    _inside.RemoveAt(i);
                }
            }

            return best;
        }

        private bool IsLayerAllowed(int layer)
        {
            return (interactableLayers.value & (1 << layer)) != 0;
        }

        public IInteractable GetCurrentInteractable()
        {
            return _current;
        }
    }
}
