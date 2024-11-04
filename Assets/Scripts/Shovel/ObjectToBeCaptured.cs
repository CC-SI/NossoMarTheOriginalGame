using System.Collections.Generic;
using Dialog.Manager;
using Interaction;
using UnityEngine;

public class ObjectToBeCaptured : InteractableObject, IInteraction
{
    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D colisor;
    
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private DialogUIManager dialogUIManager;

    [SerializeField] private bool IsCocoCaptured;
    
    private static int currentCoco = 0;

    public bool IsAllCapturedCocos;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        AddObject(colisor, this);
    }
    
    public void OnPlayerInteraction()
    {
        Debug.Log(gameObject.name + " foi capturado");
        
        if (IsCocoCaptured)
        {
            Debug.Log("Coco capturado");
            gameObject.SetActive(false);
            CocosCapturados();

            if (currentCoco >= 2)
            {
                IsAllCapturedCocos = true;
            }
        }
        
        if (dialogManager != null)
        {
            dialogManager.AvancarDialogoSilenciosamente();
        }
        
        if (dialogUIManager != null)
        {
            dialogUIManager.ShowMensagemObjetoPego(true);
        }
    }

    private void CocosCapturados()
    {
        currentCoco++;
        Debug.Log(currentCoco);
    }
}