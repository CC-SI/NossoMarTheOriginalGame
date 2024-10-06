using System;
using Actors;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Duck : MonoBehaviour, IInteractableObjects, IMovement
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
    private Movement movement;

    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }

    [field: Header("Lógicos")]
    private static int currentDuck = 0;
    public bool isFollowing;
    private AudioSource audioSource;

    private void Start()
    {
        // Inicializa os componentes do pato
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<Movement>();

        // Configura o NavMeshAgent para não atualizar a rotação e o eixo Y
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Adiciona um AudioSource dinamicamente e configura o clip de áudio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        
        // Adiciona um listener para o evento OnMoved do componente Movement
        movement.OnMoved.AddListener(OnMovedHandler);
        
        AddObject(colisor, this);
    }

    /// <summary>
    /// Função para o pato começar a seguir o jogador.
    /// </summary>
    public void StartFollowing()
    {
        // Define o alvo a ser seguido pelo pato
        if (PlayerBehaviour.Instance)
            alvo = PlayerBehaviour.Instance.GetFollowTarget(this);

        isFollowing = true;

        // Atualiza a contagem de patos seguindo o jogador
        currentDuck++;
        countDucks.text = currentDuck.ToString();

        // Toca o som de seguir
        audioSource.Play();

        // Define o alvo de movimentação no componente Movement
        movement.SetFollowTarget(alvo);
    }

    public void OnPlayerInteract()
    {
        // Inicia o seguimento se o pato ainda não estiver seguindo
        if (!isFollowing)
        {
            StartFollowing();
            RemoveObject(colisor);
        }
    }

    public bool CanInteract()
    {
        // Retorna se o pato pode ser interagido (se não estiver seguindo)
        return !isFollowing;
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