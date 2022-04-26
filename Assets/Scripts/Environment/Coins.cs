using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] GameObject coinModel;

    void Awake()
    {
        GameObject coinInstance = Instantiate(coinModel, this.transform);
        this.transform.position += new Vector3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0f);
 
        /*/
        Collider[] colliders = Physics.OverlapSphere(coinInstance.transform.position, .2f);
        if (colliders.Length > 0)
        {
            Destroy(coinInstance);
        }
        /*/
    }

}
