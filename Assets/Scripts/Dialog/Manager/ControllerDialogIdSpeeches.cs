using System;
using UnityEngine;

namespace Dialog.Manager
{
    public class ControllerDialogIdSpeeches : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogUIManager dialogUIManager;
        
        [Header("Opcional")]
        [SerializeField] private PerguntasUIManager perguntasUIManager;
        
        public void ControllerActionsForId()
        {
            string dialogId  = dialogObject.GetCurrentDialogId();

            if (!string.IsNullOrWhiteSpace(dialogId))
            {
                dialogUIManager.ShowButtonsOfDecision(false);
                dialogUIManager.ShowZonasDeAvancarDialogo(true);
                dialogUIManager.ShowPaOuChapeuOuCoco(false);
                dialogUIManager.ShowCoqueiro(false);
                
                switch (dialogId)
                {
                    // Coqueiro
                    case "perguntas_coqueiro":
                        dialogUIManager.ShowDialog(false);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        
                        if (perguntasUIManager != null)
                        {
                            perguntasUIManager.ShowPainelPerguntas(true);
                        }
                        break;
                    
                    // Pato agua de coco
                    case "player_confirmando":
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        dialogUIManager.ShowCoqueiro(true);
                        break;
                    
                    // Pato Professor
                    case "pergunta":
                        dialogUIManager.ShowDialog(false);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        
                        if (perguntasUIManager != null)
                        {
                            perguntasUIManager.ShowPainelPerguntas(true);
                        }
                        
                        break;
                    // Pato Enterrado
                    case "pato1_pedindo_ajuda":
                        dialogUIManager.ShowButtonsOfDecision(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "player3_procurando_pa":
                        dialogUIManager.ShowPaOuChapeuOuCoco(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    
                    // Pato Madame
                    case "pata_madame1":
                        dialogUIManager.ShowButtonsOfDecision(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "player_madame1":
                        dialogUIManager.ShowPaOuChapeuOuCoco(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                }
            }
        }
    }
}