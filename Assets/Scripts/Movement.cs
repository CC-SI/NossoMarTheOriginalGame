using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }
    
    [SerializeField] private FixedJoystick joystick;

    private NavMeshAgent navMeshAgent;
    private Transform followTarget;
    private Animator animator;

    private void Start()
    {
        // Inicializa o NavMeshAgent e o Animator
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        CheckAreaMask();
        if (followTarget)
        {
            FollowTarget();
            return;
        }
        // Caso contrário, usa o joystick para movimentação
        if (!joystick) return;
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        var direcao = new Vector3(horizontal, vertical, 0).normalized;

        if (direcao.magnitude >= 0.1f)
        {
            Vector3 targetPosition = transform.position + (direcao * (navMeshAgent.speed * Time.fixedDeltaTime));
            navMeshAgent.SetDestination(targetPosition);
            OnMoved.Invoke(direcao * navMeshAgent.velocity.magnitude);
        }
        else
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.ResetPath();
            OnMoved.Invoke(Vector2.zero);
        }
    }


    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void FollowTarget()
    {
        Vector2 posicaoAlvo = followTarget.position;
        Vector2 posicaoPato = transform.position;

        var direcao = ((posicaoAlvo - posicaoPato).normalized) * navMeshAgent.velocity.magnitude;

        navMeshAgent.SetDestination(posicaoAlvo);
        
        OnMoved.Invoke(direcao);
    }
    
    private void CheckAreaMask()
    {
        int WaterMask = 1 << 3;

        if (NavMesh.SamplePosition(navMeshAgent.transform.position, out NavMeshHit hit, 0.1f, WaterMask))
        {
            animator.SetBool("isSwimming", true);
            return;
        }
        
        animator.SetBool("isSwimming", false);
    }
}