using System;
using Actors;
using Dialog;
using Interaction;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Duck : InteractableObject, IInteraction, IMovement
{
    [field: Header("Componentes Externos")]
    [SerializeField] private Transform alvo;
    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioClip clip;
    private Movement movement;
    
    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D colisor;
    private Animator animator; 
    private NavMeshAgent agent;
    
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }
    
    [field: Header("Lógicos")]
    private static int currentDuck = 0;
    public bool isFollowing; 
    private AudioSource audioSource;

    [field: Header("Dialogo")]
    // private DialogManager dialogManager;
    
    private int currentDialogueIndex = 0;
    private int previousDialogueIndex = -1;
    
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
        
        if (movement != null)
        {
            movement.OnMoved.AddListener(OnMovedHandler);
        }   
    }
    
    /// <summary>
    /// Função para o pato começar a seguir o jogador.
    /// </summary>
    public void StartFollowing() 
    {
        if (PlayerBehaviour.Instance)
        {
            if (gameObject.CompareTag("DuckDefault"))
            {
                CapturaPatoSemNarrativa();
            }
            else
            {
               Debug.Log("Pato não encontrado");
            }
        }
    }
    
    private void CapturaPatoComNarrativa()
    {
            alvo = PlayerBehaviour.Instance.GetFollowTarget(this);
            isFollowing = true;
    
            currentDuck++;
            countDucks.text = currentDuck.ToString();
        
            audioSource.Play(); 
            movement.SetFollowTarget(alvo);
    }

    private void CapturaPatoSemNarrativa()
    {
        alvo = PlayerBehaviour.Instance.GetFollowTarget(this);
        isFollowing = true;

        currentDuck++;
        countDucks.text = currentDuck.ToString();
    
        audioSource.Play(); 
        movement.SetFollowTarget(alvo);
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

    private void OnMovedHandler(Vector2 direction)
    {
        // Gira o pato para a esquerda ou direita com base na direção do movimento
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
