using System;
using System.Collections.Generic;
using Lixeira;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MiniGame
{
    public class MiniGame : MonoBehaviour
    {
        [SerializeField] private Button textBox;
        [SerializeField] private List<MiniGameObject> prefabs;

        private static readonly List<MiniGameObject> objects = new();
        private static TrashBinBehaviour TrashBin => TrashBinBehaviour.Instance;

        public static MiniGame Instance { get; private set; }

        public UnityEvent<string> onMessageUpdated;

        private bool isDuckCollect = false;
        public static bool isFinished = false;
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                StartGame();
                return;
            }
        
            Destroy(gameObject);
        }

        private void StartGame()
        {
            SpawnDraggableObjects();
            SpawnHiddenObject();
            textBox.onClick.AddListener(FinishGame);
        }

        protected static void AddObject(MiniGameObject miniGameObject)
        {
            objects.Add(miniGameObject);
        }

        private static void AddObjectByIndex(int index, MiniGameObject miniGameObject)
        {
            objects.Insert(index, miniGameObject);
        }

        private static void RemoveObject(MiniGameObject miniGameObject)
        {
            objects.Remove(miniGameObject);
        }
        
        public static IReadOnlyList<MiniGameObject> GetObjects()
        {
            return objects.AsReadOnly();
        }
        
        public static int GetObjectCount()
        {
            return objects.Count;
        }
        
        public static void UpdateTrashIndex(MiniGameObject trash)
        {
            RemoveObject(trash);
            AddObjectByIndex(0, trash);

            UpdateObjectsIndex();
        }
        
        private static void UpdateObjectsIndex()
        {
            var currentIndex = 0;
        
            foreach (var miniGameObject in GetObjects())
            {
                miniGameObject.Index = currentIndex++;
            }
        }

        public void CheckObjective(Objective objective)
        {
            if (!isDuckCollect)
            {
                if (objective && objective.IsCollected)
                {
                    isDuckCollect = true; 
                }
                else
                {
                    onMessageUpdated.Invoke("Arraste os lixos para encontrar e pegar o pato");
                    return;
                }
            }

            
            if (!IsAllTrashsInTrashBin())
            {
                onMessageUpdated.Invoke("Agora coloque os lixos de volta na lixeira");
                return;
            }
            
            onMessageUpdated.Invoke("Parabéns! Você resgatou o pato.");
            textBox.gameObject.SetActive(true);
        }
        
        void FinishGame()
        {
            isFinished = true;
            GameManager.LoadGame(true);
        }

        public bool IsObjectSuperimposed(Bounds bounds, int index)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (bounds.Intersects(objects[i].Bounds) && objects[i].Bounds != bounds)
                    return true;
            }
        
            return false;
        }
        
        public void AlertSuperimposing(Bounds bounds, int index)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (bounds.Intersects(objects[i].Bounds) && objects[i].Bounds != bounds)
                {
                    var objectDragAndDrop = objects[i].GetComponent<IDragAndDrop>();
                    objectDragAndDrop?.OnSuperimposing.Invoke();
                }
            }
        }
        
        public bool IsAllTrashsInTrashBin()
        {
            foreach (var trash in GetObjects())
            {
                if (!TrashBin.ContainsObject(trash.Bounds)) return false;
            }
            
            return true;
        }
        
        private void SpawnDraggableObjects()
        {
            var spawnAmount = Random.Range(8, 12);
            
            for (var i = 0; i < spawnAmount; i++)
            {
                var randomPrefab = prefabs[Random.Range(0, prefabs.Count-1)];
                var randomPosition = new Vector3(Random.Range((float)-2, (float)2), Random.Range((float)-2, (float)1), 0);
                Instantiate(randomPrefab, randomPosition,  Quaternion.Euler(0, 0, Random.Range(0, 360)), transform);
            }
        }

        private void SpawnHiddenObject()
        {
            var randomPosition = new Vector3(Random.Range(-1, 1), Random.Range((float)-1, (float)0.5), 0);
            Instantiate(prefabs[^1], randomPosition,  Quaternion.Euler(0, 0, Random.Range(0, 360)), transform);
        }
    }
}