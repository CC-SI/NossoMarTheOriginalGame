using System;
using System.Collections;
using UnityEngine;

namespace Menus
{
	public abstract class MenuBehaviour : MonoBehaviour
	{
		protected const float DelayTime = .3f;

		protected static SettingsMenu SettingsMenu;
		protected static MainMenu MainMenu;
		protected static PauseMenu PauseMenu;
		
		static readonly WaitForSecondsRealtime Delay = new(DelayTime);
		
		protected virtual bool IsOpen
		{
			get => gameObject.activeInHierarchy;
			set => gameObject.SetActive(value);
		}
		
		protected static IEnumerator ButtonCoroutine(MenuBehaviour target, MenuBehaviour current, Action callback = null)
		{
			yield return Delay;
			Open(target);
			
			if (current)
				current.Close();
			
			callback?.Invoke();
		}
		
		static IEnumerator MainMenuCoroutine(MenuBehaviour current)
		{
			yield return ButtonCoroutine(MainMenu, current, Load);

			void Load()
			{
				if (!MainMenu)
					GameManager.LoadMainMenu();
			}
		}

		static IEnumerator DelayedActionCoroutine(Action callback)
		{
			yield return Delay;
			callback.Invoke();
		}
		
		public void ShowSettings()
			=> StartCoroutine(ButtonCoroutine(SettingsMenu, this));

		public void ShowMainMenu()
			=> StartCoroutine(MainMenuCoroutine(this));

		public void ShowPauseMenu()
			=> StartCoroutine(ButtonCoroutine(PauseMenu, this));
		
		protected void ButtonInvoke(Action callback)
			=> StartCoroutine(DelayedActionCoroutine(callback));

		protected void Close()
			=> IsOpen = false;

		protected void DelayedClose()
			=> StartCoroutine(ButtonCoroutine(null, this));

		protected virtual void Open()
			=> IsOpen = true;

		protected static void Open(MenuBehaviour menu)
		{
			if(!menu)
				return;
			
			menu.Open();
		}
	}
}