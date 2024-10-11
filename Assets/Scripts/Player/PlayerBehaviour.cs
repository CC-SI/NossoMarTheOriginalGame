using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        readonly List<Duck.Duck> ducks = new();
        readonly List<Shovel.Shovel> shovels = new(); 

        public static PlayerBehaviour Instance { get; private set; }

        public Transform GetFollowTarget(Duck.Duck duck)
        {
            Transform target = GetFollowTarget();

            if (!ducks.Contains(duck))
                ducks.Add(duck);

            return target;
        }

        public Transform GetFollowTarget(Shovel.Shovel shovel)
        {
            Transform target = GetFollowTarget();

            if (!shovels.Contains(shovel))
                shovels.Add(shovel);

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
    }
}