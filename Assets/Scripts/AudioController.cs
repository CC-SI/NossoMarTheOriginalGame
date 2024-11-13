using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public static AudioController Instance { get; private set; }

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
    
    public void PlaySong()
    {
        audioSource.Play();
    }
    
    public void StopSong()
    {
        audioSource.Stop();
    }

#if UNITY_EDITOR
    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }
#endif
}