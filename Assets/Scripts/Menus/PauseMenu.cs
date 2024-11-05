using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
	public class PauseMenu : MenuBehaviour
	{
		[SerializeField]
		Button continueButton;
		[SerializeField]
		Button settingsButton;
		[SerializeField]
		Button menuButton;
		[SerializeField]
		GameObject background;
		[SerializeField]
		GameObject buttons;

		bool IsHided
		{
			get => !buttons.activeInHierarchy;
			set => buttons.SetActive(!value);
		}

		protected override void Open()
		{
			base.Open();
			IsHided = false;
		}

		void Continue()
		{
			Invoke(nameof(Close), Delay);
		}
		
		void Settings()
		{
			Invoke(nameof(ShowSettingsOverlay), Delay);
		}
		
		void Menu()
		{
			Invoke(nameof(ShowMainMenu), Delay);
		}
		
		void ShowSettingsOverlay()
		{
			if (SettingsMenu)
				Open(SettingsMenu);

			IsHided = true;
		}

		void Awake()
		{
			PauseMenu = this;
			
			continueButton.onClick.AddListener(Continue);
			settingsButton.onClick.AddListener(Settings);
			menuButton.onClick.AddListener(Menu);
			GameManager.OnGamePaused.AddListener(background.SetActive);
		}

		void OnDestroy()
		{
			GameManager.OnGamePaused.RemoveListener(background.SetActive);
		}

#if UNITY_EDITOR
		void Reset()
		{
			background = transform.GetChild(0).gameObject;
			this.buttons = transform.GetChild(1).gameObject;
			var buttons = GetComponentsInChildren<Button>(true);
			
			continueButton = buttons[0];
			settingsButton = buttons[1];
			menuButton = buttons[2];
		}
		#endif
	}
}