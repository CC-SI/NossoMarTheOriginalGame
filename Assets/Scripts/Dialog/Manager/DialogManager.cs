using Duck;
using UnityEngine;
using UnityEngine.EventSystems;

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
            
            Debug.Log("AA");
            
            if (next)
            {
                ShowCurrentDialog();
                controllerDialogIdSpeeches.ControllerActionsForId(); 
            }
            else
            {
                EndDialog();
                
                dialogObject.AtualizarShowCocoPorId("player_confirmando_agua_coco", false);
                
                dialogUIManager.ShowIconeInteracao(false);
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

        /*
        private IEnumerator TypeText(string text)
        {
            dialogUIManager.textDialog.text = ""; 
            foreach (char letter in text)
            {
                dialogUIManager.textDialog.text += letter; 
                yield return new WaitForSeconds(0.05f); 
            }
        }
        */

        private void ShowCurrentDialog()
        {
            var currentDialog = dialogObject.GetDialogoAtual();
            
            if (currentDialog == null) return;
            
            dialogUIManager.ShowDialog(true);
            dialogUIManager.SetSpeaches(currentDialog.speaker, currentDialog.texto);
            // StartCoroutine(TypeText(currentDialog.texto));
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
