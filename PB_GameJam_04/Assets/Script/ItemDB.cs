using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /*
     * Item Id
     * 
     * [Plant]
     * 1 : Ray
     * 2 : Cattails
     * 3 : Azalea
     * 4 : Marigold
     * 5 : Pear
     * 6 : Tomato
     * 7 : FoolsMarigold
     * 8 : WolfDrake
     * 
     * [Seed] : [Plant] + 10;
     * 11 : 
    */

    public GameObject[] items;
    public Sprite[] item_sprites;
    
}
