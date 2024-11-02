using Dialog;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogManager dialogManager;
        
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                if (dialogManager != null && dialogObject != null)
                {
                    dialogManager.SetDialogObject(dialogObject, true);
                    dialogManager.StartDialog();
                }
                else
                {
                    Debug.LogWarning("DialogManager ou DialogObject não foi atribuído.");
                }
            }
        }
        
        public override void StartFollowing()
        {
            base.StartFollowing();
        }
    }
}