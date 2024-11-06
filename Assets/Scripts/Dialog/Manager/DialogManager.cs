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
        [SerializeField] private PerguntaManager perguntaManager;
        
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
            
            if (perguntaManager != null)
            {
                perguntaManager.StartPerguntas();
            }
        }
        
        public void AvancarDialogo()
        {
            var next = dialogObject.AvancarDialogo();

            if (next)
            {
                ShowCurrentDialog();
                controllerDialogIdSpeeches.ControllerActionsForId(); 
            }
            else
            {
                EndDialog();
                if (_duckDialog != null)
                {
                    _duckDialog.StartFollowing();
                }
            }
        }

        public void EndDialogAndCaptureDuck()
        {
            EndDialog();
            _duckDialog.StartFollowing();
        }

        public void AvancarDialogoSilenciosamente()
        {
            var next = dialogObject.AvancarDialogoSilenciosamente();

            if (next)
            {
                controllerDialogIdSpeeches.ControllerActionsForId(); 
            }
            else
            {
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
