using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextScene;

    void Start()
    {
        var videoPath = GetVideoPath();
        
#if UNITY_EDITOR
        if (!File.Exists(videoPath))
        {
            SwitchScene();
            return;
        }
#endif
        
        videoPlayer.url = videoPath;
        
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += PlayVideo;
        
        videoPlayer.Prepare();
    }
    
    void PlayVideo(VideoPlayer video)
    {
        video.Play();
    }
    
    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        SwitchScene();
    }

    void SwitchScene()
    {
        switch (nextScene)
        {
            case "tutorial":
                GameManager.LoadTutorial();
                return;
            case "menu":
                GameManager.LoadMainMenu();
                break;
        }
    }

    string GetVideoPath()
    {
        
        return Path.Combine(Application.streamingAssetsPath, videoFileName);
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= EndReached;
        videoPlayer.prepareCompleted -= PlayVideo;
    }

#if UNITY_EDITOR
    void Reset()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
#endif
}