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

    public float dash_timer = 0.1f;
    public float dash_speed = 15f;

    public float attack_timer = 1f;
    public GameObject hit_box;


    private void Start()
    {
        cur_speed = move_speed;
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

        //dash input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
        //attack input
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Attack());
        }

    }

    //Physical Movement
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * cur_speed * Time.fixedDeltaTime);
    }


    IEnumerator Dash()
    {
        float timer = dash_timer;
        cur_speed = dash_speed;
        while (timer > 0)
        {
            timer -= 0.01f;
            yield return null;
        }
        cur_speed = move_speed;
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

}
