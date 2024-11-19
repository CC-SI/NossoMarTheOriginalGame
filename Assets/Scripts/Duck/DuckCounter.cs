using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duck
{
	public class DuckCounter : MonoBehaviour
	{
		[Header("Configuração")]
		[SerializeField]
		bool showMax = true;
		[SerializeField]
		InputActionReference _quackAction;
		
		[Header("Componentes")]
		[SerializeField]
		Button button;
		[SerializeField]
		TMP_Text countText;
		[SerializeField]
		AudioSource audioSource;

		InputAction QuackAction => _quackAction;

		bool CanQuack
		{
			get => DuckManager.RescuedCount > 0;
			set
			{
				button.interactable = value;
				if(value)
					QuackAction.Enable();
				else
					QuackAction.Disable();
			}
		}
		
		void OnClick()
		{
			DuckManager.Quack();
		}
		
		void QuackPerformed(InputAction.CallbackContext context)
		{
			OnClick();
		}
		
		void UpdateCount()
		{
			var current = DuckManager.RescuedCount;
			CanQuack = current > 0;
			int max = DuckManager.TotalCount;
			StringBuilder value = new(current.ToString("D2"));
			
			if(showMax)
				value.AppendFormat(@"/{0:D2}", max);
			
			countText.text = value.ToString();
			
			if (GameManager.IsLoadingGameData || !audioSource.isActiveAndEnabled) return;
			
			audioSource.Play();
		}

		void UpdateCountDelayed()
		{
			Invoke(nameof(UpdateCount), 0);
		}

		void Awake()
		{
			button.onClick.AddListener(OnClick);
			DuckBehavior.OnDuckRescued += UpdateCountDelayed;

			QuackAction.performed += QuackPerformed;
		}

		void OnDestroy()
		{
			QuackAction.performed -= QuackPerformed;
			DuckBehavior.OnDuckRescued -= UpdateCountDelayed;
		}

		void Start()
		{
			audioSource.enabled = false;
			UpdateCount();
			audioSource.enabled = true;
		}

#if UNITY_EDITOR
		void Reset()
		{
			button = GetComponent<Button>();
			countText = GetComponentInChildren<TMP_Text>(true);
			audioSource = GetComponent<AudioSource>();
		}
#endif
	}
}