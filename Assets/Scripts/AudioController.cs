using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static AudioController Instance;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public static void PlaySong()
    {
        Instance.audioSource.Play();
    }
    
    public static void StopSong()
    {
        Instance.audioSource.Stop();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }
#endif
}