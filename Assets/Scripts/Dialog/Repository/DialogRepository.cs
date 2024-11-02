using UnityEngine;

namespace Dialog.Repository
{
    public class DialogRepository : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogUIManager dialogUIManager;

        public void LogDialog(Dialogo dialogo)
        {
            if (dialogObject.WhichThisIsDialogue == WhichThisIsDialogue.DUCK_BURIED)
            {
                dialogUIManager.ShowAvancarButton(false);
                
                switch (dialogo.id)
                {
                    case "pato1_pedindo_ajuda":
                        dialogUIManager.ShowYesNoButtons(true);
                        dialogUIManager.ShowAvancarButton(false);
                        break;
                    case "pato2_encontrar_pa":
                        dialogUIManager.ShowPaColetada(true);
                        break;
                    case "player3_procurando_pa":
                        dialogUIManager.ShowAvancarButton(false);
                        break;
                    default:
                        dialogUIManager.ShowPaColetada(false);
                        dialogUIManager.ShowAvancarButton(true);
                        dialogUIManager.ShowYesNoButtons(false);
                        break;
                }    
            }

            if (dialogObject.WhichThisIsDialogue == WhichThisIsDialogue.DUCK_MADAME)
            {
                dialogUIManager.ShowAvancarButton(false); 
                dialogUIManager.ShowYesNoButtons(false);
                dialogUIManager.ShowPaColetada(false);
    
                switch (dialogo.id)
                {
                    case "pata_madame1":
                        dialogUIManager.ShowYesNoButtons(true);
                        break;
                    case "player_madame1":
                        dialogUIManager.ShowPaColetada(true);
                        dialogo.SetCanAdvance(false);
                        dialogUIManager.ShowAvancarButton(false);
                        break;
                    default:
                        dialogUIManager.ShowPaColetada(false);
                        dialogUIManager.ShowYesNoButtons(false);
                        break;
                }
            }


        }
    }
}