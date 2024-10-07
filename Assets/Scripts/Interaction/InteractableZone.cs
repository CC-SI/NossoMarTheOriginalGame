using System.Collections.Generic;
using Dialog;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Interaction
{
    public class InteractableZone : MonoBehaviour
    {
        [FormerlySerializedAs("button")] [SerializeField] private Button actionButton;
        
        private readonly List<IInteraction> interactableQueue = new();
        
        private TagInteractionHandler tagInteractionHandler;
        
        /// <summary>
        /// Inicializa o botão de ação e o manipulador de interação de tag ao iniciar.
        /// </summary>
        private void Awake()
        {
            actionButton.onClick.AddListener(OnButtonClicked);
            actionButton.gameObject.SetActive(false);
            tagInteractionHandler = GetComponent<TagInteractionHandler>();
        }
        
        /// <summary>
        /// Adiciona um objeto interagível à fila quando o jogador entra na zona de interação.
        /// </summary>
        /// <param name="other">O collider do objeto que entrou na zona de interação.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);
    
            if(interactable is null) return;
            if (interactableQueue.Contains(interactable)) return;
        
            interactableQueue.Add(interactable);
            UpdateButton();
        }
        
        /// <summary>
        /// Remove um objeto interagível da fila quando o jogador sai da zona de interação.
        /// </summary>
        /// <param name="other">O collider do objeto que saiu da zona de interação.</param>
        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = InteractableObject.GetInteractable(other);

            interactableQueue.Remove(interactable);
            UpdateButton();
        }
        
        /// <summary>
        /// Atualiza a visibilidade do botão de ação com base na quantidade de objetos interagíveis na fila.
        /// </summary>
        private void UpdateButton()
        {
            actionButton.gameObject.SetActive(interactableQueue.Count > 0);
        }
        
        /// <summary>
        /// Trata a interação do jogador com o primeiro objeto interagível na fila quando o botão é clicado.
        /// </summary>
        private void OnButtonClicked()
        {
            if (interactableQueue.Count <= 0) return;
            
            var interactable = interactableQueue[0];
            
            interactable.OnPlayerInteraction();
            
            /*
             * Se o manipulador de interação de tag não for nulo, chama o método para
             * interagir com o objeto
             */
            if (tagInteractionHandler != null)
            {
                tagInteractionHandler.HandleTagInteraction(interactable.GameObject);
            }
            
            interactableQueue.RemoveAt(0);
            UpdateButton();
        }
    }
}