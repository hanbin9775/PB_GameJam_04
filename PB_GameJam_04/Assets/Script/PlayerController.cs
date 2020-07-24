using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float move_speed = 5f;
    private float cur_speed;

    public Rigidbody2D rb;

    Vector2 movement;

    public float dash_timer = 0.1f;
    public float dash_speed = 15f;

    private void Start()
    {
        cur_speed = move_speed;
    }

    //Input
    private void Update()
    {
        //simple movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        //dash input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
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

}
