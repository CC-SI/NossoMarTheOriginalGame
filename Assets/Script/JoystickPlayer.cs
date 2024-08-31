using UnityEngine;

public class Joy : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float velocidadeMovimento = 5f;
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        
        Vector2 direcao = new Vector2(horizontal, vertical).normalized;

        rb.linearVelocity = direcao * velocidadeMovimento;
    }
}
