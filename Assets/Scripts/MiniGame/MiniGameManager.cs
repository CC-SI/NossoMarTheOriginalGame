using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGame
{
    public class MiniGameManager : MonoBehaviour
    {
        [SerializeField] private List<DraggableObject> draggableObjectPrefabs;
        [SerializeField] private HiddenObject hiddenObjectPrefab;
        [SerializeField] private Transform objectsParent;

        private void Awake()
        {
            SpawnDraggableObjects();
            SpawnHiddenObject();
        }

        // protected static int SortByLowZ(TrashBehaviour a, TrashBehaviour b)
        // {
        //     if (a.transform.position.z.Equals(b.transform.position.z))
        //     {
        //         return 0;
        //     }
        //
        //     if (a.transform.position.z < b.transform.position.z)
        //     {
        //         //a.TrashIndex++;
        //         return -1;
        //     }
        //
        //     //a.TrashIndex--;
        //     return 1;
        // }
        
        private void SpawnDraggableObjects()
        {
            var spawnAmount = Random.Range(10, 20);
            
            for (var i = 0; i < spawnAmount; i++)
            {
                var randomPrefab = draggableObjectPrefabs[Random.Range(0, draggableObjectPrefabs.Count)];;
                var randomPosition = new Vector3(Random.Range((float)-2, (float)2), Random.Range((float)-2, (float)1), 0);
                Instantiate(randomPrefab, randomPosition,  Quaternion.Euler(0, 0, Random.Range(0, 360)), objectsParent);
            }
        }

        private void SpawnHiddenObject()
        {
            var randomPosition = new Vector3(Random.Range(-1, 1), Random.Range((float)-1, (float)0.5), 0);
            Instantiate(hiddenObjectPrefab, randomPosition,  Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }
}
