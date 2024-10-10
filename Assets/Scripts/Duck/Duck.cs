using Interaction;
using Player;
using TMPro;
using UnityEngine;

public class Duck : InteractableObject, IInteraction
{
    [SerializeField] private TMP_Text countDucks;
    [SerializeField] private AudioClip clip;
    
    private Movement movement;
    private Collider2D colisor;
    
    private static int currentDuck = 0; 
    private AudioSource audioSource;
    
    private void Start()
    {
        colisor = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
    
        AddObject(colisor, this);
    }
    
    private void StartFollowing()
    {
        if (PlayerBehaviour.Instance)
        {
            Transform alvo = PlayerBehaviour.Instance.GetFollowTarget(this);
            movement.SetFollowTarget(alvo);
        }

        currentDuck++;
        countDucks.text = currentDuck.ToString();
        
        audioSource.Play(); 
    }

    void IInteraction.OnPlayerInteraction()
    {
        StartFollowing();
        RemoveObject(colisor);
    }
}
