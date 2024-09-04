using System.Collections.Generic;
using UnityEngine;

namespace NM.Player
{
	public class PlayerBehaviour : MonoBehaviour
	{
		readonly List<Duck> ducks = new();

		public static PlayerBehaviour Instance { get; private set; }

		public Transform GetFollowTarget(Duck duck)
		{
			Transform target = GetFollowTarget();

			if (!ducks.Contains(duck))
				ducks.Add(duck);

			return target;
		}

		public Transform GetFollowTarget()
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
	}
}