using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] List<GameObject> obstaclePrefabs;
    GameObject obstacleModel;

    void Awake()
    {
        StartCoroutine("DestroyChildren");
        obstacleModel = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        GameObject obstacleInstance = Instantiate(obstacleModel, this.transform);
        obstacleInstance.transform.localPosition = new Vector3(Random.Range(-1.6f,1.6f), 0f, 0f);
    }

    IEnumerator DestroyChildren()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        yield return null;
    }
}
