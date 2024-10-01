using System;
using Actors;
using Interactions;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Duck : InteractableObject, IInteractable, IMovement
{
    [field: Header("Componentes Externos")]
    [SerializeField] private Transform alvo;
    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioClip clip;
    
    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D colisor;
    private Animator animator; 
    private NavMeshAgent agent;
    
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved {get; private set;}
    
    [field: Header("Lógicos")]
    private static int currentDuck = 0;
    public bool isFollowing; 
    private AudioSource audioSource; // Referência ao AudioSource criado dinamicamente.
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        
        AddObject(colisor, this);
    }

    void Update()
    {
        if (isFollowing) 
        {
           FollowPlayer(); 
        }
    }
    
    /// <summary>
    /// Função para o pato começar a seguir o jogador.
    /// </summary>
    public void StartFollowing() 
    {
		if (PlayerBehaviour.Instance)
			alvo = PlayerBehaviour.Instance.GetFollowTarget(this);
        
		isFollowing = true;
        
        currentDuck++;
        countDucks.text = currentDuck.ToString();
        
        audioSource.Play(); 
    }

    /// <summary>
    /// Função para a movimentação e chamada da animação do pato.
    /// </summary>
    private void FollowPlayer() 
    {
        Vector2 posicaoAlvo = alvo.position;
        Vector2 posicaoPato = transform.position;
        
        var direcao = ((posicaoAlvo - posicaoPato).normalized) * agent.velocity.magnitude;
        
        agent.SetDestination(posicaoAlvo);
        
        OnMoved.Invoke(direcao);
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
