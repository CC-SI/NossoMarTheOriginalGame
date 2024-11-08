using UnityEngine;

namespace Dialog.Manager
{
    public class ControllerDialogIdSpeeches : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogObject duckAguaCoco;
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
                
                if (perguntasUIManager != null)
                {
                    perguntasUIManager.isShowCoco = false;
                }
                
                switch (dialogId)
                {
                    // Coqueiro
                    case "perguntas_coqueiro":
                        dialogUIManager.ShowDialog(false);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                
                        if (duckAguaCoco != null)
                        {
                            var dialogo = duckAguaCoco.GetDialogoPorId("player_confirmando_agua_coco");
                            if (dialogo != null)
                            {
                                Debug.Log($"Dialogo encontrado: {dialogo.id}");
                                Debug.Log($"Show coco: {dialogo.ShowCoco}");

                                bool showCoco = dialogo.ShowCoco;

                                if (showCoco)
                                {
                                    perguntasUIManager.isShowCoco = true;
                                } 
                                else
                                {
                                    perguntasUIManager.isShowCoco = false;
                                }
                            }
                        }
                        
                        if (perguntasUIManager != null)
                        {
                            perguntasUIManager.ShowPainelPerguntas(true);
                        }
                
                        break;
                    
                    // Pato agua de coco
                    case "player_confirmando_agua_coco":
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        Debug.Log("Você esta aqui");
                        duckAguaCoco.AtualizarShowCocoPorId("player_confirmando_agua_coco", true);
                        
                        break;
                    
                    // Pato Lixo
                    case "pato_lixo_agradecendo":
                        GameManager.LoadMiniGame();
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
                    case "pata_madame_agradecendo":
                        if (dialogUIManager.boxDialog.activeSelf)
                        {
                            dialogUIManager.ShowAcessorioPato(true);
                        }
                        break;
                }
            }
        }
    }
}