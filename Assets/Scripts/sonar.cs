using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonar : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * player.sonarSpeed);
        StartCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(this.transform.gameObject);
    }
}
