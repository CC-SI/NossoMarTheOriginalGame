using Dialog.Manager;
using Interaction;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private ObjectToBeCaptured objectToBeCaptured;
        
        public bool isDuckAguaCoco;
        
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                if (objectToBeCaptured != null && isDuckAguaCoco && objectToBeCaptured.IsAllCapturedCocos)
                {
                    dialogManager.AvancarDialogoSilenciosamente();
                }
                dialogManager.StartDialog();
            }
        }
        
        public override void StartFollowing()
        {
            if (isDuckAguaCoco)
            {
                Debug.Log("Pato agua de coco capturado ");
            }
            
            // Debug.Log("DuckDialog: StartFollowing");
            base.StartFollowing();
            HideCaptureButton();
        }
        
        private static void HideCaptureButton()
        {
            var interactionUIButton = FindObjectOfType<InteractionUIButton>();
            if (interactionUIButton != null)
            {
                interactionUIButton.HideButton();
            }
        }
    }
}