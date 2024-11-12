using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string videoFileName;
    private VideoPlayer videoPlayer;
    [SerializeField] private string sceneTransition;
    [SerializeField] private TextMeshProUGUI textoUrl;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        var videoPath = GetVideoPath();
        
        textoUrl.text = videoPath;
        
        videoPlayer.url = videoPath;
        
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.prepareCompleted += PlayVideo;
        
        videoPlayer.Prepare();
    }
    
    void PlayVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Play();
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