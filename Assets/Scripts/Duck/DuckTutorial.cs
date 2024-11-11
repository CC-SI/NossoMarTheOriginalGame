using Dialog.Manager;
using UnityEngine;

namespace Duck
{
    public class DuckTutorial : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager; 
        
        public override void OnPlayerInteraction()
        {
            base.OnPlayerInteraction();
            dialogManager.AvancarDialogo();
            Debug.Log("DuckTutorial: OnPlayerInteraction");
        }
    }
}