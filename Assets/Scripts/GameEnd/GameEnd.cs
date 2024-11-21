using Dialog.Manager;
using Duck;
using UnityEngine;

namespace GameEnd
{
    public class GameEnd : MonoBehaviour
    {
        [SerializeField] private DialogManager endDialogText;
        [SerializeField] private GameObject endQuestText;
        [SerializeField] private Collider2D initialPoint;
        [SerializeField] private AudioSource praiaSong;
        [SerializeField] private Collider2D endPoint;

        void Awake()
        {
            DuckBehavior.OnDuckRescued += EndGame;
        }

        void EndGame()
        {
            if (DuckManager.RescuedCount < DuckManager.TotalCount) return;
            
            endQuestText.gameObject.SetActive(true);
            initialPoint.enabled = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
        
            praiaSong.Stop();
            initialPoint.enabled = false;
            endPoint.enabled = true;
            endQuestText.SetActive(false);
            endDialogText.StartDialog();
            AudioController.PlaySong();
        }
        
        void OnDestroy()
        {
            DuckBehavior.OnDuckRescued -= EndGame;
        }
    }
}
