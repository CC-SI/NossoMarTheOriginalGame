using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
	public class SettingsMenu : MenuBehaviour
	{
		[SerializeField]
		VolumeController[] volumes;
		[SerializeField]
		Button backButton;

		void Back()
		{
			switch (GameManager.CurrentState)
			{
				case GameState.Menu:
					Invoke(nameof(ShowMainMenu), Delay);
					return;
				case GameState.Playing:
					Invoke(nameof(ShowPauseMenu), Delay);
					return;
				default:
					Debug.Log("Provavelmente falta configurar algo.", this);
					break;
			}
		}
		
		void Awake()
		{
			SettingsMenu = this;
			Close();
			
			backButton.onClick.AddListener(Back);
		}

		void OnDisable()
		{
			foreach (VolumeController volume in volumes)
				volume.Save();
		}

		void OnEnable()
		{
			foreach (VolumeController volume in volumes)
				volume.Load();
		}

#if UNITY_EDITOR
		void Reset()
		{
			volumes = GetComponentsInChildren<VolumeController>(true);
			var buttons = GetComponentsInChildren<Button>(true);
			
			backButton = buttons[^1];
		}
#endif
	}
}