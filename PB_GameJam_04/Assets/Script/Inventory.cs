using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private static Inventory instance;

    public static Inventory GetInstance()
    {
        if (instance == null) instance = FindObjectOfType<Inventory>();
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

    public int[] counter;
    public Text idx1;

    

    // Update is called once per frame
    void Update()
    {
        idx1.text = counter[0].ToString();
    }
}
