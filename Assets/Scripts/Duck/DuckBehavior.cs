using System;
using System.Collections;
using Interaction;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Duck
{
	public class DuckBehavior : InteractableObject, IInteraction
	{
		const float WanderTime = 10;
    
		public static event Action OnDuckRescued;
    
		Transform alvo;
		Transform endTarget;
   
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

		PlayerBehaviour Player => PlayerBehaviour.Instance;
    
		public bool IsRescued
		{
			get => IsFollowing;
			private set
			{
				IsFollowing = value;
				OnDuckRescued?.Invoke();
			}
		}

		public Transform EndTarget
		{
			get => endTarget;
			set
			{
				endTarget = value;
				endTarget.position = new Vector3(value.position.x, transform.position.y, 0);
				
				movement.SetFollowTarget(value);
			}
		}

		public void Quack()
		{
			if(movement.IsOnWater)
				return;
			
			audioSource.Play();
			OnQuacking.Invoke();
		}
    
		public void StartFollowing()
		{
			alvo = Player.GetFollowTarget(this);
			
			if (movement)
				movement.SetFollowTarget(alvo);

			IsRescued = true;
			
			RemoveObject(colisor);
		}
    
		public virtual void OnPlayerInteraction()
		{
			if (IsFollowing)
				return;
        
			StartFollowing();
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
		void Awake()
		{
			originalSpeed = movement.Speed;
		}
		
		protected virtual IEnumerator Start()
		{
			AddObject(colisor, this);
			Player.Movement.OnMoved.AddListener(OnPlayerMoved);
			
			if(!GameManager.IsLoadingGameData)
				yield break;
			
			yield return new WaitWhile(() => GameManager.IsLoadingGameData);

			if (!IsRescued)
				yield break;
			
			movement.WarpTo(Player.transform.position);
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