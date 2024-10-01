using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Interaction
{
    public class InteractableZone : MonoBehaviour
    {
        [FormerlySerializedAs("button")] [SerializeField] private Button actionButton;

        private readonly List<IInteraction> interactableQueue = new();
    
        private void Awake()
        {
            actionButton.onClick.AddListener(OnButtonClicked);
            actionButton.gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);
    
            if(interactable is null) return;
            if (interactableQueue.Contains(interactable)) return;
        
            interactableQueue.Add(interactable);
            UpdateButton();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);

            interactableQueue.Remove(interactable);
            UpdateButton();
        }
        
        private void UpdateButton()
        {
            actionButton.gameObject.SetActive(interactableQueue.Count > 0);
        }
        
        private void OnButtonClicked()
        {
            if (interactableQueue.Count <= 0) return;
    
            interactableQueue[0].OnPlayerInteraction();
            interactableQueue.RemoveAt(0);
            UpdateButton();
        }
    }
}