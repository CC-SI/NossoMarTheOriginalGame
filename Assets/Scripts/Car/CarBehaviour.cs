using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private Vector2 destination;
    private const float Speed = 4f;

    private void Start()
    {
        destination = transform.position.y > 0 ? new Vector2((float)-33.8, (float)-22.9) : new Vector2((float)-32.33, (float)21.58);

        RotateCar();
    }
    
    private void RotateCar()
    {
        Vector2 direction = (destination - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, destination) < 0.1f)
            Destroy(gameObject);
    }
}