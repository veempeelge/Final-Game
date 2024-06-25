using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGen_SPawn : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform[] spawnPoints;
    public float Timer = 15f;

    // Start is called before the first frame update
    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            SpawnWaterGen();
            Timer = 15;
        }
    }

    private void SpawnWaterGen()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}