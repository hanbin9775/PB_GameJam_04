using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private static Inventory instance;

    public static Inventory GetInstance()
    {
        if (instance == null) instance = FindObjectOfType<Inventory>();
        return instance;
    }
    void Awake()
    {
        //Singleton Check
        if (instance != null)
        {
            if (instance != this) Destroy(gameObject);
        }
        item_cnt = new int[inventory_length];
        item_kind = new int[inventory_length];
        for (int i = 0; i < inventory_length; i++)
        {
            item_cnt[i] = 0;
            item_kind[i] = 0;
        }
    }
    #endregion

    public GameObject Player;

    private int inventory_length=4;

    private int[] item_cnt;
    private int[] item_kind;
    public Text[] cnt_text;

    private void Update_Cnt_Text()
    {
        for (int i = 0; i < inventory_length; i++)
        {
            cnt_text[i].text = item_cnt[i].ToString();
        }
    }

    private int EmptySlot()
    {
        int idx = -1;
        for (int i = 0; i < inventory_length; i++)
        {
            if (item_kind[i]==0)
            {
                idx = i;
                break;
            }
        }
        return idx;
    }

    public void Gain_Item(int item_id)
    {
        int idx=-1;

        //find item if it's in inventory
        for(int i=0; i<inventory_length; i++)
        {
            if(item_kind[i] == item_id)
            {
                idx = i;
                break;
            }
        }
        //item not found
        if (idx == -1)
        {
            idx = EmptySlot();
            if (idx == -1)
            {
                Debug.Log("Inventory is Full");
            }
            else
            {
                item_kind[idx] = item_id;
                item_cnt[idx]++;
            }
        }
        //item found
        else
        {
            item_cnt[idx]++;
        }
        Update_Cnt_Text();
    }

    public void Use_Item(int idx)
    {
        if (item_cnt[idx] > 0)
        {
            //1~10 : seed
            if (item_kind[idx] > 0 && item_kind[idx] <= 10)
            {

                item_cnt[idx]--;
                Instantiate(ItemDB.GetInstance().items[item_kind[idx] - 1],
                    new Vector2(Player.transform.position.x + 1f, Player.transform.position.y), Player.transform.rotation);
            }
            //11~20 : plant
            else if (item_kind[idx] > 10 && item_kind[idx] <= 20)
            {
                item_cnt[idx]--;
            }
        }
        Update_Cnt_Text();
    }

}
