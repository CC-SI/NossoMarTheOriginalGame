using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public abstract class InteractableObject : MonoBehaviour
    {
        private static readonly Dictionary<Collider2D, IInteraction> interactableObjects = new();

        protected static void AddObject(Collider2D collider, IInteraction interactable)
        {
            interactableObjects.TryAdd(collider, interactable);
        }

        protected void RemoveObject(Collider2D collider)
        {
            interactableObjects.Remove(collider);
        }

        public static IInteraction GetInteractable(Collider2D collider)
        {
            return interactableObjects.GetValueOrDefault(collider);
        }
    }
}