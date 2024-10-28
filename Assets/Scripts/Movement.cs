using Actors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Movement : MonoBehaviour, IMovement
{
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2, bool> OnMoved { get; private set; }
    
    //[SerializeField] private FixedJoystick joystick;

    private NavMeshAgent navMeshAgent;
    private Transform followTarget;
    private bool isInWater;
    Vector3 direcao;
    
    public void Move(Vector2 direction)
    {
        direcao = direction;
    }

    private void Start()
    {
        // Inicializa o NavMeshAgent e o Animator
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        CheckWaterMask();
        
        if (followTarget)
        {
            FollowTarget();
            return;
        }
        
        // if (!joystick) return;
        // float horizontal = joystick.Horizontal;
        // float vertical = joystick.Vertical;
        //
        // var direcao = new Vector3(horizontal, vertical, 0).normalized;

        if (direcao.magnitude >= 0.1f)
        {
            //Vector3 targetPosition = transform.position + direcao;
            //navMeshAgent.SetDestination(targetPosition);
            
            navMeshAgent.velocity = direcao * navMeshAgent.speed;
        }
        else
        {
            navMeshAgent.ResetPath();
        }
        
        OnMoved.Invoke(navMeshAgent.velocity, isInWater);
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void FollowTarget()
    {
        var posicaoAlvo = followTarget.position;
        var posicaoPato = transform.position;

        _ = navMeshAgent.SetDestination(posicaoAlvo);
        
        OnMoved.Invoke(navMeshAgent.velocity, isInWater);
    }

    public bool IsPlayerWalking()
    {
        return navMeshAgent.velocity.magnitude > 0.1f;
    }
    
    private void CheckWaterMask()
    {
        var waterMask = NavMesh.GetAreaFromName("Water");

        isInWater = !NavMesh.SamplePosition(navMeshAgent.transform.position, out _, 0.1f, waterMask);
    }

    void OnDisable()
    {
        direcao = Vector3.zero;
    }
}