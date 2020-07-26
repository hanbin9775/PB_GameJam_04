using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float move_speed = 5f;
    private float cur_speed;

    private float cur_dir_x;
    private float cur_dir_y;

    public Rigidbody2D rb;

    Vector2 movement;

    public float attack_delay = 1f;
    private bool attackable = true;
    public float attack_timer = 1f;
    public GameObject hit_box;

    //F: weapon, Q,W,E,R :items
    private int inventory_selector;


    private void Start()
    {
        cur_speed = move_speed;
        inventory_selector = 4;
        hit_box.SetActive(false);
    }

    //Input
    private void Update()
    {
        //simple movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x!=0 || movement.y != 0)
        {
            cur_dir_x = movement.x;
            cur_dir_y = movement.y;
        }

        //inventory slot select
        if (Input.GetKeyDown(KeyCode.Q)) inventory_selector = 0;
        if (Input.GetKeyDown(KeyCode.W)) inventory_selector = 1;
        if (Input.GetKeyDown(KeyCode.E)) inventory_selector = 2;
        if (Input.GetKeyDown(KeyCode.R)) inventory_selector = 3;
        if (Input.GetKeyDown(KeyCode.F)) inventory_selector = 4;

        //interaction(space) input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attackable && inventory_selector==4)
            {
                StartCoroutine(Attack());
                StartCoroutine(AttackDelay());
            }
            else if(inventory_selector<4)
            {
                Inventory.GetInstance().Use_Item(inventory_selector);
            }
        }

    }

    //Physical Movement
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * cur_speed * Time.fixedDeltaTime);
    }

    IEnumerator Attack()
    {
        hit_box.SetActive(true);
        hit_box.GetComponent<Transform>().position = new Vector2( transform.position.x+cur_dir_x, transform.position.y + cur_dir_y);
        float timer = attack_timer;
        while (timer > 0)
        {
            timer -= 0.01f;
            yield return null;
        }
        hit_box.GetComponent<Transform>().position = new Vector2(transform.position.x, transform.position.y);
        hit_box.SetActive(false);
    }

    IEnumerator AttackDelay()
    {
        attackable = false;
        float timer = attack_delay;
        while (timer > 0)
        {
            timer -= 0.01f;
            yield return null;
        }
        attackable = true;
    }

}
