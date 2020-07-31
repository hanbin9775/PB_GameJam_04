using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy_Self());
    }

    IEnumerator Destroy_Self()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
