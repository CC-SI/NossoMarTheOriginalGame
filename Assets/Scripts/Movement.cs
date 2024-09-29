using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float velocidadeMovimento = 5f;
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }

    private NavMeshAgent navMeshAgent;
    private Transform followTarget;
    private Animator animator;

    void Start()
    {
        // Inicializa o NavMeshAgent e o Animator
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Se há um alvo a seguir, chama FollowTarget
        if (followTarget != null)
        {
            FollowTarget();
        }
        // Caso contrário, usa o joystick para movimentação
        else if (joystick != null)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direcao = new Vector3(horizontal, vertical, 0).normalized;

            if (direcao.magnitude >= 0.1f)
            {
                // Move o agente na direção do joystick
                navMeshAgent.Move(direcao * velocidadeMovimento * Time.fixedDeltaTime);
                OnMoved.Invoke(new Vector2(horizontal, vertical) * velocidadeMovimento);
            }
            else
            {
                // Para o movimento se o joystick não estiver sendo usado
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.ResetPath();
                OnMoved.Invoke(Vector2.zero);
            }
        }
    }

    public void SetFollowTarget(Transform target)
    {
        // Define o alvo a ser seguido
        followTarget = target;
    }

    private void FollowTarget()
    {
        if (followTarget != null)
        {
            // Calcula a direção para o alvo e move o agente
            Vector2 posicaoAlvo = followTarget.position;
            Vector2 posicaoPato = transform.position;

            var direcao = ((posicaoAlvo - posicaoPato).normalized) * navMeshAgent.velocity.magnitude;

            navMeshAgent.SetDestination(posicaoAlvo);

            OnMoved.Invoke(direcao);
            
            // Atualiza a animação de movimento
            animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0);
        }
    }
}