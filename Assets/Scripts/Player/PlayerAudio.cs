using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Movement))]
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private AudioClip swimming, walking;

        [SerializeField] private Movement movement;

        float audioTimer;
        float walkAudioInterval = 0.4f;
        float swimAudioInterval = 0.7f;

        private void Update()
        {
            if (audioTimer < 0)
                return;

            audioTimer -= Time.deltaTime;
        }

        void playWalkAudio(Vector2 direction, bool isMoving)
        {
            if (!isMoving || audioTimer > 0) return;
            
            audioTimer = walkAudioInterval;
            audioSource.PlayOneShot(walking);
        }

        void playSwimAudio(bool isOnWater)
        {
            if (!isOnWater || audioTimer > 0) return;
            
            audioTimer = swimAudioInterval;
            audioSource.PlayOneShot(swimming);
        }

        private void Awake()
        {
            movement.OnWaterEvent.AddListener(playSwimAudio);
            movement.OnMoved.AddListener(playWalkAudio);
        }

#if UNITY_EDITOR
        void Reset()
        {
            movement = GetComponent<Movement>();
            audioSource = GetComponent<AudioSource>();
        }
#endif
    }
}