using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int item_id;
    public float generate_timer;
    public int hp;
    public int price;
    public GameObject item_obj;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack") hp -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            //해당 오브젝트 제거된 후 인벤토리에 추가
            Instantiate(item_obj, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
