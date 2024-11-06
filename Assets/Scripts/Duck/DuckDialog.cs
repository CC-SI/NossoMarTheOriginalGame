using Dialog.Manager;
using Interaction;
using Player;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private ObjectToBeCaptured objectToBeCaptured;
        
        public bool isDuckAguaCoco;
        
        PlayerBehaviour Player => PlayerBehaviour.Instance;
        
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                if (objectToBeCaptured != null)
                {
                    if (isDuckAguaCoco && objectToBeCaptured.IsAllCapturedCocos)
                    {
                        dialogManager.AvancarDialogoSilenciosamente();
                    }
                }
                
                dialogManager.StartDialog();
                
            }
        }
        
        public void StartFollowing()
        {
            if (isDuckAguaCoco)
            {
                Debug.Log("Pato agua de coco capturado ");
                
            }
            
            // Debug.Log("DuckDialog: StartFollowing");
            // base.StartFollowing(Player);
            base.OnPlayerInteraction();
        }
    }
}