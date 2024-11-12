using System;
using Dialog.Manager;
using Duck;
using UnityEngine;
using UnityEngine.Events;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private DialogManager endDialogText;
    [SerializeField] private GameObject endQuestText;
    [SerializeField] private Collider2D endArea;

    void Awake()
    {
        DuckBehavior.OnDuckRescued += EndGame;
    }

    void EndGame()
    {
        if (DuckManager.RescuedCount < 1) return;
            
        endQuestText.gameObject.SetActive(true);
        endArea.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        endArea.enabled = false;
        endQuestText.SetActive(false);
        endDialogText.StartDialog();
    }
        
    void OnDestroy()
    {
        DuckBehavior.OnDuckRescued -= EndGame;
    }
}