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
		[SerializeField]
		RectTransform content;

		protected override bool IsOpen
		{
			get => content.gameObject.activeInHierarchy;
			set => content.gameObject.SetActive(value);
		}

		void Back()
		{
			switch (GameManager.CurrentState)
			{
				case GameState.Menu:
					ShowMainMenu();
					return;
				case GameState.Praia:
				case GameState.MiniGame:
				case GameState.Vila:
					ShowPauseMenu();
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
			content = transform.GetChild(0) as RectTransform;
			var buttons = GetComponentsInChildren<Button>(true);
			
			backButton = buttons[^1];
		}
#endif
	}
}