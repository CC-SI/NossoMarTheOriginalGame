using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2, bool> OnMoved { get; private set; }
    
    [SerializeField] private FixedJoystick joystick;

    private NavMeshAgent navMeshAgent;
    private Transform followTarget;
    private bool isInWater;

    private void Start()
    {
        // Inicializa o NavMeshAgent e o Animator
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        
        if (!joystick) return;
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        var direcao = new Vector3(horizontal, vertical, 0).normalized;

        if (direcao.magnitude >= 0.1f)
        {
            Vector3 targetPosition = transform.position + (direcao * (navMeshAgent.speed * Time.fixedDeltaTime));
            navMeshAgent.SetDestination(targetPosition);
        }
        else
        {
            navMeshAgent.ResetPath();
        }
        
        OnMoved.Invoke(direcao * navMeshAgent.velocity.magnitude, isInWater);
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
        
        OnMoved.Invoke(direcao, isInWater);
    }
    
    private void CheckAreaMask()
    {
        int WaterMask = 1 << 3;

        if (NavMesh.SamplePosition(navMeshAgent.transform.position, out NavMeshHit hit, 0.1f, WaterMask))
        {
            isInWater = true;
            return;
        }
        
        isInWater = false;
    }
}