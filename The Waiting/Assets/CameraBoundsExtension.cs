using Cinemachine;
using UnityEngine;

public class CameraBoundsExtension : CinemachineExtension
{
    public Transform background; // Assign your background image in the Inspector

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // Calculate the bounds of the background
        Vector3 minBounds = background.GetComponent<SpriteRenderer>().bounds.min;
        Vector3 maxBounds = background.GetComponent<SpriteRenderer>().bounds.max;

        // Clamp the camera's position
        state.RawPosition = new Vector3(
            Mathf.Clamp(state.RawPosition.x, minBounds.x + state.Lens.OrthographicSize * state.Lens.Dutch, maxBounds.x - state.Lens.OrthographicSize * state.Lens.Dutch),
            Mathf.Clamp(state.RawPosition.y, minBounds.y + state.Lens.OrthographicSize, maxBounds.y - state.Lens.OrthographicSize),
            state.RawPosition.z
        );
    }
}