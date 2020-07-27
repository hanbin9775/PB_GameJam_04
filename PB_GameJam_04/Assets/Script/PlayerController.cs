using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    private static PlayerController instance;

    public static PlayerController GetInstance()
    {
        if (instance == null) instance = FindObjectOfType<PlayerController>();
        return instance;
    }
    void Awake()
    {
        //Singleton Check
        if (instance != null)
        {
            if (instance != this) Destroy(gameObject);
        }
    }
    #endregion


    public float move_speed = 5f;
    private float cur_speed;

    private float cur_dir_x;
    private float cur_dir_y;

    public Rigidbody2D rb;

    Vector2 movement;
    private bool facing_right = true;


    public float attack_delay;
    private bool attackable = true;
    public float attack_timer;
    public GameObject hit_box;

    //F: weapon, Q,W,E,R :items
    private int inventory_selector;

    //disguise
    private bool hold_space;
    public float hold_time;
    private float hold_timer;
    private bool disguise;

    private void Start()
    {
        cur_speed = move_speed;
        inventory_selector = 4;
        hold_timer = hold_time;
        disguise = false;
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
            GetComponent<Animator>().SetBool("is_moving", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("is_moving", false);
        }

        if(cur_dir_y == -1) GetComponent<Animator>().SetBool("look_front", true);
        else if(cur_dir_y==1) GetComponent<Animator>().SetBool("look_front", false);

        if (movement.x > 0 && !facing_right)
            Flip();
        else if (movement.x < 0 && facing_right)
            Flip();

        //Disguise
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hold_space = true;
            hold_timer = hold_time;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            hold_space = false;
            hold_timer = hold_time;
        }
        if (hold_space)
        {
            if (hold_timer > 0) hold_timer -= Time.deltaTime;
            else
            {
                if (!disguise)
                {
                    disguise = true;
                    GetComponent<Animator>().SetTrigger("disguise");
                    hold_timer = hold_time;
                }
                else
                {
                    disguise = false;
                    GetComponent<Animator>().SetTrigger("undisguise");
                    hold_timer = hold_time;
                }
            }
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
        GetComponent<Animator>().SetBool("player_attack", true);
        hit_box.GetComponent<Transform>().position = new Vector2( transform.position.x+cur_dir_x, transform.position.y + cur_dir_y);
        float timer = attack_timer;
        cur_speed = 0;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        hit_box.GetComponent<Transform>().position = new Vector2(transform.position.x, transform.position.y);
        GetComponent<Animator>().SetBool("player_attack", false);
        cur_speed = move_speed;
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

    void Flip()
    {
        facing_right = !facing_right;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
