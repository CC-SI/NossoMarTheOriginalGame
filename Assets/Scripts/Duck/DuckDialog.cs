using System.Collections.Generic;
using Actors;
using Dialog;
using Interaction;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        public static DuckDialog Instance { get; private set; }
        
        private Dictionary<string, string> mapDialogues = new Dictionary<string, string>()
        {
            { "DuckBuriedTag", "patoenterrado" }
        };

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                if (mapDialogues.TryGetValue(tag, out string dialogueId))
                {
                    DialogManager.Instance.StartDialogue(dialogueId);

                    if (DialogManager.Instance.IsDialogFinshed)
                    {
                        StartFollowing();
                    }
                }
            }
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