using System;
using System.Collections.Generic;
using Interaction;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Duck
{
	public class DuckBehavior : InteractableObject, IInteraction
	{
		const float WanderTime = 10;
		static readonly List<DuckBehavior> Ducks = new();
    
		public static event Action<int> OnDuckRescued;
    
		Transform alvo;
   
		[field: Header("Componentes")]
		[field: SerializeField]
		public Collider2D colisor { get; private set; }
   
		[field: SerializeField]
		public Movement movement { get; private set; }
    
		[field: SerializeField]
		public AudioSource audioSource {get; private set;}
    
		protected bool IsFollowing;
		float originalSpeed;
    
		PlayerBehaviour Player => PlayerBehaviour.Instance;
    
		public static int Rescued { get; private set; }
		public static int TotalCount => Ducks.Count;

		public bool IsRescued
		{
			get => IsFollowing;
			private set
			{
				if(value)
					Rescued++;
				else if (IsRescued)
					Rescued--;
            
				IsFollowing = value;
				OnDuckRescued?.Invoke(Rescued);
			}
		}
    
		public int Index { get; private set; }

		public static void Quack()
		{
			foreach (DuckBehavior duck in Ducks)
			{
				if(!duck.IsRescued)
					continue;
            
				float variation = Random.value;

				duck.audioSource.pitch = Mathf.Lerp(1, 1.1f, variation);;
				duck.audioSource.volume = Mathf.Lerp(.9f, 1, variation);
				duck.Invoke(nameof(QuackSound), variation);
			}
		}

		void QuackSound()
		{
			audioSource.Play();
		}
    
		private void Start()
		{
			AddObject(colisor, this);
			Player.Movement.OnMoved.AddListener(OnPlayerMoved);
		}
		void StartFollowing()
		{
			alvo = Player.GetFollowTarget(this);
        
			if (movement)
				movement.SetFollowTarget(alvo);

			IsRescued = true;
		}
    
		public virtual void  OnPlayerInteraction()
		{
			if (IsFollowing)
				return;
        
			StartFollowing();
			RemoveObject(colisor);
		}
    
		void OnPlayerMoved(Vector2 direction, bool isSwimming)
		{
			if (!IsRescued)
				return;
        
			bool playerStopped = Mathf.Approximately(direction.sqrMagnitude, 0);
			CancelInvoke();
			movement.Speed = originalSpeed;

			movement.SetFollowTarget(playerStopped ? Player.transform : alvo);
        
			DelayedWander();
		}
    
		void DelayedWander()
		{
			Invoke(nameof(Wander), 1 + Random.value * WanderTime);
		}

		void Wander()
		{
			Vector3 position = Random.insideUnitCircle * 5 + (Vector2)Player.transform.position;
			if (movement.MoveTo(position))
			{
				DelayedWander();
				movement.Speed = originalSpeed + Random.value;
			}
			else
				Wander();
		}

		void Awake()
		{
			Index = TotalCount;
			Ducks.Add(this);
			originalSpeed = movement.Speed;
		}

		void OnDestroy()
		{
			Ducks.Remove(this);
		}
    
#if UNITY_EDITOR
		void Reset()
		{
			colisor = GetComponent<Collider2D>();
			movement = GetComponent<Movement>();
			audioSource = GetComponent<AudioSource>();
		}
#endif
	}
}