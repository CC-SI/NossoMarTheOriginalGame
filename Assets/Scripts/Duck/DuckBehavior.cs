using System.Collections.Generic;
using Actors;
using Interaction;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Duck
{
    public class DuckBehavior : InteractableObject, IInteraction
    {
        [field: Header("Eventos")]
        [field: SerializeField]
        public UnityEvent<Vector2> OnMoved { get; private set; }

        [SerializeField] private TMP_Text countDucks;
        [SerializeField] private AudioClip clip;
        
        private Movement movement;
        private Collider2D colisor;
        public GraphicBehaviour playerGraphic;
        
        private readonly Dictionary<string, Transform> alvos = new();

        private static int currentDuck = 0;
        protected bool isFollowing;
        private AudioSource audioSource;
        
        PlayerBehaviour Player => PlayerBehaviour.Instance;
    
        private void Start()
        {
            colisor = GetComponent<Collider2D>();
            movement = GetComponent<Movement>();

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;

            AddObject(colisor, this);
        }

        public void StartFollowing(PlayerBehaviour player)
        {
            isFollowing = true;
            alvos.TryAdd("alvodafrente",  player.GetFollowTarget(this));
            alvos.TryAdd("jogador", player.transform);
            currentDuck++;
            countDucks.text = currentDuck.ToString();
            Grasnar();
        }

        private void Update()
        {
            if (!isFollowing) return;
        
            if (playerGraphic.IsMoving)
            {
                movement.SetFollowTarget(alvos.GetValueOrDefault("alvodafrente"));
                return;
            }
        
            movement.SetFollowTarget(alvos.GetValueOrDefault("jogador"));
        }

        private void Grasnar()
        {
            audioSource.Play();
        }

        public virtual void OnPlayerInteraction()
        {
            if (isFollowing) return;
            StartFollowing(Player); 
            RemoveObject(colisor);
        }
    }
}