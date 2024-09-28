using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractionZone : MonoBehaviour
{
    [SerializeField] private Button button;
    
    private Dictionary<Collider2D, IInteractableObjects> dicObjects = new Dictionary<Collider2D, IInteractableObjects>();
    private Queue<Collider2D> objectsQueue = new Queue<Collider2D>();

    /// <summary>
    /// CALMA! EU VOU MELHORAR!
    /// 
    /// Dicionário: pegando todos os objetos do tipo MonoBehaviour, verificando se implementam a interface IInteractableObjects; verificando se possuem um Collider2D; adicionando-os no dicionário.
    /// Botão: adicionando um listener para o evento de clique e ocultando-o.
    /// </summary>
    private void Awake()
    {
        MonoBehaviour[] allBehaviours = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour behaviour in allBehaviours)
        {
            if (behaviour is IInteractableObjects interactable)
            {
                Collider2D collider = behaviour.GetComponent<Collider2D>();
                if (collider != null && !dicObjects.ContainsKey(collider))
                {
                    // Armazena o collider e seu respectivo objeto interagível
                    dicObjects.Add(collider, interactable);
                }
            }
        }
        
        button.onClick.AddListener(OnButtonClicked);
        button.gameObject.SetActive(false);
    }

    /// <summary>
    /// Adicionando um objeto na fila quando ele entra na zona de interação, está indexado no dicionário e permite interagir.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dicObjects.TryGetValue(other, out IInteractableObjects interactable) && interactable.CanInteract() && !objectsQueue.Contains(other))
        {
            Debug.Log("Interactable object found");
            objectsQueue.Enqueue(other);
            UpdateButton();
        }
    }

    /// <summary>
    /// Removendo um objeto da fila quando ele sai da zona de interação.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsQueue.Contains(other))
        {
            objectsQueue = new Queue<Collider2D>(objectsQueue.Where(c => c != other));
            UpdateButton();
        }
    }

    /// <summary>
    /// Exibindo o botão quando tem objetos na fila.
    /// </summary>
    private void UpdateButton()
    {
        button.gameObject.SetActive(objectsQueue.Count > 0);
    }

    /// <summary>
    /// Quando clicar no botão, será pego o primeiro objeto da fila e chamada a sua interação. Depois, o objeto é removido da fila.
    /// </summary>
    public void OnButtonClicked()
    {
        if (objectsQueue.Count > 0)
        {
            Collider2D currentObject = objectsQueue.Peek();
            IInteractableObjects target = dicObjects[currentObject];
            if (target != null)
            {
                target.OnPlayerInteract();
                objectsQueue.Dequeue();
                UpdateButton();
            }
        }
    }
}