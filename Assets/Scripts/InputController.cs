using UnityEngine;

[RequireComponent (typeof (TimelineController), typeof(VideoController))]
public class InputController : MonoBehaviour
{
    private VideoController _videoController;

	private void Awake()
	{
	    _videoController = GetComponent<VideoController>();
	}

	private void Update ()
    {
	    // "Play/Pause"
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
            _videoController.TogglePlay();
	    }
	    // "Step Backward"
	    if (Input.GetKeyDown(KeyCode.LeftArrow) && _videoController.frameReady)
	    {
            _videoController.StepBackward();
        }
	    // "Step Forward"
	    if (Input.GetKeyDown(KeyCode.RightArrow) && _videoController.frameReady)
	    {
            _videoController.StepForward();
        }
	    // Controls for changing alpha of the projection plane
	    if (Input.GetKey(KeyCode.Equals))
	    {
	        _videoController.AdjustAlpha(0.01f);
	    }
	    if (Input.GetKey(KeyCode.Minus))
	    {
	        _videoController.AdjustAlpha(-0.01f);
	    }
	    // TODO: Create controls for zooming in and out (Figure out a real 2D magnify solution, not FOV-based)
	}
}
