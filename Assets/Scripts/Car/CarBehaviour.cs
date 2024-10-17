using UnityEngine;
using UnityEngine.Serialization;

public class CarBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("Speed")] [SerializeField] private float speed = 4f;

    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position -= transform.right * (speed * Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D _)
    {
        transform.position = startPosition;
    }
}