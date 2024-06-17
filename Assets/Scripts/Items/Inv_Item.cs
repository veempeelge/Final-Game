using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inv_Item : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    public bool hasTrap;

    public int playerNumber;
    private string fireButton;
    public GameObject TrapPrefab;
    public float spawnDistance;

    private MovementPlayer1 mvP1;
    private Transform PTransform;

    private void Start()
    {
        mvP1 = GetComponent<MovementPlayer1>();
        PTransform = GetComponent<Transform>();
        fireButton = "Fire" + playerNumber;
    }

    private void Update()
    {
        if (Input.GetButtonDown(fireButton))
        {
            UseItem();
        }
    }

    public void DiscardItem(int slotIndex)
    {
        if (isFull[slotIndex])
        {
            foreach (Transform child in slots[slotIndex].transform)
            {
                hasTrap = false;
                Destroy(child.gameObject);
            }

            isFull[slotIndex] = false;
        }
    }

    private void UseItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (hasTrap)
            {
                Vector3 spawnPosition = PTransform.position + (PTransform.forward * spawnDistance);
                Debug.Log("Spawn Position: " + spawnPosition);
                // Instantiate the prefab at the calculated position and rotation
                Instantiate(TrapPrefab, spawnPosition, PTransform.rotation);

                Debug.Log("Player " + playerNumber + " used an item!");
                DiscardItem(i);
            }
            else
            {
                Debug.Log("Player " + playerNumber + " does not have item");
            }
        }
    }

}
