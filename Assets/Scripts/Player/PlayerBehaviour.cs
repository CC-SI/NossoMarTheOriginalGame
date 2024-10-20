using System.Collections.Generic;
using Interaction;
using Interaction;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        readonly List<DuckBehavior> ducks = new();
        readonly List<Shovel.Shovel> shovels = new(); 

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
            if (ducks.Count + shovels.Count < 1)
                return transform;

            if (shovels.Count > 0)
                return shovels[^1].transform; 

            return ducks[^1].transform; 
        }

		void Awake()
		{
			if (!Instance)
			{
				Instance = this;
				return;
			}

			Destroy(gameObject);
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