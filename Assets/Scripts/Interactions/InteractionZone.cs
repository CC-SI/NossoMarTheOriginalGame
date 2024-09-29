using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interactions
{
    public class InteractionZone : MonoBehaviour
    {
        [SerializeField] private Button button;
    
        private readonly List<IInteractableObjects> objectsQueue = new();
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
            button.gameObject.SetActive(false);
        }

        /// <summary>
        /// Adicionando um objeto na fila quando ele entra na zona de interação, está indexado no dicionário e permite interagir.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractableObjects interactable = InteractableObjects.GetInteractable(other);
        
            if(interactable is null) return;
        
            if (!objectsQueue.Contains(interactable))
            {
                Debug.Log("Objeto na fila");
                objectsQueue.Add(interactable);
                UpdateButton();
            }
        }

        /// <summary>
        /// Removendo um objeto da fila quando ele sai da zona de interação.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractableObjects interactable = InteractableObjects.GetInteractable(other);
        
            if(objectsQueue.Remove(interactable))
                UpdateButton();
        }

        /// <summary>
        /// Exibindo o botão quando tem objetos na fila.
        /// </summary>
        private void UpdateButton()
        {
            button.gameObject.SetActive(objectsQueue.Count > 0);
        }

        /// <summary>
        /// Quando clicar no botão, será pego o primeiro objeto da fila e chamada a sua interação. Depois, o objeto é removido da fila.
        /// </summary>
        public void OnButtonClicked()
        {
            if (objectsQueue.Count <= 0) return;
        
            objectsQueue[0].OnPlayerInteract();
            objectsQueue.RemoveAt(0);
            UpdateButton();
        }
    }
}