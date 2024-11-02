using UnityEngine;

namespace Dialog.Manager
{
    public class ControllerDialogIdSpeeches : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogUIManager dialogUIManager;
        
        public void ControllerActionsForId()
        {
            string dialogId  = dialogObject.GetCurrentDialogId();

            if (!string.IsNullOrWhiteSpace(dialogId))
            {
                dialogUIManager.ShowButtonsOfDecision(false);
                dialogUIManager.ShowZonasDeAvancarDialogo(true);
                dialogUIManager.ShowPaOuChapeuOuCoco(false);
                
                switch (dialogId)
                {
                    case "pato1_pedindo_ajuda":
                        dialogUIManager.ShowButtonsOfDecision(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "player3_procurando_pa":
                        dialogUIManager.ShowPaOuChapeuOuCoco(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
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