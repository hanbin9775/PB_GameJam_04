using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainableItem : MonoBehaviour
{
    public int item_id;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player 와 충동
        if (collision.gameObject.tag == "Player")
        {
            Inventory.GetInstance().Gain_Item(item_id);
            Destroy(gameObject);
        }
    }
}
