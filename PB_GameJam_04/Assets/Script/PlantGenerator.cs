using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{ 
    public GameObject[] plants;
    public float generate_time = 4f;
    private float timer;


    float generate_pos_x;
    float generate_pos_y;
    int generate_idx;


    private void Start()
    {
        generate_pos_x = Random.Range(-5f, 5f);
        generate_pos_y = Random.Range(-4f, 4f);
        generate_idx = Random.Range(0, plants.Length - 1);
        timer = generate_time;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0) timer -= 0.01f;
        else
        {
            Instantiate(plants[generate_idx],new Vector2(generate_pos_x, generate_pos_y), transform.rotation);
            generate_pos_x = Random.Range(-5f, 5f);
            generate_pos_y = Random.Range(-4f, 4f);
            generate_idx = Random.Range(0, plants.Length - 1);
            timer = generate_time;
        }
    }


}       
