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

    private void Awake()
    {
        cur_stage = 1;
        StartCoroutine(Harvest_Timer());
        child_anims = GetComponentsInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack") cur_hp -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (cur_hp <= 0)
        {
            for (int i = 1; i < cur_stage; i++)
                Instantiate(drop_item, new Vector2(transform.position.x + Random.Range(-0.5f,0.5f), transform.position.y + Random.Range(-0.5f, 0.5f)), transform.rotation);
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

}
