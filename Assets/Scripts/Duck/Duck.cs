using System;
using Actors;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Duck : MonoBehaviour, IInteractable, IMovement
{
    [field: Header("Componentes Externos")]
    [SerializeField] private Transform alvo;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioSource audioSource;
    
    [field: Header("Componentes Internos")]
    private Rigidbody2D rb;
    private Collider2D areaInteracao;
    private Animator animator; 
    private NavMeshAgent agent;
    
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved {get; private set;}
    
    [field: Header("Lógicos")]
    private static int currentDuck = 0;
    private bool isFollowing; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        areaInteracao = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        areaInteracao.enabled = false;
        
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
        
        float distancia = Vector2.Distance(posicaoPato, posicaoAlvo); 
        
        var direcao = ((posicaoAlvo - posicaoPato).normalized) * agent.velocity.magnitude;
        
        agent.SetDestination(posicaoAlvo);
        
        OnMoved.Invoke(direcao);
    }
    
    /// <summary>
    /// Função para quando o jogador estiver dentro na área de interação, chamando a função para mostrar o botão.
    /// </summary>
    /// <param name="colisor"></param>
    void OnTriggerStay2D(Collider2D colisor) 
    {
        if (colisor.CompareTag("Player") && !isFollowing)
        {
            UIActionButton.instance.ShowButton(this); 
        }
    }
    
    /// <summary>
    /// Função para quando o jogador sair da área de interação, chamando a função para esconder o botão.
    /// </summary>
    /// <param name="colisor"></param>
    void OnTriggerExit2D(Collider2D colisor)
    {
        if (colisor.CompareTag("Player") && !isFollowing)
        {
            UIActionButton.instance.HideButton();
        }
    }

    /// <summary>
    /// Função que o ActionButton usa para interagir com o pato. Definindo que nessa interação o pato deve seguir o jogador.
    /// </summary>
    public void OnInteract() // Função para interagir com o pato.
    {
        StartFollowing();
    }
}
