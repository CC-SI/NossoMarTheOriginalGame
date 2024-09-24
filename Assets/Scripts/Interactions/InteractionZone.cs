using System;
using UnityEngine;
using UnityEngine.UI;


public class InteractionZone : MonoBehaviour
{
    [SerializeField] private Button button;

    private IInteractableObjects currentInteractable;


    private void Awake()
    {
        button.gameObject.SetActive(false);
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IInteractableObjects interactable = other.GetComponent<IInteractableObjects>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            button.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractableObjects interactable = other.GetComponent<IInteractableObjects>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            button.gameObject.SetActive(false);
        }
    }

    public void OnButtonClicked()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnPlayerInteract();
            button.gameObject.SetActive(false);
        }
    }
}
