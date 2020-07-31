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
        player_trans = transform;
    }
    #endregion

    public int player_hp;
    public int player_money;

    public float move_speed = 5f;
    private float cur_speed;

    public float cur_dir_x;
    public float cur_dir_y;

    public Rigidbody2D rb;

    Vector2 movement;
    private bool facing_right = true;


    public float attack_delay;
    private bool attackable = true;

    public Transform attackPoint;
    public Transform attackPointUp;
    public Transform attackPointDown;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public Transform player_trans;

    //F: weapon, Q,W,E,R :items
    private int inventory_selector;
    public GameObject[] inv_cursors;
    public int player_dmg;

    public int weapon_durability;
    public GameObject projectile;

    //disguise
    private bool hold_space;
    public float hold_time;
    private float hold_timer;
    private bool disguise;
    public bool stealth;
    public GameObject smoke_effect;

    public bool on_shop;
    public bool buy;
    public int id_to_buy;

    private void Start()
    {
        Inv_Select(4);
        on_shop = false;
        cur_dir_x = 0;
        cur_dir_y = 0;
        weapon_durability =0;
        cur_speed = move_speed;
        inventory_selector = 4;
        hold_timer = hold_time;
        disguise = false;
        stealth = false;
        foreach(GameObject obj in inv_cursors)
        {
            obj.SetActive(false);
        }
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

        if (movement.x > 0 && !facing_right && attackable)
            Flip();
        else if (movement.x < 0 && facing_right && attackable)
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
            else if(hold_timer<=0)
            {
                if (!disguise)
                {
                    Instantiate(smoke_effect, transform.position, transform.rotation);
                    disguise = true;
                    stealth = true;
                    GetComponent<Animator>().SetTrigger("disguise");
                    hold_timer = hold_time;
                }
                else
                {
                    Instantiate(smoke_effect, transform.position, transform.rotation);
                    disguise = false;
                    stealth = false;
                    GetComponent<Animator>().SetTrigger("undisguise");
                    hold_timer = hold_time;
                }
            }
        }

        //inventory slot select
        if (Input.GetKeyDown(KeyCode.Q)) Inv_Select(0);
        if (Input.GetKeyDown(KeyCode.W)) Inv_Select(1);
        if (Input.GetKeyDown(KeyCode.E)) Inv_Select(2);
        if (Input.GetKeyDown(KeyCode.R)) Inv_Select(3);
        if (Input.GetKeyDown(KeyCode.D)) Inv_Select(4);

        //interaction(space) input
        if (Input.GetKeyDown(KeyCode.Space) && !disguise)
        {
            if (attackable && inventory_selector == 4)
            {
                attackable = false;
                StartCoroutine(FirstAttackDelay());
                StartCoroutine(AttackDelay());
            }
            else if (inventory_selector < 4)
            {
                Inventory.GetInstance().Use_Item(inventory_selector);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && on_shop)
        {
            Inventory.GetInstance().Sell_Item(inventory_selector);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && buy)
        {
            Inventory.GetInstance().Buy_Item(id_to_buy);
        }
    }

    private void Inv_Select(int idx)
    {
        inventory_selector = idx;
        for(int i=0;i<inv_cursors.Length; i++)
        {
            if (idx == i) inv_cursors[i].SetActive(true);
            else inv_cursors[i].SetActive(false);
        }
    }

    //Physical Movement
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * cur_speed * Time.fixedDeltaTime);
    }

    IEnumerator AttackDelay()
    {
        cur_speed = 0;
        yield return new WaitForSeconds(attack_delay);
        cur_speed = move_speed;
        attackable = true;
    }

    IEnumerator FirstAttackDelay()
    {
        GetComponent<Animator>().SetTrigger("attack");
        yield return new WaitForSeconds(0.2f);
        Attack();
    }

    void Attack()
    {
        Collider2D[] hitEnemies = null;
        
        if(cur_dir_y==0) hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        else if(cur_dir_y==1) hitEnemies = Physics2D.OverlapCircleAll(attackPointUp.position, attackRange, enemyLayers);
        else if (cur_dir_y == -1) hitEnemies = Physics2D.OverlapCircleAll(attackPointDown.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.tag=="Plant")
                enemy.GetComponent<Plant>().Take_Damage(player_dmg);
            if (enemy.tag == "Enemy")
                enemy.GetComponent<Follower>().Take_Damage(player_dmg);
            if (enemy.tag == "Catcher")
                enemy.GetComponent<Catcher>().Take_Damage(player_dmg);

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawSphere(attackPoint.position, attackRange);
        Gizmos.DrawSphere(attackPointUp.position, attackRange);
        Gizmos.DrawSphere(attackPointDown.position, attackRange);
    }

    void Flip()
    {
        facing_right = !facing_right;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Speed_Up(float duration)
    {
        StartCoroutine(Buff_Speed(duration));
    }

    IEnumerator Buff_Speed(float duration)
    {
        cur_speed *= 1.5f;
        yield return new WaitForSeconds(duration);
        cur_speed = move_speed;
    }

    public void Go_Stealth(float duration)
    {
        StartCoroutine(Stealth(duration));
    }

    IEnumerator Stealth(float duration)
    {
        stealth = true;
        yield return new WaitForSeconds(duration);
        stealth = false;
    }

    public void Throw_Projectile()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
