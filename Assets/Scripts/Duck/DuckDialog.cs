using Dialog.Manager;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private ObjectToBeCaptured objectToBeCaptured;

        [SerializeField] private GameObject acessorio;
        [SerializeField] private GameObject ponto;
        
        public bool isDuckAguaCoco;
        
        public override void OnPlayerInteraction()
        {
            if (!IsFollowing)
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
            IsRescued = true;

            if (IsRescued)
            {
                acessorio.SetActive(true);
                ponto.SetActive(false);
            }
            
            base.OnPlayerInteraction();
        }
    }
}