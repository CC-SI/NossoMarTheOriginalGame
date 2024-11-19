using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menus
{
	public class PauseMenu : MenuBehaviour
	{
		[Header("Configurações")]
		[SerializeField]
		InputActionReference _pauseAction;
		
		[Header("Componentes")]
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
		
		InputAction PauseAction => _pauseAction.action;

		protected override void Open()
		{
			base.Open();
			IsHided = false;
		}

		public void Pause()
		{
			GameManager.IsGamePaused = true;
		}

		void Continue()
			=> DelayedClose();

		void Settings()
			=> StartCoroutine(ButtonCoroutine(SettingsMenu, null, () => IsHided = true));

		void Menu()
			=> ShowMainMenu();

		void OnInputPerformed(InputAction.CallbackContext obj)
		{
			GameManager.IsGamePaused = !GameManager.IsGamePaused;
		}
		
		void OnGamePaused(bool isPaused)
		{
			if (isPaused)
			{
				Open();
				PauseAction.Disable();
			}
			else
			{
				Close();
				PauseAction.Enable();
			}

			background.SetActive(isPaused);
		}

		void Awake()
		{
			PauseMenu = this;
			
			continueButton.onClick.AddListener(Continue);
			settingsButton.onClick.AddListener(Settings);
			menuButton.onClick.AddListener(Menu);
			
			PauseAction.performed += OnInputPerformed;
		}

		void OnDestroy()
		{
			GameManager.OnGamePaused -= OnGamePaused;
			PauseAction.performed -= OnInputPerformed;
		}

		void OnDisable()
		{
			GameManager.IsGamePaused = false;
		}

		void Start()
		{
			GameManager.OnGamePaused += OnGamePaused;
			OnGamePaused(GameManager.IsGamePaused);
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