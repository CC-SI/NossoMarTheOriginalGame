using Actors;
using Dialog;
using Interaction;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogObject dialogObject;
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                WhichThisIsDialogue dialogueType = GetDialogueTypeByTag(tag);

                if (dialogueType == WhichThisIsDialogue.DUCK_BURIED || dialogueType == WhichThisIsDialogue.DUCK_MADAME)
                {
                    DialogManager.Instance.StartDialog(dialogObject);
                }
            }
        }

        private WhichThisIsDialogue GetDialogueTypeByTag(string tag)
        {
            return tag switch
            {
                "DuckBuriedTag" => WhichThisIsDialogue.DUCK_BURIED,
                "DuckMadameTag" => WhichThisIsDialogue.DUCK_BURIED
            };
        }

        public void StartFollowing()
        {
            base.OnPlayerInteraction();
            HideCaptureButton();

            var graphicBehavior = GetComponentInChildren<GraphicBehaviour>();
            
            graphicBehavior.BuriedDuck();
        }

        private void HideCaptureButton()
        {
            InteractionUIButton interactionUIButton = FindObjectOfType<InteractionUIButton>();
            if (interactionUIButton != null)
            {
                interactionUIButton.HideButton();
            }
        }
    }
}