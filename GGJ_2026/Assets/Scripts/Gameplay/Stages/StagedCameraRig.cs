using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using static UnityEditor.PlayerSettings;

public class StagedCameraRig : MonoBehaviour
{
    [SerializeField] private bool IsStageLocked = false;
    [SerializeField] private Transform CameraTarget;
    [SerializeField] private CameraTarget _target;
    private Vector3 lockedPos;

    private void Update()
    {
        if (IsStageLocked)
        {
            CameraTarget.position = lockedPos;
        }
    }

    public void LockStage(Vector3 pos)
    {
        lockedPos = pos;
        IsStageLocked = true;
        _target.enabled = false;
    }

    public void UnlockStage()
    {
        IsStageLocked = false;
        _target.enabled = true;
    }
}
