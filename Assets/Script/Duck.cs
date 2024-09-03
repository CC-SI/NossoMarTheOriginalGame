using System;
using UnityEngine;

public class Duck : MonoBehaviour, IInteractable
{
    [SerializeField] private float velocidadePato; // Velocidade de movimento do pato.
    [SerializeField] private float distanciaMinima; // Distancia mínima do pato para o jogador.
    [SerializeField] private Rigidbody2D rb; // Rigidbody do pato.
    [SerializeField] private Transform alvo; // Alvo que o pato irá seguir.
    [SerializeField] private Collider2D areaInteracao; // Área de interação com o jogador.
    [SerializeField] private SpriteRenderer sprite; // Sprite do pato.
    [SerializeField] private Animator animator; // Animator do pato.

    private bool isFollowing; // Variável para saber se o pato está seguindo o jogador.
    
    void Update() // Função de atualização.
    {
        if (isFollowing) 
        {
           FollowPlayer(); 
        }
    }
    
    public void StartFollowing() // Função para o pato começar a seguir o jogador.
    {
        isFollowing = true; 
        areaInteracao.enabled = false; // Desabilitando a área de interação com o jogador.
    }

    private void FollowPlayer() // Função para o pato seguir o jogador.
    {
        Vector2 posicaoAlvo = this.alvo.position;
        Vector2 posicaoPato = this.transform.position;
            
        float distancia = Vector2.Distance(posicaoPato, posicaoAlvo); // Calculando a distância entre o pato e o jogador.

        if (distancia >= this.distanciaMinima) // Se o pato estiver distante do jogador.
        {
            Vector2 direcao = (posicaoAlvo - posicaoPato).normalized; // Definindo e normalizando a direção para o pato seguir.
            this.rb.linearVelocity = (direcao * velocidadePato); // Movendo o pato.
            
            this.animator.SetBool("isWalking", true);
        }
        else
        {
            this.rb.linearVelocity = Vector2.zero;
            this.animator.SetBool("isWalking", false);
        }

        FlipSprite();
    }

    private void FlipSprite() // Função para virar o sprite do pato.
    {
        if (this.rb.linearVelocity.x > 0)
        {
            this.sprite.flipX = false;
        }
        else if (this.rb.linearVelocity.x < 0)
        {
            this.sprite.flipX = true; // Virando o sprite do pato.
        }
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
