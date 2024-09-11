using System;
using Actors;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Duck : MonoBehaviour, IInteractable, IMovement
{
    [SerializeField] private float velocidadePato; // Velocidade de movimento do pato.
    [SerializeField] private float distanciaMinima; // Distancia mínima do pato para o jogador.
    [SerializeField] private Rigidbody2D rb; // Rigidbody do pato.
    [SerializeField] private Transform alvo; // Alvo que o pato irá seguir.
    [SerializeField] private Collider2D areaInteracao; // Área de interação com o jogador.
    [SerializeField] private SpriteRenderer sprite; // Sprite do pato.
    [SerializeField] private Animator animator; // Animator do pato.
    [SerializeField] private TMP_Text countDucks; // Referência ao texto que mostra o número de patos seguindo. 
    [SerializeField] private AudioClip clip; // Referência ao AudioClip que será tocado.
    
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved {get; private set;} 

    private static int currentDuck = 0; // Variável estática para contar quantos patos estão seguindo
    private bool isFollowing; // Variável para saber se o pato está seguindo o jogador.
    private AudioSource audioSource; // Referência ao AudioSource criado dinamicamente.

    private void Start()
    {
        // Adiciona um AudioSource ao GameObject se ele ainda não tiver um
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip; // Atribui o AudioClip ao AudioSource
    }

    void Update() // Função de atualização.
    {
        if (isFollowing) 
        {
           FollowPlayer(); 
        }
    }
    
    public void StartFollowing() // Função para o pato começar a seguir o jogador.
    {
		if (PlayerBehaviour.Instance)
			alvo = PlayerBehaviour.Instance.GetFollowTarget(this);
        
		isFollowing = true; 
        areaInteracao.enabled = false; // Desabilitando a área de interação com o jogador.
        
        currentDuck++; // Incrementa a contagem de patos.
        countDucks.text = currentDuck.ToString(); // Atualizando o texto para mostrar a nova contagem.
        
        audioSource.Play(); // Toca o áudio quando o pato começa a seguir o jogador.
    }

    private void FollowPlayer() // Função para o pato seguir o jogador.
    {
        Vector2 posicaoAlvo = alvo.position;
        Vector2 posicaoPato = transform.position;
            
        float distancia = Vector2.Distance(posicaoPato, posicaoAlvo); // Calculando a distância entre o pato e o jogador.
        var direcao = Vector2.zero;

        if (distancia >= this.distanciaMinima) // Se o pato estiver distante do jogador.
        {
            direcao = (posicaoAlvo - posicaoPato).normalized; // Definindo e normalizando a direção para o pato seguir.
            direcao *= velocidadePato;
        }

        rb.linearVelocity = direcao;
        OnMoved.Invoke(direcao);
    }
    
    void OnTriggerStay2D(Collider2D colisor) // Função para quando o jogador estiver dentro na área de interação.
    {
        if (colisor.CompareTag("Player") && !isFollowing)
        {
            UIActionButton.instance.ShowButton(this); // Chamando a função para mostrar o botão.
        }
    }
    
    void OnTriggerExit2D(Collider2D colisor) // Função para quando o jogador sair da área de interação.
    {
        if (colisor.CompareTag("Player") && !isFollowing)
        {
            UIActionButton.instance.HideButton(); // Escondendo o botão.
        }
    }

    public void OnInteract() // Função para interagir com o pato.
    {
        StartFollowing();
    }
}
