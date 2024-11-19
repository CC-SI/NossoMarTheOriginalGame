using System.Collections.Generic;
using Duck;
using Interaction;
using Serialization;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour, ISerializable
    {
        readonly List<DuckBehavior> ducks = new();

        [field: Header("Componentes")]
        [field: SerializeField]
        public Movement Movement { get; private set; }
        [field: SerializeField]
        public InteractableZone InteractableZone { get; private set; }

        public static PlayerBehaviour Instance { get; private set; }

        public Transform GetFollowTarget(DuckBehavior duckBehavior)
        {
            Transform target = GetFollowTarget();

            if (!ducks.Contains(duckBehavior))
                ducks.Add(duckBehavior);

            return target;
        }

        public Transform GetFollowTarget()
        {
            if (ducks.Count < 1 || ducks.Count % 6 == 0)
                return transform;

            return ducks[^1].transform; 
        }

        public void Save(SaveData data)
        {
            data.playerPosition = transform.position;
        }

        public void Load(SaveData data)
        {
            transform.position = data.playerPosition;
        }

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                GameManager.Subscribe(this);
                return;
            }

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameManager.Unsubscribe(this);
        }

#if UNITY_EDITOR
        void Reset()
        {
            Movement = GetComponent<Movement>();
            InteractableZone = GetComponentInChildren<InteractableZone>(true);
        }
#endif
    }
}
