using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : BaseEnemy
{

    [SerializeField] private FieldOfView field_of_view;

    public enum FollowerState { idle, patrol, trace, attack, die };
    public FollowerState follower_state = FollowerState.patrol;

    public float attack_dist = 0.5f;
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

    private float agro_gage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        timer = wait_timer;
        empty = new GameObject();
        target = empty.transform;
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

            if (field_of_view.isPlayerDetected())
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
                    break;
            }
            yield return null;
        }
    }

    void Trace()
    {
        target = PlayerController.GetInstance().GetComponent<Transform>();
        transform.position = Vector2.MoveTowards(transform.position, target.position, (move_speed+1f) * Time.deltaTime);
    }

    void Patrol()
    {
        if(!set_patrol)
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
                GetComponent<Animator>().SetBool("serf_rest", true);
            }
            else
            {
                set_patrol = false;
                GetComponent<Animator>().SetBool("serf_rest", false);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            set_patrol = false;
        }
    }

}
