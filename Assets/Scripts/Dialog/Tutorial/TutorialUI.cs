using System.Collections;
using Dialog.Manager;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Button dialogButton;
    [SerializeField] private DialogManager dialogManager;

    [SerializeField] private bool isMoveInMessageInitial;
    
    private IEnumerator Start()
    {
        if (isMoveInMessageInitial)
        {
            dialogButton.gameObject.SetActive(true);
            yield return null;
            PlayerBehaviour.Instance.Movement.enabled = false;
        }
        
        dialogButton.onClick.AddListener(() =>
        {
            PlayerBehaviour.Instance.Movement.enabled = true;
            dialogButton.gameObject.SetActive(false);

            if (dialogManager != null)
            {
                dialogManager.StartDialog();
            }
        });
      
    }
}