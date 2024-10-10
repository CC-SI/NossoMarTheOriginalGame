using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    private Vector2 destination;
    private const float Speed = 4f;
    private CarSpawner spawner;

    private void Start()
    {
        RotateCar();
    }
    
    public void SetDestination(Vector2 target)
    {
        destination = target;
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