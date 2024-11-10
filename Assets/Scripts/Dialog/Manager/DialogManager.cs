using Duck;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dialog.Manager
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private DialogUIManager dialogUIManager;
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private ControllerDialogIdSpeeches controllerDialogIdSpeeches;
        [SerializeField] private DuckDialog _duckDialog;
        [SerializeField] private PerguntaManager perguntaManager;
        [SerializeField] private bool isDuckBuried;
        [SerializeField] private Animator duckAnimator;
        
        private void Start()
        {
            dialogUIManager.InitComponent();
            ResetDialog();
            
            if (duckAnimator != null && isDuckBuried)
            {
                duckAnimator.SetBool("isBurried", true);
            }
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
                
                dialogObject.AtualizarShowCocoPorId("player_confirmando_agua_coco", false);
                
                dialogUIManager.ShowIconeInteracao(false);
                if (_duckDialog != null)
                {
                    _duckDialog.StartFollowing();
                }
                
                if (duckAnimator != null && isDuckBuried)
                {
                    duckAnimator.SetBool("isBurried", false);
                }
                Debug.Log("ACABOU");
            }
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
