using UnityEngine;

namespace MiniGame
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] private MiniGameObject hiddenObject;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private AudioSource duckSound;
        
        private bool isCollected;
        private bool isSuperimposed;
        
        private static MiniGame miniGame => MiniGame.Instance;
        
        public bool IsCollected
        {
            get => isCollected;
            private set
            {
                isCollected = value;
                miniGame.CheckObjective(this);
                enabled = !isCollected;
                sprite.enabled = !isCollected;
            }
        }

        public bool IsSuperimposed
        {
            get => isSuperimposed;
            private set
            {
                isSuperimposed = value;

                if (value)
                {
                    miniGame.AlertSuperimposing(hiddenObject.Bounds, hiddenObject.Index);
                }
            }
        }
        
        private void OnMouseDown()
        {
            IsSuperimposed = miniGame.IsObjectSuperimposed(hiddenObject.Bounds, hiddenObject.Index);
            
            if (IsSuperimposed) return;
            
            duckSound.Play();
            IsCollected = true;
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            hiddenObject = GetComponent<MiniGameObject>();
            duckSound = GetComponent<AudioSource>();
        }
#endif
    }
}