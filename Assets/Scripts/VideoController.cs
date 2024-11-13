using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    [SerializeField] private string nextScene;
    [SerializeField] private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += PlayVideo;
    }

    void Start()
    {
        var videoPath = GetVideoPath();
        
        videoPlayer.url = videoPath;
        
        videoPlayer.Prepare();
        
        StartCoroutine(WaitForVideoPreparation());
    }

    private IEnumerator WaitForVideoPreparation()
    {
        float videoStartTime = Time.realtimeSinceStartup;
        bool isVideoReady = false;
        
        while (Time.realtimeSinceStartup - videoStartTime < 5f && !isVideoReady)
        {
            if (videoPlayer.isPrepared)
            {
                isVideoReady = true;
            }
            
            yield return null;
        }

        if (isVideoReady) yield break;
        
        SwitchScene();
    }
    void PlayVideo(VideoPlayer video)
    {
        video.Play();
    }
    
    void EndReached(VideoPlayer video)
    {
        video.Stop();
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
                AudioController.Instance.StopSong();
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