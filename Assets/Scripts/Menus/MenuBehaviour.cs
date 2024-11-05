using UnityEngine;

namespace Menus
{
	public abstract class MenuBehaviour : MonoBehaviour
	{
		protected const float Delay = .25f;

		protected static SettingsMenu SettingsMenu;
		protected static MainMenu MainMenu;
		protected static PauseMenu PauseMenu;
		
		bool IsOpen
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive(value);
		}
		
		public void ShowSettings()
		{
			if (SettingsMenu)
				SettingsMenu.Open();
			
			Close();
		}
		
		public void ShowMainMenu()
		{
			if(MainMenu)
				MainMenu.Open();
			else
				GameManager.LoadMainMenu();
			
			Close();
		}
		
		public void ShowPauseMenu()
		{
			if(PauseMenu)
				PauseMenu.Open();
			
			Close();
		}

		protected void Close()
		{
			IsOpen = false;
		}

		protected virtual void Open()
		{
			IsOpen = true;
		}

		protected static void Open(MenuBehaviour menu)
		{
			menu.Open();
		}
	}
}