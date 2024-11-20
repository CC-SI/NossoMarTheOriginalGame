using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialog
{
	public class TextBox : MonoBehaviour
	{
		[Header("Configuração")]
		[SerializeField]
		[Min(0)]
		float typingSpeed = 0.05f;
		[SerializeField]
		InputActionReference _nextAction;
		[SerializeField]
		InputActionReference _closeAction;

		[Header("Animações")]
		[SerializeField]
		string _openParameter;
		
		[Header("Componentes")]
		[SerializeField]
		GameObject speakerBox;
		[SerializeField]
		TMP_Text speakerText;
		[SerializeField]
		TMP_Text dialogText;
		[SerializeField]
		RectTransform content;
		[SerializeField]
		Button advanceButton;
		[SerializeField]
		Button closeButton;
		[SerializeField]
		Animator animator;

		static TextBox instance;
		
		bool isTyping;
		WaitForSeconds instruction;

		Vector2 originalPosition;

		public static event Action OnTextEnded;

		public bool IsOpen
		{
			get => animator.GetBool(_openParameter);
			private set => animator.SetBool(_openParameter, value);
		}
		
		InputAction NextAction => _nextAction.action;
		InputAction CloseAction => _closeAction.action;

		public void Open(IPage page)
		{
			IsOpen = true;
			if(isTyping)
				return;
			
			speakerText.text = page.Speaker;
			dialogText.text = page.Text;
			
			StartCoroutine(TypingCoroutine());
		}
		
		public static void Show(IPage page)
			=> instance.Open(page);

		// Usado pelo Animation Event.
		void AnimationClose()
		{
			OnTextEnded?.Invoke();
		}
		
		void Close()
		{
			IsOpen = false;
		}

		void StartTyping()
		{
			isTyping = true;
		}

		void TypingSpeedUp()
		{
			// Se tiver terminado de digitar, fecha o dialogo.
			if (!isTyping)
			{
				Close();
				return;
			}
			
			if (instruction is null)
			{
				TypingCancel();
				return;
			}
			
			instruction = null;
		}
		
		void TypingCancel()
		{
			StopCoroutine(TypingCoroutine());
			dialogText.maxVisibleCharacters = dialogText.text.Length;
			isTyping = false;
		}

		IEnumerator TypingCoroutine()
		{
			dialogText.maxVisibleCharacters = 0;
			yield return new WaitUntil(() => isTyping);
			
			instruction = new WaitForSeconds(typingSpeed);
			
			while (dialogText.maxVisibleCharacters < dialogText.text.Length)
			{
				dialogText.maxVisibleCharacters++;
				yield return instruction;
			}
			
			isTyping = false;
		}
		
		void NextActionPerformed(InputAction.CallbackContext context)
		{
			if(!advanceButton.isActiveAndEnabled)
				return;
			
			TypingSpeedUp();
		}
		
		void CloseActionPerformed(InputAction.CallbackContext context)
		{
			if(!closeButton.isActiveAndEnabled)
				return;
			
			Close();
		}

		void Awake()
		{
			if (instance)
			{
				Destroy(gameObject);
				return;
			}
			
			instance = this;
			advanceButton.onClick.AddListener(TypingSpeedUp);
			closeButton.onClick.AddListener(Close);
			NextAction.performed += NextActionPerformed;
			CloseAction.performed += CloseActionPerformed;
		}

		void OnDestroy()
		{
			NextAction.performed -= NextActionPerformed;
			CloseAction.performed -= CloseActionPerformed;
		}

		void OnDisable()
		{
			NextAction.Disable();
			CloseAction.Disable();
		}

		void OnEnable()
		{
			NextAction.Enable();
			CloseAction.Enable();
		}

#if UNITY_EDITOR
		void Reset()
		{
			content = transform.GetChild(0) as RectTransform;
			
			var images = GetComponentsInChildren<RectTransform>(true);
			
			for(int i = 0; i < images.Length; i++)
			{
				if(images[i].name == "Orador")
				{
					speakerBox = images[i].gameObject;
					break;
				}
			}

			var texts = GetComponentsInChildren<TMP_Text>(true);
			speakerText = texts[0];
			dialogText = texts[1];

			var buttons = GetComponentsInChildren<Button>(true);
			advanceButton = buttons[0];
			closeButton = buttons[1];
			
			animator = GetComponent<Animator>();
		}
#endif
	}
}