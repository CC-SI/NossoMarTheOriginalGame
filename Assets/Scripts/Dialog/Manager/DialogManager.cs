using Duck;
using UnityEngine;

namespace Dialog.Manager
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private DialogUIManager dialogUIManager;
        [SerializeField] private ControllerDialogIdSpeeches controllerDialogIdSpeeches;
        [SerializeField] private DuckDialog _duckDialog;
        [SerializeField] private PerguntaManager perguntaManager;
        [SerializeField] private bool isDuckBuried;
        [SerializeField] private Animator duckAnimator;
        [SerializeField] private DialogObject dialogObject;
        
        [SerializeField] private bool isTutorial;
        [SerializeField] private Animator playerAnimator;
        
        public bool IsDialogActive;
        
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
            IsDialogActive = true;
            
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
                Debug.Log("Inicio");
                playerAnimator.SetBool("isMoving", false);
            }
            else
            {
                EndDialog();
                
                dialogObject.AtualizarShowCocoPorId("player_confirmando_agua_coco", false);
                
                ResetDialog();
                
                dialogUIManager.ShowIconeInteracao(false);
                if (_duckDialog != null)
                {
                    _duckDialog.StartFollowing();
                }
                
                if (duckAnimator != null && isDuckBuried)
                {
                    duckAnimator.SetBool("isBurried", false);
                }
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


            string dialogText = currentDialog.texto;
            
            string deviceType = PlayerPrefs.GetString("DeviceType");
            
            if (currentDialog.id.Equals("joystick"))
            {
                if (deviceType.Equals("Mobile"))
                {
                    dialogText = "Olha só, no canto inferior esquerdo está o joystick. Use ele para se mover e explorar!";
                }
                else
                {
                    dialogText = "Você pode se movimentar usando as teclas \"A,W,S,D\" ou \"\u2190, \u2191, \u2192, \u2193\"";
                }
            } else if (currentDialog.id.Equals("button_captured"))
            {
                if (deviceType.Equals("Mobile"))
                {
                    dialogText = "Veja, ali está um patinho! Vá até ele. Irá aparecer um botão para capturá-lo. Pressione-o para salvar o patinho!";
                }
                else
                {
                    dialogText = "Veja, ali está um patinho! Vá até ele e pressione \"Espaço\" para capturá-lo e salvar o patinho!";
                }
            }
            
            dialogUIManager.ShowDialog(true);
            dialogUIManager.SetSpeaches(currentDialog.speaker, dialogText);
        }

        public void EndDialog()
        {
            IsDialogActive = false;
            dialogUIManager.ShowDialog(false);
        }

    
        private void ResetDialog()
        {
            dialogObject.ResetDialog();
        }
    }
}
