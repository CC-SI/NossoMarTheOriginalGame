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
			get => DuckBehavior.Rescued > 0;
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
			DuckBehavior.Quack();
		}
		
		void QuackPerformed(InputAction.CallbackContext context)
		{
			OnClick();
		}
		
		void UpdateCount(int current)
		{
			CanQuack = current > 0;
			int max = DuckBehavior.TotalCount;
			StringBuilder value = new(current.ToString("D2"));
			
			if(showMax)
				value.AppendFormat(@"/{0:D2}", max);
			
			countText.text = value.ToString();
			audioSource.Play();
		}

		void Awake()
		{
			button.onClick.AddListener(OnClick);
			DuckBehavior.OnDuckRescued += UpdateCount;

			QuackAction.performed += QuackPerformed;
		}

		void Start()
		{
			audioSource.enabled = false;
			UpdateCount(DuckBehavior.Rescued);
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