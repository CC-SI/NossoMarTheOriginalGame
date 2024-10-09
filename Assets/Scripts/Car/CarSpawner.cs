using UnityEngine;
using UnityEngine.Serialization;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private  GameObject car;

    [field: Header("Direção na mão esquerda")]
    [SerializeField] private Transform leftSpawn;

    [field: Header("Direção na mão direita")]
    [SerializeField] private Transform rightSpawn;
    
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
        
        spawnTimer = 0;
        spawnInterval = Random.Range(2, 5);
        
        if (Random.Range(0, 2) == 0)
        {
            Instantiate(car, leftSpawn.position, Quaternion.identity);
            return;
        }
        
        Instantiate(car, rightSpawn.position, Quaternion.identity);
    }
}