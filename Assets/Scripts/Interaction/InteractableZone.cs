using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractableZone : MonoBehaviour
    {
        private readonly List<IInteraction> interactableQueue = new();

        bool canInteract = false;
        
        [field: Header("Eventos")]
        [field: SerializeField]
        public UnityEvent<bool> OnCanInteractChanged { get; private set; }

        public bool CanInteract
        {
            get => canInteract;
            private set
            {
                canInteract = value;
                OnCanInteractChanged.Invoke(canInteract);
            }
        }

        public void Interact()
        {
            if (!CanInteract)
                return;
    
            interactableQueue[0].OnPlayerInteraction();
            interactableQueue.RemoveAt(0);
            
            OnQueueUpdated();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);
    
            if(interactable is null) return;
            if (interactableQueue.Contains(interactable)) return;
        
            interactableQueue.Add(interactable);
            OnQueueUpdated();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);

            interactableQueue.Remove(interactable);
            OnQueueUpdated();
        }
        
        void OnQueueUpdated()
        {
            CanInteract = interactableQueue.Count > 0;
        }
    }
}