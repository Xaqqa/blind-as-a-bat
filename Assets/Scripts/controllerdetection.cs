using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class controllerdetection : MonoBehaviour
{
    [SerializeField] List<string> controllers;
    [SerializeField] GameObject toolbar;

    [SerializeField] RawImage login_Button;
    [SerializeField] Texture ps4_start;
    [SerializeField] Texture xbox_start;
    [SerializeField] Texture pc_start;

    [SerializeField] Texture ps4_A;
    [SerializeField] Texture ps4_B;
    [SerializeField] Texture ps4_Lstick;

    [SerializeField] Texture pc_A;
    [SerializeField] Texture pc_B;
    [SerializeField] Texture pc_Lstick;

    IEnumerator Start()
    {
        foreach (string controller in Input.GetJoystickNames())
        {
            controllers.Add(controller);
        }
        //controllers.RemoveAt(0);
        if (controllers.Count > 1)
        {
            Debug.Log("Multiple controllers connected... Defaulting to 'Keyboard & Mouse' Prompts");
            PlayerPrefs.SetString("buttonPrompts", "PC");
        }
        else if (controllers.Count == 0)
        {
            PlayerPrefs.SetString("buttonPrompts", "PC");
        }
        else if (controllers[0].Contains("Wireless Controller"))
        {
            PlayerPrefs.SetString("buttonPrompts", "Playstation");
        }
        else if (controllers.Count == 1)
        {
            PlayerPrefs.SetString("buttonPrompts", "Xbox");
        }
        PlayerPrefs.Save();
        StartCoroutine("buttonSprites");
        yield return null;
    }


    IEnumerator buttonSprites()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (PlayerPrefs.GetString("buttonPrompts") == "Playstation")
            {
                login_Button.texture = ps4_start;
            }
            else if (PlayerPrefs.GetString("buttonPrompts") == "Xbox")
            {
                login_Button.texture = xbox_start;
            }
            else if (PlayerPrefs.GetString("buttonPrompts") == "PC")
            {
                login_Button.texture = pc_start;
            }
        }
        


        foreach (Transform child in toolbar.transform)
        {
            if (child.GetComponent<RawImage>() != null)
            {
                if (child.name == "A_Button")
                {
                    if (PlayerPrefs.GetString("buttonPrompts") == "Playstation")
                    {
                        child.GetComponent<RawImage>().texture = ps4_A;
                    }
                    else if (PlayerPrefs.GetString("buttonPrompts") == "PC")
                    {
                        child.GetComponent<RawImage>().texture = pc_A;
                    }
                }
                else if (child.name == "B_Button")
                {
                    if (PlayerPrefs.GetString("buttonPrompts") == "Playstation")
                    {
                        child.GetComponent<RawImage>().texture = ps4_B;
                    }
                    else if (PlayerPrefs.GetString("buttonPrompts") == "PC")
                    {
                        child.GetComponent<RawImage>().texture = pc_B;
                    }
                }
                else if (child.name == "L_Stick")
                {
                    if (PlayerPrefs.GetString("buttonPrompts") == "Playstation")
                    {
                        child.GetComponent<RawImage>().texture = ps4_Lstick;
                    }
                    else if (PlayerPrefs.GetString("buttonPrompts") == "PC")
                    {
                        child.GetComponent<RawImage>().texture = pc_Lstick;
                    }
                }
                }
            }
            yield return null;
        }
    }
