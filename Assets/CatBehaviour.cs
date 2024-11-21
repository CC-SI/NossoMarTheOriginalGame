using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    
    [SerializeField] private AudioSource audio;
    
    void Start()
    {
        InvokeRepeating(nameof(Meow), Random.Range(2,5), Random.Range(2,5));
    }

    void Meow()
    {
        audio.Play();
    }
}
