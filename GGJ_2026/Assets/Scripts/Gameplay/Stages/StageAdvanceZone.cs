using System;
using System.Collections.Generic;
using UnityEngine;

public struct EnemySpawnObject
{
    public GameObject Prefab;
    public int count;
    public Transform spawnLocation;
}

public class StageAdvanceZone : MonoBehaviour
{
    [SerializeField] StagedCameraRig CamRig;
    [SerializeField] private List<EnemySpawnObject> spawns;
    [SerializeField] private bool HasSpawned;

    [SerializeField] private int ToKill;

    private void Awake()
    {
        GameEventSystem.Instance.RegisterListener(GameEvent.EnemyDied, Enemydied);
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

            if( !HasSpawned )
            {
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        foreach(EnemySpawnObject obj in spawns)
        {
            for (int i = 0; i < obj.count; i++)
            {
                GameObject _obj = Instantiate(obj.Prefab, obj.spawnLocation.position, Quaternion.identity);
                ToKill++;
            }
        }
        HasSpawned = true;
    }

    private void Enemydied(object obj)
    {
        ToKill--;
        if( ToKill == 0)
        {
            GameObject.FindFirstObjectByType<StagedCameraRig>().UnlockStage();
            Destroy(gameObject);
        }
    }

}
