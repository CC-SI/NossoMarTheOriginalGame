using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float velocidadeMovimento = 30f;
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
        if (followTarget != null)
        {
            FollowTarget();
        }
        
        else if (joystick != null)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direcao = new Vector3(horizontal, vertical, 0).normalized;

            if (direcao.magnitude >= 0.1f)
            {
                Vector3 targetPosition = transform.position + (direcao * velocidadeMovimento * Time.deltaTime);
                navMeshAgent.SetDestination(targetPosition);
                
                OnMoved.Invoke(new Vector2(horizontal, vertical));

                if (horizontal != 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Sign(horizontal) * Mathf.Abs(scale.x);  
                    transform.localScale = scale;
                }
            }
            else
            {
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.ResetPath();
                OnMoved.Invoke(Vector2.zero);
            }
            
            animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0);
        }
    }



    public void SetFollowTarget(Transform target)
    {
        // Define o alvo a ser seguido
        followTarget = target;
    }

    public void FollowTarget()
    {
        if (followTarget != null)
        {
            Vector2 posicaoAlvo = followTarget.position;
            Vector2 posicaoPato = transform.position;

            var direcao = ((posicaoAlvo - posicaoPato).normalized) * navMeshAgent.velocity.magnitude;

            navMeshAgent.SetDestination(posicaoAlvo);
            
            // Atualiza a animação de movimento
            animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0);
            
            OnMoved.Invoke(direcao);
        }
    }
    
}