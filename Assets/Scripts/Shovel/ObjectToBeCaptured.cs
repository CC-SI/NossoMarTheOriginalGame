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
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        AddObject(colisor, this);
    }
    
    public void OnPlayerInteraction()
    {
        Debug.Log(gameObject.name + " foi capturado");
        
        dialogManager.AvancarDialogoSilenciosamente();
        
        dialogUIManager.ShowMensagemObjetoPego(true);
    } 
}