using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Axis co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraAxis : CinemachineExtension
{
    enum AxisToLock
    {
        X,
        Y,
        Z
    };

    [SerializeField]
    private AxisToLock _lockedAxis;

    [SerializeField, Tooltip("Lock the camera's Axis position to this value")]
    private float _lockedAxisPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            switch (_lockedAxis)
            {
                case AxisToLock.X:
                    pos.x = _lockedAxisPosition;
                    break;
                case AxisToLock.Y:
                    pos.y = _lockedAxisPosition;
                    break;
                case AxisToLock.Z:
                    pos.z = _lockedAxisPosition;
                    break;
            }
            state.RawPosition = pos;
        }
    }
}