using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Spawn : MonoBehaviour
{
    public GameObject _PP;
    public GameObject _Arena;


    public float Timer = 10;

    public List<GameObject> weaponList = new List<GameObject> ();
    //public GameObject RollPin;
    //public GameObject MeatHam;
    //public GameObject BSpoon;
    //public GameObject CookBook;

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            SpawnPP();
            Timer = 10;
        }
    }

    private void SpawnPP()
    {
        int weaponIndex = UnityEngine.Random.Range(0, weaponList.Count -1);
        Vector3 _SpawnPos = new Vector3(Random.Range(-15,15),8,Random.Range(22,54));
        Instantiate(weaponList[weaponIndex],_SpawnPos, Quaternion.identity);


    }
}
