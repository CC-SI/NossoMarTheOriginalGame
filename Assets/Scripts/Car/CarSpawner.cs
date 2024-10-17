using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private  CarBehaviour car;

    [field: Header("Direção na mão esquerda")]
    [SerializeField] private Transform leftSpawn;
    
    [field: Header("Direção na mão direita")]
    [SerializeField] private Transform rightSpawn;
    
    [SerializeField] private int maxCars;
    
    private float spawnTimer;
    private float spawnInterval;
    private int spawnCount;

    private void Start()
    {
        spawnInterval = Random.Range(2, 4);
    }

    private void Update()
    {
        if (spawnCount == maxCars) return;
        
        spawnTimer += Time.deltaTime;

        if (!(spawnTimer >= spawnInterval)) return;
        
        spawnCount++;
        SpawnCar();
        
        spawnTimer = 0;
        spawnInterval = Random.Range(2, 4);
    }
    
    private void SpawnCar()
    {
        if (spawnCount % 2 == 0)
        {
            Instantiate(car, leftSpawn.position, leftSpawn.rotation);
            return;
        }
        
        Instantiate(car, rightSpawn.position, rightSpawn.rotation);
    }
}