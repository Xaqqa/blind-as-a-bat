using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarCheck : MonoBehaviour
{
    Collider[] obstacleColliders;
    List<GameObject> pillars = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("findPillars");
    }

    IEnumerator findPillars()
    {
        yield return new WaitForSeconds(0.05f);
        obstacleColliders = Physics.OverlapSphere(transform.position, 5f); //Scans a chunk of 3 obstacles

        foreach (Collider i in obstacleColliders)
        {
            if (i.gameObject.CompareTag("PillarObstacle")) //Finds any pillars
            {
                Debug.Log("Found Pillar");
                pillars.Add(i.gameObject); //Adds each pillar to a list called "pillars"
            }
        }

        for (int i = 0; i < pillars.Count - 1; i++)
        {
            Destroy(pillars[0].gameObject); //destroy a pillar
            Debug.Log("Removed Pillar");
        }

        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(transform.position, 5f);
    }
}
