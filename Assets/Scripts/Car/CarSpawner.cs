using UnityEngine;
using UnityEngine.Serialization;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private  GameObject car;

    [field: Header("Direção na mão esquerda")]
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform leftDestroyer;
    
    [field: Header("Direção na mão direita")]
    [SerializeField] private Transform rightSpawn;
    [SerializeField] private Transform rightDestroyer;
    
    private float spawnTimer;
    private float spawnInterval;

    private void Start()
    {
        spawnInterval = Random.Range(2, 4);
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (!(spawnTimer >= spawnInterval)) return;
        
        SpawnCar();
        spawnTimer = 0;
        spawnInterval = Random.Range(2, 5);
    }
    
    private void SpawnCar()
    {
        Transform spawnPoint, endPoint;
        
        if (Random.Range(0, 2) == 0)
        {
            spawnPoint = leftSpawn;
            endPoint = leftDestroyer;
        }
        else
        {
            spawnPoint = rightSpawn;
            endPoint = rightDestroyer;
        }
        
        GameObject newCar = Instantiate(car, spawnPoint.position, Quaternion.identity);
        newCar.GetComponent<CarBehaviour>().SetDestination(endPoint.position);
    }
}