using System.Collections.Generic;
using Actors;
using Dialog;
using Interaction;
using UnityEngine;
using UnityEngine.WSA;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        private static DuckDialog Instance { get; set; }

        [SerializeField] private DialogObject dialogObject;
        
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
                WhichThisIsDialogue dialogueType = GetDialogueTypeByTag(tag);

                if (dialogueType == WhichThisIsDialogue.DUCK_BURIED)
                {
                    DialogManager.Instance.StartDialog(dialogObject);
                }
            }
        }

        private WhichThisIsDialogue GetDialogueTypeByTag(string tag)
        {
            return tag switch
            {
                "DuckBuriedTag" => WhichThisIsDialogue.DUCK_BURIED
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