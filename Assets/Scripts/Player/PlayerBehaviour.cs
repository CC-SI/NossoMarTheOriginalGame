using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace Player
{
	public class PlayerBehaviour : MonoBehaviour
	{
		readonly List<Duck> ducks = new();

		[field: Header("Componentes")]
		[field: SerializeField]
		public Movement Movement { get; private set; }
		[field: SerializeField]
		public InteractableZone InteractableZone { get; private set; }
		
		public static PlayerBehaviour Instance { get; private set; }

		public Transform GetFollowTarget(Duck duck)
		{
			Transform target = GetFollowTarget();

			if (!ducks.Contains(duck))
				ducks.Add(duck);

			return target;
		}

		private Transform GetFollowTarget()
		{
			if (ducks.Count < 1)
				return transform;

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