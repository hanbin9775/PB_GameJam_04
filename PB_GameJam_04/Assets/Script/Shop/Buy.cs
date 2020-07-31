using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy : MonoBehaviour
{
    private int item_id;
    public GameObject icon;


    private void Start()
    {
        item_id = Random.Range(11, 18);
        Debug.Log(item_id);
        icon.GetComponent<SpriteRenderer>().sprite = ItemDB.GetInstance().item_sprites[item_id];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.GetInstance().buy = true;
            PlayerController.GetInstance().id_to_buy = item_id;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.GetInstance().buy = false;
        }
    }
}
