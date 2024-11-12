using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string videoFileName = "meu_video.mp4";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (!File.Exists(GetVideoPath())) return;
        
        videoPlayer.url = GetVideoPath();
        videoPlayer.Play();
    }

    string GetVideoPath()
    {
        var videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        return videoPath;
    }
}