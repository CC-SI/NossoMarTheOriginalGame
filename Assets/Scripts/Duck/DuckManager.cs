using System.Collections.Generic;
using Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Duck
{
    public class DuckManager : MonoBehaviour, ISerializable
    {
        [SerializeField] private List<DuckBehavior> Ducks = new();

        private static DuckManager Instance;
        
        public static int RescuedCount { get; private set; }

        public static int TotalCount
        {
            get
            {
                if (!Instance)
                    return 0;
                
                return Instance.Ducks.Count;
            }
        }

        void OnDuckRescued()
        {
            RescuedCount++;
        }
        
        public static void SetEndDestination(Transform endPoint)
        {
            foreach (var duck in Instance.Ducks)
            {
                if(!duck.IsRescued)
                    continue;
                
                duck.EndTarget = endPoint;
            }
        }
        
        public static void Quack()
        {
            if (!Instance) return;
            
            foreach (DuckBehavior duck in Instance.Ducks)
            {
                if(!duck.IsRescued
                   || duck.movement.IsOnWater)
                    continue;
            
                float variation = Random.value;

                duck.audioSource.pitch = Mathf.Lerp(1, 1.1f, variation);
                duck.Invoke(nameof(duck.Quack), variation);
            }
        }
        
        public void Save(SaveData data)
        {
            for (var index = 0; index < Ducks.Count; index++)
            {
                var duck = Ducks[index];
                
                if (duck.IsRescued)
                    data.ducks.Add(index);
            }
        }
        
        public void Load(SaveData data)
        {
            for (var i = 0; i < data.ducks.Count; i++)
            {
                int index = data.ducks[i];
                if(index < 0 || index >= Ducks.Count) continue;
                
                var duck = Ducks[index];
                
                duck.StartFollowing();
            }
        }
        
        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            GameManager.Subscribe(this);
            
#if UNITY_EDITOR
            RenameDucksByIndex();
#endif
            
            DuckBehavior.OnDuckRescued += OnDuckRescued;
        }

        void OnDestroy()
        {
            GameManager.Unsubscribe(this);
            RescuedCount = 0;
            DuckBehavior.OnDuckRescued -= OnDuckRescued;
        }
        
#if UNITY_EDITOR
        [ContextMenu("Renomear Patos")]
        void RenameDucksByIndex()
        {
            for (int i = 0; i < Ducks.Count; i++)
            {
                Ducks[i].name = $"Pato {i}";
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
#if UNITY_EDITOR
        [ContextMenu("Pegar Patos")]
        void GetDucks()
        {
            Ducks.Clear();
            var ducks = GetComponentsInChildren<DuckBehavior>(true);
            Ducks.AddRange(ducks);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}