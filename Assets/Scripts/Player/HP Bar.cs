using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image HPCurrentBar;
    [SerializeField] float HPFull;
    [SerializeField] float HPCurrent;
    [SerializeField] MovementPlayer1 player1;
    // Start is called before the first frame update
    void Start()
    {
        HPFull = player1.MaxHP;
        HPCurrent = player1.currentHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBar(float updatedHealth)
    {
        HPCurrent = updatedHealth;
        HPCurrentBar.fillAmount = HPCurrent / HPFull;
    }
}
