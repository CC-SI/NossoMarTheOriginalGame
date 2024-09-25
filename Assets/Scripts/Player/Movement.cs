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
    
    
    // private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
    
        Vector3 direcao = new Vector3(horizontal, vertical, 0).normalized;

        if (direcao.magnitude >= 0.1f)
        {
            navMeshAgent.Move(direcao * velocidadeMovimento * Time.fixedDeltaTime);
            OnMoved.Invoke(new Vector2(horizontal, vertical) * velocidadeMovimento);
        }
        else
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.ResetPath();
            OnMoved.Invoke(Vector2.zero);
        }
        
    }
}
