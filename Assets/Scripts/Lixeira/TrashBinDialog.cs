using Dialog.Manager;
using Duck;
using Interaction;
using UnityEngine;

namespace Lixeira
{
    public class TrashBinDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;

        public override void OnPlayerInteraction()
        {
            dialogManager.StartDialog();
        }
    }
}