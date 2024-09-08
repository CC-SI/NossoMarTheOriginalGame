using Actors;
using UnityEngine;
using UnityEngine.Events;

public class Joy : MonoBehaviour, IMovement
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float velocidadeMovimento = 5f;
    [field: Header("Eventos")]
    [field: SerializeField] public UnityEvent<Vector2> OnMoved { get; private set; }
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        
        Vector2 direcao = new Vector2(horizontal, vertical).normalized;

        rb.linearVelocity = direcao * velocidadeMovimento;
        OnMoved.Invoke(direcao * velocidadeMovimento);
    }
}
