using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] GameObject sectionPrefab;
    //GameObject temp_section;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Building Next Section...");
            Instantiate(sectionPrefab, transform.parent.transform.position + new Vector3(0f, 0f, 60f), transform.rotation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SectionDelete"))
        {

            Debug.Log("Removing Section...");
            Destroy(transform.parent.gameObject);
        }
    }
}
