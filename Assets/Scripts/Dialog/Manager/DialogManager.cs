using Duck;
using UnityEngine;

namespace Dialog.Manager
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private DialogUIManager dialogUIManager;
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private ControllerDialogIdSpeeches controllerDialogIdSpeeches;
        [SerializeField] private DuckDialog _duckDialog;
        
        private void Start()
        {
            dialogUIManager.InitComponent();
            ResetDialog();
        }

        public void StartDialog()
        {
            if (dialogObject.Dialogos.Count == 0) return;
        
            ShowCurrentDialog();
            controllerDialogIdSpeeches.ControllerActionsForId(); 
        }
        
        public void AvancarDialogo()
        {
            var next = dialogObject.AvancarDialogo();

            if (next)
            {
                Debug.Log("Avançar Dialogo");
                ShowCurrentDialog();
                controllerDialogIdSpeeches.ControllerActionsForId(); 
            }
            else
            {
                Debug.Log("Fim do Dialogo");
                EndDialog();
                _duckDialog.StartFollowing();
            }
        }

        public void AvancarDialogoSilenciosamente()
        {
            var next = dialogObject.AvancarDialogoSilenciosamente();

            if (next)
            {
                Debug.Log("Avançar Dialogo Silenciosamente");
                controllerDialogIdSpeeches.ControllerActionsForId(); 
            }
            else
            {
                Debug.Log("Fim do Dialogo Silenciosamente");
                EndDialog(); 
            }
        }

        private void ShowCurrentDialog()
        {
            var currentDialog = dialogObject.GetDialogoAtual();
            
            if (currentDialog == null) return;
            
            dialogUIManager.ShowDialog(true);
            dialogUIManager.SetSpeaches(currentDialog.speaker, currentDialog.texto);
        }

        public void EndDialog()
        {
            dialogUIManager.ShowDialog(false);
        }

        private void ResetDialog()
        {
            dialogObject.ResetDialog();
        }
    }
}
