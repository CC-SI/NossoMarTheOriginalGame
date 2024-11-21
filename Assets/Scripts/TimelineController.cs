using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
        
    public void Awake()
    {
        VideoController.OnVideoEnd += StartTimeline;
    }

    void StartTimeline()
    {
        playableDirector.Play();
    }

    private void OnDestroy()
    {
        VideoController.OnVideoEnd -= StartTimeline;
    }
}