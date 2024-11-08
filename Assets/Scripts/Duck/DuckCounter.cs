using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duck
{
	public class DuckCounter : MonoBehaviour
	{
		[Header("Configuração")]
		[SerializeField]
		bool showMax = true;
		[Header("Componentes")]
		[SerializeField]
		Button button;
		[SerializeField]
		TMP_Text countText;
		[SerializeField]
		AudioSource audioSource;

		void OnClick()
		{
			DuckBehavior.Quack();
		}
		
		void UpdateCount(int current)
		{
			button.interactable = current > 0;
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
		}
#endif
	}
}