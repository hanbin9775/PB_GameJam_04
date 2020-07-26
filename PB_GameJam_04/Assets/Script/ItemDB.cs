using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    #region Singleton
    private static ItemDB instance;

    public static ItemDB GetInstance()
    {
        if (instance == null) instance = FindObjectOfType<ItemDB>();
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
    public GameObject[] items;
}
