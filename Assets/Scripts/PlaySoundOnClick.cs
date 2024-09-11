using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private AudioClip clip;
    private AudioSource source;
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        
        btn.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        source.clip = clip;
        source.Play();
    }
}
