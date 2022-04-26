using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingssync : MonoBehaviour
{

    static public bool invertlook = false;
    static public bool hudshow = false;
    static public bool subtitles = false;
    static public bool fullscreen = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("invertLook") == 1)
        {
            invertlook = true;
        }

        if (PlayerPrefs.GetInt("hud") == 1)
        {
            hudshow = true;
        }

        if (PlayerPrefs.GetInt("subtitles") == 1)
        {
            subtitles = true;
        }

        if (PlayerPrefs.GetInt("fullscreen") == 1)
        {
            fullscreen = true;
        }
    }
}
