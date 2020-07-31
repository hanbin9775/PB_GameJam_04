using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.GetInstance();

        rb.velocity = new Vector2(player.cur_dir_x,
                                  player.cur_dir_y)
                                   * speed;
    }

    private void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime*1000);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Follower>().Take_Damage(5);
            
        }
        if(collision.tag!="Player") Destroy(gameObject);
    }
}
