using System.Collections.Generic;
using UnityEngine;

public class CombatRegistry : MonoBehaviour
{
    private static CombatRegistry _instance;
    public static CombatRegistry Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instance
                _instance = FindFirstObjectByType<CombatRegistry>();

                // If no instance was found, create one
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("CombatRegistry");
                    _instance = singleton.AddComponent<CombatRegistry>();
                }
            }
            return _instance;
        }
    }

    private readonly List<IDamageable> playerTeam = new();
    private readonly List<IDamageable> enemyTeam = new();

    public void RegisterPlayer(IDamageable d)
    {
        if (!playerTeam.Contains(d)) playerTeam.Add(d);
    }

    public void UnregisterPlayer(IDamageable d) => playerTeam.Remove(d);

    public void RegisterEnemy(IDamageable d)
    {
        if (!enemyTeam.Contains(d)) enemyTeam.Add(d);
    }

    public void UnregisterEnemy(IDamageable d) => enemyTeam.Remove(d);

    public IReadOnlyList<IDamageable> Players => playerTeam;
    public IReadOnlyList<IDamageable> Enemies => enemyTeam;
}
