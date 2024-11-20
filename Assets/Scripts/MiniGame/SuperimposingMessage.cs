using UnityEngine;

namespace MiniGame
{
	public class SuperimposingMessage : MonoBehaviour
	{
		static SuperimposingMessage instance;
        
		void Awake()
		{
			instance = this;
			Hide();
		}

		public static void Show() => instance.Show(true);

		public static void Hide() => instance.Show(false);

		// Usado pelo Animation Event.
		void AnimationHide() => Hide();
		
		void Show(bool show)
		{
			instance.gameObject.SetActive(show);
		}
		
		void OnDestroy()
		{
			instance = null;
		}
	}
}