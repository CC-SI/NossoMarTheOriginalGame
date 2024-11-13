using UnityEngine;

namespace MiniGame
{
    public class SuperimposingMessage : MonoBehaviour
    {
        [SerializeField] private GameObject messageBox;

        public static SuperimposingMessage Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
            Hide();
        }

        public void Show()
        {
            messageBox.SetActive(true);
            Invoke(nameof(Hide), 1f);
        }
        
        public void Hide()
        {
            messageBox.SetActive(false);
        }
    }
}