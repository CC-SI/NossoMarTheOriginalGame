
using Dialog;
using Interaction;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DuckBehavior : InteractableObject, IInteraction
{
    [field: Header("Componentes Externos")] [SerializeField]
    private Transform alvo;

    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioClip clip;
    private Movement movement;

    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;

    private Collider2D colisor;
    private Animator animator;
    private NavMeshAgent agent;

    [field: Header("Eventos")]
    [field: SerializeField]
    public UnityEvent<Vector2> OnMoved { get; private set; }

    [field: Header("Lógicos")] private static int currentDuck = 0;
    public bool isFollowing;
    private AudioSource audioSource;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<Movement>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;

        AddObject(colisor, this);
    }

    public void StartFollowing()
    {
        // Inicia o seguimento do pato ao jogador se a instância do PlayerBehaviour estiver presente.
        if (PlayerBehaviour.Instance)
        {
            alvo = PlayerBehaviour.Instance.transform;
            movement.SetFollowTarget(alvo);
            currentDuck++;
            countDucks.text = currentDuck.ToString();
            audioSource.Play();
            movement.SetFollowTarget(alvo);
            isFollowing = true;
        }
    }

    public virtual void  OnPlayerInteraction()
    {
        if (!isFollowing)
        {
            StartFollowing(); 
            RemoveObject(colisor);
        }
    }
}