using Dialog.Manager;
using Duck;
using UnityEngine;

namespace Coqueiro
{
    public class Coqueiro : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        
        public override void OnPlayerInteraction()
        {
            if (!IsFollowing)
            {
                dialogManager.StartDialog();
            }
        }
    }
}