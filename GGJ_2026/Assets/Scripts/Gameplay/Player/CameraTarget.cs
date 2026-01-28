using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private List<GameObject> Players;
    private int checkFrameCountdown = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CheckForPlayers(true);
    }

    void LateUpdate()
    {
        CheckForPlayers();

        Vector3 avgPos = Vector3.zero;

        foreach (GameObject obj in Players)
        {
            avgPos.x += obj.transform.position.x;
            avgPos.y += obj.transform.position.y;
        }

        avgPos.x /= Players.Count;
        avgPos.y /= Players.Count;
        avgPos.z = Players[0].transform.position.z;

        transform.position = avgPos;
    }

    private void CheckForPlayers(bool forceLoad = false)
    {
        checkFrameCountdown--;
        if (checkFrameCountdown == 0 || forceLoad) 
        {
            checkFrameCountdown = 15;

            if (Players == null)
            {
                Players = new List<GameObject>();
            } else
            {
                Players.Clear();
            }

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                Players.Add(obj);
            }
        } else
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(obj == null )
                {
                    Players.Remove(obj);
                }
            }
        }
    }
}
