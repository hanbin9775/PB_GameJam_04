using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : BaseEnemy
{

    [SerializeField] private FieldOfView field_of_view;

    public enum FollowerState { idle, patrol, trace, attack, die };
    public FollowerState follower_state = FollowerState.patrol;

    public float attack_dist;
    public float move_speed = 2f;

    private bool is_dead = false;

    private Transform target;
    GameObject empty;

    public float minX = -5f;
    public float minY = -4f;
    public float maxX = 5f;
    public float maxY = 4f;
    public float wait_timer = 1.0f;
    private float timer;
    private bool set_patrol = false;

    private bool is_angry;
    private float agro_gage = 1f;

    private PlayerController player;
    private Transform player_tr;
    private Transform monster_tr;
    private Animator animator;

    public float attack_time;
    private float attack_timer;

    public GameObject dmg_pop;
    public GameObject die_effect;
    public GameObject hit_effect;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.GetInstance();
        is_angry = false;
        attack_timer = attack_time;
        timer = wait_timer;
        empty = new GameObject();
        target = empty.transform;
        player_tr = PlayerController.GetInstance().player_trans;
        monster_tr = this.gameObject.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckFollowerState());
        StartCoroutine(FollowerAction());   
    }

    // Update is called once per frame
    void Update()
    {
        field_of_view.SetOrigin(transform.position);
    }

    IEnumerator CheckFollowerState()
    {
        while (!is_dead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector2.Distance(player_tr.position, monster_tr.position);

            if(hp <= 0)
            {
                follower_state = FollowerState.die;
            }
            else if (dist<=attack_dist && is_angry)
            { 
                follower_state = FollowerState.attack;
            }
            else if (field_of_view.isPlayerDetected() && is_angry && !player.stealth)
            {
                if (agro_gage > 0)
                {
                    agro_gage -= 0.2f;
                }
                else follower_state = FollowerState.trace;
            }
            else
            {
                agro_gage = 1f;
                is_angry = false;
                follower_state = FollowerState.patrol;
            }
        }
    }

    IEnumerator FollowerAction()
    {
        while (!is_dead)
        {
            switch (follower_state)
            {
                case FollowerState.patrol:
                    Patrol();
                    break;
                case FollowerState.trace:
                    set_patrol = false;
                    Trace();
                    break;
                case FollowerState.attack:
                    Attack();
                    break;
                case FollowerState.die:
                    Dead();
                    break;
            }
            yield return null;
        }
    }

    void Attack()
    {
        
        if (attack_timer > 0) attack_timer -= Time.deltaTime;
        else
        {
            animator.SetTrigger("attack");
            PlayerController.GetInstance().player_hp -= 1;
            attack_timer = attack_time;
        }
    }

    void Trace()
    {
        attack_timer = attack_time;
        animator.SetBool("serf_rest", false);
        target = PlayerController.GetInstance().player_trans;
        transform.position = Vector2.MoveTowards(transform.position, target.position, (move_speed+1f) * Time.deltaTime);
    }

    void Dead()
    {
        Instantiate(die_effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void Patrol()
    {

        if (!set_patrol)
        {
            empty = new GameObject();
            target = empty.transform;
            timer = wait_timer;
            target.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            set_patrol = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.position, move_speed * Time.deltaTime);
        if (Vector2.Distance(target.position, transform.position)< 0.2f)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                animator.SetBool("serf_rest", true);
            }
            else
            {
                set_patrol = false;
                animator.SetBool("serf_rest", false);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("obstacle");
            set_patrol = false;
            follower_state = FollowerState.patrol;
        }
    }

    public void Take_Damage(int dmg)
    {
        Instantiate(dmg_pop, new Vector2(transform.position.x + Random.Range(0, 0.5f),
                                         transform.position.y + Random.Range(0, 0.5f)),
                                         transform.rotation);
        Instantiate(hit_effect, new Vector2(transform.position.x + Random.Range(-0.2f, 0.2f),
                                         transform.position.y + Random.Range(-0.2f, 0.2f)),
                                         transform.rotation);
        follower_state = FollowerState.trace;
        StartCoroutine(Get_Angry());
        hp -= dmg;
    }

    IEnumerator Get_Angry()
    {
        move_speed = 0f;
        yield return new WaitForSeconds(0.5f);
        is_angry = true;
        agro_gage = 0f;
        move_speed = 2f;
        
    }
}
