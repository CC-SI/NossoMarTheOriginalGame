using Dialog.Manager;
using UnityEngine;

namespace Dialog
{
    public class DialogController : MonoBehaviour
    {
        
        [SerializeField] private DialogManager dialog;

        void Start()
        {
            dialog.StartDialog();
        }
    }
}