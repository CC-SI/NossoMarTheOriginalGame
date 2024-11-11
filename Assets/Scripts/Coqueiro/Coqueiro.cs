using Dialog.Manager;
using Interaction;
using UnityEngine;

namespace Coqueiro
{
    public class Coqueiro : InteractableObject, IInteraction
    {
        private Rigidbody2D rb;
        private Collider2D colisor;
        
        [SerializeField] private DialogManager dialogManager;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            colisor = GetComponent<Collider2D>();
            AddObject(colisor, this);
        }
        
        public void OnPlayerInteraction()
        {
             dialogManager.StartDialog();
        }
    }
}