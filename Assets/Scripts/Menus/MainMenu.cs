using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
	public class MainMenu : MenuBehaviour
	{
		[Header("Componentes")]
		[SerializeField]
		Button continueButton;
		[SerializeField]
		Button newGameButton;
		[SerializeField]
		Button optionsButton;
		[SerializeField]
		Button exitButton;

		void Continue()
			=> ButtonInvoke(() => GameManager.LoadGame(true));

		void NewGame()
			=> ButtonInvoke(() => GameManager.LoadIntro());

		void Options()
		{
			ShowSettings();
		}
		
		void Exit()
			=> ButtonInvoke(GameManager.Exit);

		void Awake()
		{
			MainMenu = this;
			continueButton.onClick.AddListener(Continue);
			newGameButton.onClick.AddListener(NewGame);
			optionsButton.onClick.AddListener(Options);
			exitButton.onClick.AddListener(Exit);
		}

		void Start()
		{
			optionsButton.interactable = SettingsMenu;
			continueButton.gameObject.SetActive(GameManager.HasGameData);
		}

#if UNITY_EDITOR
		void Reset()
		{
			var buttons = GetComponentsInChildren<Button>(true);
			
			continueButton = buttons[0];
			newGameButton = buttons[1];
			optionsButton = buttons[2];
			exitButton = buttons[3];
		}
#endif
	}
}