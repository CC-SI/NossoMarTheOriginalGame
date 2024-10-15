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
    private int tentativasDeCaptura = 0; 

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
        // Inicia o seguimento do pato ao jogador se a instância do PlayerBehaviour estiver presente.
        if (PlayerBehaviour.Instance)
        {
            alvo = PlayerBehaviour.Instance.transform; 
            isFollowing = true; 
            
            if (gameObject.CompareTag("DuckDefault"))
            {
                CapturaPatoSemNarrativa();
            }
            else
            {
                TentativasDeCapturas(); 
            }
        }
    }

    private void TentativasDeCapturas()
    {
        // Incrementa o contador de tentativas de captura.
        tentativasDeCaptura++;
        Debug.Log(tentativasDeCaptura); // Log da quantidade de tentativas de captura.
    }
    
    public void CaptureDuck()
    {
        // Captura o pato e atualiza a contagem de patos.
        isFollowing = true; 
        currentDuck++; 
        countDucks.text = currentDuck.ToString();
        audioSource.Play(); 
        movement.SetFollowTarget(alvo); 

        colisor.enabled = false; // Desabilita o colisor após a captura.
    }
    

    private void CapturaPatoSemNarrativa()
    {
        // Captura o pato sem narrativa e atualiza o alvo e contagem.
        alvo = PlayerBehaviour.Instance.GetFollowTarget(this); // Obtém o alvo do jogador.
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
            movement.FollowTarget(); // Chama o método para seguir o alvo.
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
                // Verifica se o diálogo foi finalizado antes de capturar o pato.
                if (DialogManager.Instance.IsDialogFinshed)
                {
                    CaptureDuck(); // Captura o pato.
                    RemoveObject(colisor); // Remove o pato após captura.
                }
                else
                {
                    Debug.Log("O diálogo não está finalizado."); 
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
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
