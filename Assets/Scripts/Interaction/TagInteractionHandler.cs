using Dialog;
using UnityEngine;

namespace Interaction
{
    /// <summary>
    /// Classe responsável por gerenciar interações baseadas em tags
    /// e exibir diálogos apropriados.
    /// </summary>
    public class TagInteractionHandler : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;
        
        /// <summary>
        /// Lida com a interação de um objeto baseado em sua tag.
        /// </summary>
        /// <param name="obj">O objeto que está sendo interagido.</param>
        public void HandleTagInteraction(GameObject obj)
        {
            // Verifica se o objeto possui a tag "DuckBuriedTag"
            if (obj.CompareTag("DuckBuriedTag"))
            {
                // Se a tag corresponder, exibe o diálogo associado à ID "patoenterrado"
                dialogManager.ShowDialogForId("patoenterrado");
            } 
        }
        
    }
}