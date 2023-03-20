using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;

    public int numberOfEnemiesToSpawn = 5;
    public float spawnDelay = 1f;
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++) 
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(enemyPrefabs[i], numberOfEnemiesToSpawn));
        }       
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        int spawnedEnemies = 0;

        while (spawnedEnemies < numberOfEnemiesToSpawn)
        {
            if (EnemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(spawnedEnemies);
            }
            else if (EnemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            spawnedEnemies++;

            yield return wait;
        }
    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int spawnIndex = spawnedEnemies % enemyPrefabs.Count;

        DoSpawnEnemy(spawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemyPrefabs.Count));
    }

    private void DoSpawnEnemy(int spawnIndex)
    {
        PoolableObject poolableObject = enemyObjectPools[spawnIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            int vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1)) // last param basically means what type of navmesh: walkable, jumpable, etc. -1 means all
            {
                enemy.agent.Warp(hit.position);
                // enemy must be enabled to function
                enemy.movement.target = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChasing();
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {triangulation.vertices[vertexIndex]}");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {spawnIndex} from object pool. Out of objects?");
        }
    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random
    }
}