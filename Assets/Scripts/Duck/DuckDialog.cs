using Dialog.Manager;
using Interaction;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                dialogManager.StartDialog();
            }
        }

        public override void StartFollowing()
        {
            Debug.Log("DuckDialog: StartFollowing");
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