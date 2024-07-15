using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGen_SPawn : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform[] spawnPoints;
    public float Timer = 15f;

    // Start is called before the first frame update

    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        }
    }
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
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        }
    }
}
