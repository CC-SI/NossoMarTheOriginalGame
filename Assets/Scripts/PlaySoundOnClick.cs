using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }  
        else
        {
            Debug.LogWarning("Nenhum AudioSource foi encontrado no GameObject.");
        } 
    }
}
