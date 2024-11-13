using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private string videoFileName;
    [SerializeField] private string nextScene;
    [SerializeField] private Button skipText;
    [SerializeField] private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += PlayVideo;
        skipText.onClick.AddListener(SwitchScene);
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
        Invoke(nameof(ShowSkipVideo), 5f);
    }
    
    void ShowSkipVideo()
    {
        skipText.gameObject.SetActive(true);
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