using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string videoFileName = "meu_video.mp4";
    private VideoPlayer videoPlayer;
    [SerializeField] private String sceneTransition;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        var videoPath = GetVideoPath();
        
// #if UNITY_ANDROID
//         videoPath = "jar:file://" + videoPath;
// #elif UNITY_IOS
//         videoPath = "file://" + videoPath;
// #elif UNITY_STANDALONE_WIN
//         videoPath = "file://" + videoPath;
// #elif UNITY_STANDALONE_OSX
//         videoPath = "file://" + videoPath;
// #elif UNITY_WEBGL
//         videoPath = "https://example.com/meu_video.mp4";
// #endif
        
        if (!File.Exists(videoPath)) return;
        
        videoPlayer.url = videoPath;
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
    }
    
    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        
        switch (sceneTransition)
        {
            case "creditos":
                GameManager.LoadCredits();
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
}