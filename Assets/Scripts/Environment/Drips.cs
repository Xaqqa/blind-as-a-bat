using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drips : MonoBehaviour
{
    [SerializeField] GameObject drip;

    IEnumerator Start()
    {
        Vector3 temp_position = new Vector3(Random.Range(-1.6f, 1.6f), 4f, transform.position.z);
        GameObject temp_drip = Instantiate(drip, temp_position, transform.rotation);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        Destroy(temp_drip);
        StartCoroutine("Start");
    }
}
