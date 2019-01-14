using UnityEngine;
using UnityEngine.Video;

[RequireComponent (typeof(VideoPlayer))]
public class VideoController : MonoBehaviour
{
    public int inFrame;
    public int outFrame;
    [HideInInspector] public bool frameReady;
    private VideoPlayer _videoPlayer;
    private int _currentFrame;
    private bool _isPaused;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.prepareCompleted += PlayerReady;
        _videoPlayer.frameReady += FrameReady;
        // BUG: Must set alpha to something other than 1.0 initially or it will not respond to script
        _videoPlayer.targetCameraAlpha = 0.5f;
    }

    private void FrameReady(VideoPlayer player, long frameIndex)
    {
        frameReady = true;
        _currentFrame = (int)frameIndex;
        // Check if the last frame of the range has loaded
        if (_currentFrame == outFrame)
        {
            Pause();
        }
    }

    private void PlayerReady(VideoPlayer player)
    {
        frameReady = true;
        _videoPlayer.playbackSpeed = 0f;
        _videoPlayer.Play();
        _currentFrame = 0;
    }

    private void Start()
    {
        _videoPlayer.Prepare();
        Pause();
    }

    public void Pause()
    {
        _videoPlayer.sendFrameReadyEvents = false;
        _isPaused = true;
        _videoPlayer.playbackSpeed = 0f;
    }

    public void Resume()
    {
        _videoPlayer.sendFrameReadyEvents = true;
        _isPaused = false;
        _videoPlayer.playbackSpeed = 1f;
        frameReady = false;
        // Start over if at last frame of range
        if (_currentFrame == outFrame)
        {
            GoToFrame(inFrame);
        }
    }

    public void StepForward()
    {
        GoToFrame(_currentFrame + 1);
    }

    public void StepBackward()
    {
        GoToFrame(_currentFrame - 1);
    }

    public void TogglePlay()
    {
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void GoToFrame(int frameIndex)
    {
        var nextFrame = Mathf.Clamp(frameIndex, inFrame, outFrame);
        if (nextFrame == _currentFrame) return;
        _videoPlayer.sendFrameReadyEvents = true;
        frameReady = false;
        _videoPlayer.frame = nextFrame;
        _currentFrame = nextFrame;
    }
    // Adjust the alpha channel by the supplied value
    public void AdjustAlpha(float value)
    {
        var newAlpha = Mathf.Clamp(value + _videoPlayer.targetCameraAlpha, 0, 1);
        _videoPlayer.targetCameraAlpha = newAlpha;
    }
}
