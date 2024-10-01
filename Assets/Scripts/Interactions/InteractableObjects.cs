using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public abstract class InteractableObject : MonoBehaviour
    {
        private static readonly Dictionary<Collider2D, IInteractable> interactableObjects = new();

        protected static void AddObject(Collider2D collider, IInteractable interactable)
        {
            interactableObjects.TryAdd(collider, interactable);
        }

        protected static void RemoveObject(Collider2D collider)
        {
            interactableObjects.Remove(collider);
        }

        public static IInteractable GetInteractable(Collider2D collider)
        {
            return interactableObjects.GetValueOrDefault(collider);
        }
    }
}