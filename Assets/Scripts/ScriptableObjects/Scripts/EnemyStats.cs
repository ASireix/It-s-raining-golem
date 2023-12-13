using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float maxHP;
    public float speed;
    public float attackRange;
    public float attackCooldown;

    public GameObject prefab;
    public int spawnWeigth;
}
