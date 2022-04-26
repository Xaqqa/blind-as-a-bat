using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionDelete : MonoBehaviour
{


    [SerializeField] Transform player;
    void Update()
    {
        this.transform.position = player.position;
    }
}
