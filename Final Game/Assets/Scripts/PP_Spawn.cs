using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Spawn : MonoBehaviour
{
    public GameObject _PP;
    private GameObject _Player;
    public GameObject _Arena;

    public float Timer = 10;

    public bool isInArena;

    public void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            SpawnPP();
            Timer = 10;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInArena = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInArena = false;
        }
    }


    private void SpawnPP()
    {
        Vector3 _SpawnPos = new Vector3(Random.Range(-15,15),-7,Random.Range(22,54));
        Instantiate(_PP,_SpawnPos, Quaternion.identity);
    }
}
