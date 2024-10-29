using System.Collections;
using System.Collections.Generic;
using Dialog;
using Interaction;
using UnityEngine;

public class Shovel : InteractableObject, IInteraction
{
    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D colisor;

    [field: Header("Lógicos")]
    public bool isCollected = false;

    private DialogManager dialogManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        AddObject(colisor, this);
    }

    public void Initialize(DialogManager dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public void OnPlayerInteraction()
    {
        if (!isCollected)
        {
            Debug.Log("Capturou a pá");

            isCollected = true;

            DialogUIManager.Instance.ShowMensagemDePaColetada(true);

            gameObject.SetActive(false);

            dialogManager.AdvanceDialogWithoutUI();
        }
    }
}