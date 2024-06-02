using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Behave : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            MovementPlayer1 mvP1 = other.GetComponent<MovementPlayer1>();

            if (mvP1 != null)
            {
                if (gameObject.tag == "RollPin")
                {
                    mvP1.ChangeStats(3, 2, 5, 2, 6, 5);
                }
                else if (gameObject.tag == "MeatHam")
                {
                    mvP1.ChangeStats(5, 1, 6, 2, 10, 3);
                }
                else if (gameObject.tag == "BSpoon")
                {
                    mvP1.ChangeStats(2, 3, 5, 3, 7, 9);
                }
                else if (gameObject.tag == "Book")
                {
                    mvP1.ChangeStats(4, 4, 5, 5, 5, 9);
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("No MovementPlayer1 component found on the player object.");
            }
        }
    }
}
