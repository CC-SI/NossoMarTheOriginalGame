using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sound
{
	public class VolumeController : MonoBehaviour
	{
		const float MinimumValue = .0001f;
		const float MaximumValue = 1f;

		const int Minimum = 0;
		const float Maximum = 100f;
		
		const float MinimumDB = -80f;
		const float MaximumDB = 0f;
		
		[Header("Configurações")]
		[SerializeField]
		AudioMixer _mixer;
		[SerializeField]
		string _volumeParameter;

		[Header("Componentes")]
		[SerializeField]
		Button raiseButton;
		[SerializeField]
		Button lowerButton;
		[SerializeField]
		TMP_Text valueText;

		int? value;

		int Value
		{
			get
			{
				if (value.HasValue)
					return value.Value;
				
				_mixer.GetFloat(_volumeParameter, out float volume);
				value = (int)Mathf.InverseLerp(MinimumDB, MaximumDB, volume) * 100;
				Debug.Log(volume);
				return value.Value;
			}
			
			set
			{
				this.value = Mathf.Clamp(value, Minimum, (int)Maximum);
				SetText(this.value.Value);
				float volume = this.value.Value / 100f;
				SetVolume(volume);
			}
		}
		
		void SetVolume(float volume)
		{
			volume = Mathf.Clamp(volume, MinimumValue, MaximumValue);
			_mixer.SetFloat(_volumeParameter, Mathf.Log10(volume) * 20);
		}

		void SetText(int value)
		{
			valueText.text = value.ToString();
		}
		
		void VolumeUp()
		{
			Value += 5;
		}
		
		void VolumeDown()
		{
			Value -= 5;
		}

		void Awake()
		{
			raiseButton.onClick.AddListener(VolumeUp);
			lowerButton.onClick.AddListener(VolumeDown);
		}

		void OnEnable()
		{
			SetText(Value);
		}

#if UNITY_EDITOR
		void Reset()
		{
			var buttons = GetComponentsInChildren<Button>(true);
			raiseButton = buttons[1];
			lowerButton = buttons[0];

			var texts = GetComponentsInChildren<TMP_Text>(true);
			valueText = texts[^1];
		}
#endif
	}
}