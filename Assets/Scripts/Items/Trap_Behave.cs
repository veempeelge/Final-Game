using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Behave : MonoBehaviour
{
    private Inv_Item inv;
    public GameObject itemButton;

    private void OnCollisionEnter(Collision other)
    {
        MovementPlayer1 mvP1 = other.gameObject.GetComponent<MovementPlayer1>();

        if (gameObject.tag == "Trap")
        {
            Debug.Log("Player Trapped!");
        }
        else
        {
            inv = mvP1.GetComponent<Inv_Item>();

            for (int i = 0; i < inv.slots.Length; i++)
            {
                if (inv.isFull[i] == false)
                {
                    inv.hasTrap = true;
                    inv.hasWater = false;
                    inv.isFull[i] = true;
                    Instantiate(itemButton, inv.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
                if (inv.isFull[i] == true)
                {
                    inv.DiscardItem(i);

                    inv.hasTrap = true;
                    inv.hasWater = false;
                    inv.isFull[i] = true;
                    Instantiate(itemButton, inv.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }

    }

}
