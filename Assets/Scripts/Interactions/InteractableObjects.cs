using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public static class InteractableObjects
    {
        private static Dictionary<Collider2D, IInteractableObjects> dic = new();
    
        public static void AddObject(this IInteractableObjects interactable, Collider2D collider)
        {
            if (!dic.ContainsKey(collider))
            {
                dic.Add(collider, interactable);
            }
        }
    
        public static void RemoveObject(this Collider2D collider)
        {
            dic.Remove(collider);
        }
    
        public static bool Interact(Collider2D collider)
        {
            if (dic.TryGetValue(collider, out IInteractableObjects interactable))
            {
                interactable.OnPlayerInteract();
                return true;
            }
            return false;
        }
    
        public static IInteractableObjects GetInteractable(Collider2D collider)
        {
            if (dic.TryGetValue(collider, out IInteractableObjects interactable))
            {
                return interactable;
            }

            return null;
        }
    }
}