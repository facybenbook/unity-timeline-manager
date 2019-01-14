using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Video;

[RequireComponent (typeof (PlayableDirector))]
public class TimelineController : MonoBehaviour
{
    private PlayableDirector _director;
    private TimelineAsset _timeline;
    private VideoPlayer _videoPlayer;    // Subscribed to its frameReady event
    private int _currentFrame;
    private float _frameTimer;
    private bool _isPlaying;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _timeline = _director.playableAsset as TimelineAsset;
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.frameReady += FrameReady;
    }

    private void FrameReady(VideoPlayer player, long frameIndex)
    {
        GoToFrame((int)frameIndex);
    }

    private void Start()
    {
        _isPlaying = true;
        _director.Evaluate();
        Pause();
    }

    private void Pause()
    {
        if (!_isPlaying) return;

        _director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        _isPlaying = false;
    }

    private void Resume()
    {
        if (_isPlaying) return;

        if (_director.duration - _director.time < 0.1f)
        {
            _director.time = 0.0f;
        }

        _director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        _isPlaying = true;
    }

    public void TogglePlay()
    {
        var speed = _director.playableGraph.GetRootPlayable(0).GetSpeed();

        if (speed < 0.01f)
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
        _currentFrame = frameIndex;
        _director.time = _currentFrame / _timeline.editorSettings.fps;
        _director.Evaluate();
    }

    public void StepForward()
    {
        GoToFrame(_currentFrame + 1);
    }

    public void StepBackward()
    {
        GoToFrame(_currentFrame - 1);
    }

    // Manually update Timeline
    private void ThrottledUpdate()
    {
        if (!_timeline || !_isPlaying) return;
        _frameTimer += Time.deltaTime;

        if (_frameTimer >= 1 / _timeline.editorSettings.fps)
        {
            StepForward();
            // Reset frame timer
            _frameTimer = 0f;
        }
    }
}
