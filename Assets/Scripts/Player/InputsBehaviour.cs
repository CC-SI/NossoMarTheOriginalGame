using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	public class InputsBehaviour : MonoBehaviour
	{
		[Header("Inputs")]
		[SerializeField]
		InputActionReference _moveAction;

		[Header("Componentes")]
		[SerializeField]
		FixedJoystick joystick;

		bool Interrupted
		{
			get => !joystick.isActiveAndEnabled;
			set
			{
				if(Interrupted == value)
					return;
				
				joystick.gameObject.SetActive(!value);
			}
		}

		InputAction MoveAction => _moveAction.action;
		
		PlayerBehaviour Player => PlayerBehaviour.Instance;
		
		void Move(Vector2 direction)
		{
			if(!Player)
			{
				return;
			}
			
			Player.Movement.Move(direction);
		}
		
		void OnMoveActionPerformed(InputAction.CallbackContext context)
		{
			if(!Player)
			{
				return;
			}
			
			var direction = context.ReadValue<Vector2>();
			Interrupted = direction != Vector2.zero;
			Move(direction);
		}

		void Awake()
		{
			if (MoveAction is not null)
			{
				MoveAction.performed += OnMoveActionPerformed;
				MoveAction.canceled += OnMoveActionPerformed;
			}
		}

		void FixedUpdate()
		{
			if(Interrupted)
				return;
			
			Player.Movement.Move(joystick.Direction);
		}

		void OnDestroy()
		{
			if (MoveAction is not null)
			{
				MoveAction.performed -= OnMoveActionPerformed;
				MoveAction.canceled -= OnMoveActionPerformed;
			}
		}

		void OnDisable()
		{
			MoveAction.Disable();
		}

		void OnEnable()
		{
			MoveAction.Enable();
			Interrupted = false;
		}
		
#if UNITY_EDITOR
		void Reset()
		{
			joystick = GetComponentInChildren<FixedJoystick>(true);
		}
#endif
	}
}