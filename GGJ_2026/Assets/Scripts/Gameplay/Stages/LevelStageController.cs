using UnityEngine;
using System.Collections.Generic;
using ProgressGraph;
using System;

[Serializable]
public struct Stage
{
    public StageEntranceController EntranceController;
    public string StageName;
}

public class LevelStageController : MonoBehaviour
{
    [SerializeField] private List<Stage> Stages;

    private void Awake()
    {
        for(int i = 0; i < Stages.Count; i++)
        {
            Progress.DefineFlag(Stages[i].StageName);
            if( i > 0 )
            {
                Progress.Require(Stages[i].StageName, Stages[i-1].StageName);
                Stages[i].EntranceController.stageName = Stages[i].StageName;
            }
        }
        ProgressSnapshot shot = Progress.Export();
        Debug.Log("");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
