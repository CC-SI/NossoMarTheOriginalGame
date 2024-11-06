using System.Collections.Generic;
using Actors;
using Interaction;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DuckBehavior : InteractableObject, IInteraction
{
    [field: Header("Componentes Externos")] 
    [SerializeField] private Transform alvo;

    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioClip clip;
    private Movement movement;

    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D colisor;
    private NavMeshAgent agent;

    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }

    [field: Header("Lógicos")] 
    private static int currentDuck = 0;
    public bool isFollowing;
    private AudioSource audioSource;
    public GraphicBehaviour playerGraphic;
    
    private readonly Dictionary<string, Transform> alvos = new();
    
    PlayerBehaviour Player => PlayerBehaviour.Instance;
    
    private void Start()
    {
        if (countDucks != null)
        {
            currentDuck = 0;
            countDucks.text = currentDuck.ToString();
        }
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<Movement>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        if (clip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
        }

        AddObject(colisor, this);
    }
    public void StartFollowing(PlayerBehaviour player)
    {
        // Inicia o seguimento do pato ao jogador se a instância do PlayerBehaviour estiver presente.
        if (PlayerBehaviour.Instance)
        {
            alvo = PlayerBehaviour.Instance.transform;
            if (movement != null)
            {
                movement.SetFollowTarget(alvo);
            }

            if (CompareTag("Duck"))
            {
                currentDuck++;
                Debug.Log("Contando");
                Debug.Log(currentDuck);
                countDucks.text = currentDuck.ToString();
            }

            alvos.TryAdd("alvodafrente",  player.GetFollowTarget(this));
            alvos.TryAdd("jogador", player.transform);
            
            if (audioSource != null)
            {
                audioSource.Play();
            }

            isFollowing = true;
        }
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
    
    public virtual void  OnPlayerInteraction()
    {
        if (!isFollowing)
        {
            StartFollowing(Player); 
            RemoveObject(colisor);
        }
    }
}