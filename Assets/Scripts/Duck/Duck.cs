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
    private int tentativasDeCaptura = 0; // Contador de tentativas de captura.
    

    private void Start()
    {
        // Inicializa os componentes e configura o pato.
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
        // Inicia o seguimento do pato ao jogador.
        if (PlayerBehaviour.Instance)
        {
            if (gameObject.CompareTag("DuckDefault"))
            {
                CapturaPatoSemNarrativa(); // Captura o pato sem diálogo.
            }
            else
            {
                TentativasDeCapturas(); // Incrementa a tentativa de captura.
            }
        }
    }

    private void TentativasDeCapturas()
    {
        // Incrementa o contador de tentativas de captura.
        tentativasDeCaptura++;
        Debug.Log(tentativasDeCaptura);
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
        // Atualiza a movimentação do pato se estiver seguindo o jogador.
        if (isFollowing)
        {
            movement.FollowTarget(); 
        }
    }

    public void OnPlayerInteraction()
    {
        // Lida com a interação do jogador com o pato.
        if (!isFollowing)
        {   
            StartFollowing(); // Inicia o seguimento se o pato não estiver seguindo.

            if (gameObject.CompareTag("DuckDefault"))
            {
                RemoveObject(colisor); // Remove o pato se for do tipo padrão.
            }
            else
            {
                if (DialogManager.Instance.IsDialogFinshed)
                {
                    Debug.Log("O diálogo está finalizado."); // Log se o diálogo foi finalizado.
                }
                else
                {
                    Debug.Log("O diálogo não está finalizado."); // Log se o diálogo não foi finalizado.
                }
            }
        }
    }

    private void OnMovedHandler(Vector2 direction)
    {
        // Rotaciona o pato com base na direção do movimento.
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x); // Inverte a escala no eixo X.
            transform.localScale = scale; // Aplica a nova escala.
        }
    }
}
