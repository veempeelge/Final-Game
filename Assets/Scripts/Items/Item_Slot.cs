using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item_Slot : MonoBehaviour
{
    public TMP_Text countText;
    public int count = 3;

    private Inv_Item inv;
    public int i;

    [SerializeField] MovementPlayer1 player1;

    // Start is called before the first frame update
    private void Start()
    {
        inv = player1.GetComponent<Inv_Item>();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inv.isFull[i] = false;
        }
    }
}
