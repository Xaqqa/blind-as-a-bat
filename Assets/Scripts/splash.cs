using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class splash : MonoBehaviour
{
    VideoPlayer vp;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        StartCoroutine("delaycheck");
    }

    IEnumerator delaycheck()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("waitforsplash");
    }
    IEnumerator waitforsplash()
    {
        if (!vp.isPlaying)
        {
            Debug.Log("move to mainmenu");
            SceneManager.LoadScene("MainMenu");

        }
        else
        {

        }
        yield return null;
        StartCoroutine("waitforsplash");
    }
}
