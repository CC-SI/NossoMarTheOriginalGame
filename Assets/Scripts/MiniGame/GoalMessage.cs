using TMPro;
using UnityEngine;

namespace MiniGame
{
    public class GoalMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        
        private static MiniGame miniGame => MiniGame.Instance;
        
        void Start()
        {
            miniGame.onMessageUpdated.AddListener(UpdateMessage);
        }

        void OnDisable()
        {
            miniGame.onMessageUpdated.RemoveListener(UpdateMessage);
        }
        
        void UpdateMessage(string message)
        {
            messageText.text = message;
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            messageText = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}