using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// holds BASE STATS for enemy that can be modified at object creation time
// and can reset their stats if died or modified during runtime

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // enemy stats
    public int health = 1;
    public float attackDelay = 1f;
    public int damage = 1;
    public float attackRadius = 1.5f;
    public bool isRanged = false;

    // NavMeshAgent configs
    public float AIUpdateInterval = 0.1f;

    public float acceleration = 8;
    public float angularSpeed = 120;
    // -1 means everything (walkable, jump, etc)
    public int areaMask = -1;
    public int avoidancePriority = 50;
    public float baseOffset = 0;
    public float height = 2f;
    public ObstacleAvoidanceType obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float radius = 0.5f;
    public float speed = 3f;
    public float stoppingDistance = 0.5f;

}
