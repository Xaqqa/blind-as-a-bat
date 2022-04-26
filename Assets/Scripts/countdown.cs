using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class countdown : MonoBehaviour
{
    TextMeshProUGUI tmp;
    PlayableDirector director;

    int currentNum = 3;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        director = GetComponent<PlayableDirector>();
    }


    private void Update()
    {
        if (director.state == PlayState.Paused && tmp.text != "GO!")
        {
            if (currentNum != 1)
            {
                currentNum--;
                tmp.text = currentNum.ToString();
                director.Play();
            }
            else
            {
                tmp.text = "GO!";
                director.Play();
                player.countdownComplete = true;
            }
        }
    }
}
