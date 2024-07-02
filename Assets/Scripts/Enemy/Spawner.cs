using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float timeBetweenSpawns;
    float nextSpawnTime;

    public GameObject[] spawnableEnemies;
    public int minenemiesPerSpawn = 1;
    public int maxenemiesPerSpawn = 4;

    public Transform[] spawnPoints;

    public Transform enemiesParent;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            int enemiesToSpawn = Random.Range(minenemiesPerSpawn, maxenemiesPerSpawn + 1);

             for (int i = 0; i < enemiesToSpawn; i++)
             {
                SpawnEnemiesAt(randomSpawnPoint);
                //GameObject spawnedEnemy = Instantiate(enemy, randomSpawnPoint.position, Quaternion.identity);
                //spawnedEnemy.transform.parent = enemiesParent;
             }
              
            //Debug.Log($"{enemiesToSpawn} enemies spawned at: " + randomSpawnPoint.position);
       
            //Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Instantiate(enemy, randomSpawnPoint.position, Quaternion.identity);
        }
    }

    void SpawnEnemiesAt(Transform spawnPoint)
    {
        if (spawnableEnemies.Length > 0)
        {
            GameObject enemiesToSpawn = spawnableEnemies[Random.Range(0, spawnableEnemies.Length)];
            GameObject spawnedEnemies = Instantiate(enemiesToSpawn, spawnPoint.position, Quaternion.identity);
            spawnedEnemies.transform.parent = enemiesParent;
        }
        else
        {
            Debug.LogWarning("No Objects To Spawn. Please Assign Objects To The Inspector.");
        }
    }

    public void StartSpawning()
    {
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }
}