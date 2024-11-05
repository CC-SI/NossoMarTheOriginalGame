using System;
using TMPro;
using UnityEngine;

namespace MiniGame
{
    public class GoalMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        
        private static MiniGame miniGame => MiniGame.Instance;
        
        private void OnEnable()
        {
            miniGame.OnMessageUpdated.AddListener(UpdateMessage);
        }

        private void OnDisable()
        {
            miniGame.OnMessageUpdated.RemoveListener(UpdateMessage);
        }
        
        private void UpdateMessage(string message)
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