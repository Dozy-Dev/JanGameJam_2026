using System.Collections.Generic;
using UnityEngine;

public class StageAdvanceZone : MonoBehaviour
{
    [SerializeField] StagedCameraRig CamRig;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.CompareTag("Player"))
        {
            Vector3 lockedPos = new Vector3(transform.position.x, transform.position.y, -1);
            CamRig.LockStage(lockedPos);
        }
    }
}
