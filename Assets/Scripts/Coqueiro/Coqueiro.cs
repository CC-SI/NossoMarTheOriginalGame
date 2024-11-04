using Dialog.Manager;
using UnityEngine;

namespace Coqueiro
{
    public class Coqueiro : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        
        public override void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                Debug.Log("Player interagiu com o coqueiro");
                dialogManager.StartDialog();
            }
        }
    }
}