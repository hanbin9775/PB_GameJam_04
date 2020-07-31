using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private static Inventory instance;

    PlayerController player;


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

    private int inventory_length=4;

    private int[] item_cnt;
    private int[] item_kind;
    public Text[] cnt_text;
    public Image[] item_img;
    public Sprite UI_mask;

    private void Start()
    {
        player = PlayerController.GetInstance();
        Gain_Item(11);
        Gain_Item(12);
    }

    private void Update_Cnt_Text()
    {
        for (int i = 0; i < inventory_length; i++)
        {
            cnt_text[i].text = item_cnt[i].ToString();
            if (item_cnt[i] == 0)
            {
                item_img[i].sprite = UI_mask;
                item_kind[i] = 0;
            }
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

    public void Buy_Item(int item_id)
    {
        int idx = -1;
        int price =0;
        
        switch (item_id)
        {
            case 11:
                price = 2;
                break;
            case 12:
                price = 4;
                break;
            case 13:
                price = 3;
                break;
            case 14:
                price = 4;
                break;
            case 15:
                price = 4;
                break;
            case 16:
                price = 2;
                break;
            case 17:
                price = 4;
                break;
            case 18:
                price = 8;
                break;
        }
        Debug.Log(price);
        if (price > player.player_money) return;
        
        //find item if it's in inventory
        for (int i = 0; i < inventory_length; i++)
        {
            if (item_kind[i] == item_id)
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
                player.player_money -= price;
                item_kind[idx] = item_id;
                item_cnt[idx]++;
                item_img[idx].sprite = ItemDB.GetInstance().item_sprites[item_id];
            }
        }
        //item found
        else
        {
            player.player_money -= price;
            item_cnt[idx]++;
        }
        Update_Cnt_Text();
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
                item_img[idx].sprite = ItemDB.GetInstance().item_sprites[item_id];
            }
        }
        //item found
        else
        {
            item_cnt[idx]++;
        }
        Update_Cnt_Text();
    }

    public void Sell_Item(int idx)
    {
        if (item_cnt[idx] > 0)
        {
            item_cnt[idx]--;
            //seed
            if (item_kind[idx] > 10 && item_kind[idx] <= 20)
            {
                player.player_money += 1;
                
            }
            //plant
            else if (item_kind[idx] > 0 && item_kind[idx] <= 10)
            {
                switch (item_kind[idx])
                {
                    case 1:
                        player.player_money += 8;
                        break;
                    case 2:
                        player.player_money += 16;
                        break;
                    case 3:
                        player.player_money += 12;
                        break;
                    case 4:
                        player.player_money += 40;
                        break;
                    case 5:
                        player.player_money += 16;
                        break;
                    case 6:
                        player.player_money += 8;
                        break;
                }
            }
            Update_Cnt_Text();
        }
    }

    //slot index에 해당하는 아이템 사 
    public void Use_Item(int idx)
    {
        if (item_cnt[idx] > 0)
        {
            //11~20 : seed
            if (item_kind[idx] > 10 && item_kind[idx] <= 20)
            {

                item_cnt[idx]--;
                //씨앗을 심으면 식물이 난다
                Instantiate(ItemDB.GetInstance().items[item_kind[idx] - 1 - 10],
                    new Vector2(player.transform.position.x + player.cur_dir_x,
                                player.transform.position.y + player.cur_dir_y),
                                player.transform.rotation);
            }
            //1~10 : plant
            else if (item_kind[idx] > 0 && item_kind[idx] <= 10)
            {
                switch (item_kind[idx])
                {
                    case 1:
                        Use_Rye();
                        item_cnt[idx]--;
                        break;
                    case 2:
                        Use_Cattails();
                        item_cnt[idx]--;
                        break;
                    case 3:
                        Use_Azaela();
                        item_cnt[idx]--;
                        break;
                    case 4:
                        break;
                    case 5:
                        Use_Pear();
                        item_cnt[idx]--;
                        break;
                    case 6:
                        Use_Tomato();
                        item_cnt[idx]--;
                        break;
                }

                
            }
        }
        Update_Cnt_Text();
    }


    //Plant Effect
    // 1
    private void Use_Rye()
    {
        player.player_hp += 2;
    }
    // 2
    private void Use_Cattails()
    {
        player.weapon_durability += 20;
    }
    // 3
    private void Use_Azaela()
    {
        player.Speed_Up(10f);
    }
   
    // 5
    private void Use_Pear()
    {
        player.Go_Stealth(10f);
    }
    //6
    private void Use_Tomato()
    {
        player.Throw_Projectile();
    }

}
