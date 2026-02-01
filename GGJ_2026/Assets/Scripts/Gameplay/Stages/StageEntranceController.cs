using UnityEngine;
using ProgressGraph;

public class StageEntranceController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    public string stageName;
    [SerializeField] private GameObject bars;

    private void Awake()
    {
        collider.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Progress.GetUnmetRequirements(stageName).Count == 0)
        {
            collider.enabled = true;
            bars.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
