using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game parameter", menuName = "Data/Game Parameter")]
public class GameParameter : ScriptableObject
{
    public List<EnemyStats> enemyList;
    public GameObject boss;
    public float spawnSpeed;
    public GameObject playerPrefab;
    public BossProgressParam bossProgressParam;
}
