using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Interaction
{
	public class InteractionUIButton : MonoBehaviour
	{
		[Header("Inputs")]
		[SerializeField]
		InputActionReference _interactAction;
		
		[Header("Componentes")]
		[SerializeField]
		Button button;
		
		InputAction InteractAction => _interactAction.action;

		public bool IsShowing
		{
			get => button.gameObject.activeSelf;
			set => button.gameObject.SetActive(value);
		}
        
		PlayerBehaviour Player => PlayerBehaviour.Instance;
		
		void Interact()
		{
			if(!Player
			   || !Player.InteractableZone)
				return;
			
			Player.InteractableZone.Interact();
		}
		
		void OnInteractActionPerformed(InputAction.CallbackContext context)
			=> Interact();

		void OnCanInteractChanged(bool canInteract)
		{
			IsShowing = canInteract;
		}

		void Awake()
		{
			IsShowing = IsShowing;
			
			if (InteractAction is not null)
				InteractAction.performed += OnInteractActionPerformed;
			
			button.onClick.AddListener(Interact);
		}

		void OnDestroy()
		{
			if (InteractAction is not null)
				InteractAction.performed -= OnInteractActionPerformed;

			if(!Player
			   || !Player.InteractableZone)
				return;
            
			Player.InteractableZone.OnCanInteractChanged.RemoveListener(OnCanInteractChanged);
		}
		
		void OnDisable()
		{
			InteractAction.Disable();
		}

		void OnEnable()
		{
			InteractAction.Enable();
		}

		IEnumerator Start()
		{
			yield return new WaitUntil(() => Player);

			bool isShowing = false;

			if (Player.InteractableZone)
			{
				Player.InteractableZone.OnCanInteractChanged.AddListener(OnCanInteractChanged);
				isShowing = Player.InteractableZone.CanInteract;
			}

			IsShowing = isShowing;
		}

		public void HideButton()
		{
			IsShowing = false;
		}
		
#if UNITY_EDITOR
		void Reset()
		{
			button = GetComponent<Button>();
		}
#endif
	}
}