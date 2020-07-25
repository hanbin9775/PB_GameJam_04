using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float generate_timer;
    public int hp;
    public int price;

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit!");
        hp -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            //해당 오브젝트 제거된 후 인벤토리에 추가
            Harvest();
        }
    }

    private void Harvest()
    {
        Inventory.GetInstance().counter[0]++;
        Destroy(gameObject);
    }
}
