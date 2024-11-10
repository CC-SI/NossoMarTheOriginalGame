using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using Player;
using Serialization;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Duck
{
	public class DuckBehavior : InteractableObject, IInteraction, ISerializable
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
		
		[field: Header("Eventos")]
		[field: SerializeField]
		public UnityEvent OnQuacking { get; private set; }
    
		protected bool IsFollowing;
		float originalSpeed;
		private ISerializable serializableImplementation;

		PlayerBehaviour Player => PlayerBehaviour.Instance;
    
		public static int RescuedCount { get; private set; }
		public static int TotalCount => Ducks.Count;

		public bool IsRescued
		{
			get => IsFollowing;
			private set
			{
				if(value)
					RescuedCount++;
				else if (IsRescued)
					RescuedCount--;
				
				IsFollowing = value;
				OnDuckRescued?.Invoke(RescuedCount);
			}
		}
    
		public int Index { get; private set; }

		public static void Quack()
		{
			foreach (DuckBehavior duck in Ducks)
			{
				if(!duck.IsRescued
				   || duck.movement.IsOnWater)
					continue;
            
				float variation = Random.value;

				duck.audioSource.pitch = Mathf.Lerp(1, 1.1f, variation);;
				duck.audioSource.volume = Mathf.Lerp(.9f, 1, variation);
				duck.Invoke(nameof(QuackSound), variation);
			}
		}

		void QuackSound()
		{
			if(movement.IsOnWater)
				return;
			
			audioSource.Play();
			OnQuacking.Invoke();
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
			GameManager.SaveGameData();
		}
    
		void OnPlayerMoved(Vector2 direction, bool isMoving)
		{
			if (!IsRescued)
				return;
			
			CancelInvoke(nameof(Wander));
			movement.Speed = originalSpeed;

			movement.SetFollowTarget(isMoving ? alvo : Player.transform);
			
			if(isMoving)
				return;
			
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

		public void Save(SaveData data)
		{
			if (!IsRescued)
				return;
			
			data.ducks.Add(Index);
		}
        
		public void Load(SaveData data)
		{
			if(!data.ducks.Contains(Index))
				return;
			
			RemoveObject(colisor);
			StartFollowing();
		}
		
		void Awake()
		{
			Index = TotalCount;
#if UNITY_EDITOR
			name = $"Pato {Index}";
#endif
			originalSpeed = movement.Speed;
			Ducks.Add(this);
		}

		void OnDestroy()
		{
			Ducks.Remove(this);
			GameManager.Unsubscribe(this);
			
			if(Ducks.Count > 0)
				return;
			
			RescuedCount = 0;
		}
		
		IEnumerator Start()
		{
			GameManager.Subscribe(this);
			AddObject(colisor, this);
			Player.Movement.OnMoved.AddListener(OnPlayerMoved);
			
			if(!GameManager.IsLoadingGameData)
				yield break;
			
			yield return new WaitWhile(() => GameManager.IsLoadingGameData);

			if (!IsRescued)
				yield break;
			
			transform.position = Player.transform.position;
			Wander();
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