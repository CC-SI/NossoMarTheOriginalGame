using Actors;
using Interaction;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Shovel
{
    public class Shovel : InteractableObject, IInteraction, IMovement
    {
        [field: Header("Componentes Externos")]
        [SerializeField] private Transform alvo;
        private Movement movement;

        [field: Header("Componentes Internos")]
        private Rigidbody2D rb;
        private Collider2D colisor;
        private NavMeshAgent agent;

        [field: Header("Eventos")]
        [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }

        [field: Header("Lógicos")]
        public bool isFollowing;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            colisor = GetComponent<Collider2D>();
            agent = GetComponent<NavMeshAgent>();
            movement = GetComponent<Movement>();

            agent.updateRotation = false;
            agent.updateUpAxis = false;

            AddObject(colisor, this);
        }
    
        /// <summary>
        /// Função para a pá começar a seguir o jogador.
        /// </summary>
        public void StartFollowing()
        {
            if (PlayerBehaviour.Instance)
            {
                alvo = PlayerBehaviour.Instance.GetFollowTarget(this); 
                isFollowing = true;
                movement.SetFollowTarget(alvo); 
            }
        }

        private void FixedUpdate()
        {
            if (isFollowing)
            {
                movement.FollowTarget();
            }
        }

        public void OnPlayerInteraction()
        {
            if (!isFollowing)
            {
                StartFollowing(); 
                RemoveObject(colisor); 
            }
        }
    }
}
