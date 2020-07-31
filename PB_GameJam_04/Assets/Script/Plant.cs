using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int item_id;

    public int cur_hp;

    public GameObject drop_item;

    private int cur_stage;
    public float stage2_flag;
    public float stage3_flag;

    Animator []child_anims;

    //foolsmarigold

    private void Awake()
    {
        cur_stage = 1;
        StartCoroutine(Harvest_Timer());
        child_anims = GetComponentsInChildren<Animator>();
    }

    public void Take_Damage(int dmg)
    {
        cur_hp -= dmg;
        Debug.Log("got hit!");
        if (cur_hp <= 0)
        {
            //fools'marigold
            if (item_id == 7)
            {
                Collider2D[] hitEnemies = null;
                hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
                foreach (Collider2D enemy in hitEnemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        if(cur_stage==2)
                            enemy.GetComponent<Follower>().Take_Damage(5);
                        if (cur_stage == 3)
                            enemy.GetComponent<Follower>().Take_Damage(10);
                    }
                }
            }
            for (int i = 1; i < cur_stage; i++)
                Instantiate(drop_item, new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f)), transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator Harvest_Timer()
    {
        yield return new WaitForSeconds(stage2_flag);
        for(int i=child_anims.Length-1; i>=0; i--)
        {
            child_anims[i].SetBool("stage2", true);
        }
        cur_stage = 2;
        yield return new WaitForSeconds(stage3_flag);
        for (int i = child_anims.Length - 1; i >= 0; i--)
        {
            child_anims[i].SetBool("stage3", true);
        }
        cur_stage = 3;
    }


    //Plant Effect

}
