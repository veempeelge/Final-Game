using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatGen_Behave : MonoBehaviour
{
    private Inv_Item inv;
    private Item_Slot slot;
    public GameObject itemButton;

    private void Start()
    {
        StartCoroutine(Delete());
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            MovementPlayer1 mvP1 = other.gameObject.GetComponent<MovementPlayer1>();
            inv = mvP1.GetComponent<Inv_Item>();
            mvP1.StartWaterCoroutine();
          
            slot = inv.slot;

            for (int i = 0; i < inv.slots.Length; i++)
            {
                if (inv.isFull[i] == false)
                {
                    inv.hasWater = true;
                    inv.hasTrap = false;
                    inv.isFull[i] = true;
                    Instantiate(itemButton, inv.slots[i].transform, false);
                    slot.count = 3;
                    slot.RefreshCount();
                    break;
                }
                if (inv.isFull[i] == true)
                {
                    inv.DiscardItem(i);
                    inv.hasWater = true;
                    inv.hasTrap = false;
                    inv.isFull[i] = true;
                    Instantiate(itemButton, inv.slots[i].transform, false);
                    slot.count = 3;
                    slot.RefreshCount();
                    break;
                }
            }
        }
    }
}
