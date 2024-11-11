using Dialog.Manager;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Button dialogButton;
    [SerializeField] private DialogManager dialogManager;
    
    private void Start()
    {
        dialogButton.gameObject.SetActive(true);
        
        dialogButton.onClick.AddListener(() =>
        {
            dialogButton.gameObject.SetActive(false);

            if (dialogManager != null)
            {
                dialogManager.StartDialog();
            }
        });
      
    }
}