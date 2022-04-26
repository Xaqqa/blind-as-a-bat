using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class store : MonoBehaviour
{
    bool axisNav = true;
    bool promptOpen = false;

    [SerializeField] PlayableDirector transition;
    [SerializeField] PlayableAsset transitionOutAsset;

    [SerializeField] TextMeshProUGUI purchasePromptItemText;
    [SerializeField] PlayableDirector purchasePrompt;
    [SerializeField] List<GameObject> menuOptions;

    [SerializeField] TextMeshProUGUI currentlyEquipped;
    [SerializeField] TextMeshProUGUI A_Text;
    [SerializeField] int selectedIndex;

    [SerializeField] PlayableDirector purchased;
    [SerializeField] PlayableDirector equipped;
    [SerializeField] GameObject colorCost;
    [SerializeField] GameObject notEnoughCoins;
    [SerializeField] GameObject purchasedText;

    [SerializeField] TextMeshProUGUI coinCount;
    [SerializeField] List<VideoClip> colorClips;
    [SerializeField] VideoPlayer colorClipPlayer;



    // Start is called before the first frame update
    void Start()
    {
        currentlyEquipped.text = "Currently Equipped: " + PlayerPrefs.GetString("SonarColor");
        PlayerPrefs.SetInt("White", 1); //Purchased (Default)
        coinCount.text = PlayerPrefs.GetInt("Coins")+ " Coins";
        StartCoroutine("colorMenuOption");
        A_Text.text = "Equip";
    }

    // Update is called once per frame
    void Update()
    {
        #region Debug
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            int temp_coins = PlayerPrefs.GetInt("Coins") + 1000;
            PlayerPrefs.SetInt("Coins", temp_coins);
            PlayerPrefs.Save();
            coinCount.text = PlayerPrefs.GetInt("Coins") + " Coins";
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            int temp_coins = PlayerPrefs.GetInt("Coins") - 100;
            PlayerPrefs.SetInt("Coins", temp_coins);
            PlayerPrefs.Save();
            coinCount.text = PlayerPrefs.GetInt("Coins") + " Coins";
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.SetInt("White", 1); //Purchased (Default)
            PlayerPrefs.SetInt("Yellow", 0); //Unpurchased
            PlayerPrefs.SetInt("Orange", 0); //Unpurchased
            PlayerPrefs.SetInt("Red", 0); //Unpurchased
            PlayerPrefs.SetInt("Green", 0); //Unpurchased
            PlayerPrefs.SetInt("Blue", 0); //Unpurchased
            PlayerPrefs.SetInt("Purple", 0); //Unpurchased
            PlayerPrefs.SetInt("Pink", 0); //Unpurchased

            PlayerPrefs.Save();
            //SceneManager.LoadScene("Store");
        }

        #endregion

        if (PlayerPrefs.GetString("buttonPrompts") == "PC")
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                selectUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                selectDown();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                selectConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectBack();
            }
        }
        else if (PlayerPrefs.GetString("buttonPrompts") == "Playstation")
        {

            if (Input.GetAxisRaw("PS4DPADY") < -.45f || Input.GetAxisRaw("Vertical") < -.45f)
            {
                if (axisNav)
                {
                    selectUp();
                    axisNav = false;
                    StartCoroutine("AxisDelay");
                }
            }
            else if (Input.GetAxisRaw("PS4DPADY") > 45f || Input.GetAxisRaw("Vertical") > 45f)
            {
                if (axisNav)
                {
                    selectDown();
                    axisNav = false;
                    StartCoroutine("AxisDelay");
                }
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                selectConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                selectBack();
            }
        }
        else if (PlayerPrefs.GetString("buttonPrompts") == "Xbox")
        {

            if (Input.GetAxisRaw("XBOXDPADY") < -.45f || Input.GetAxisRaw("Vertical") < -.45f)
            {
                if (axisNav)
                {
                    selectUp();
                    axisNav = false;
                    StartCoroutine("AxisDelay");
                }

            }
            else if (Input.GetAxisRaw("XBOXDPADY") > .45f || Input.GetAxisRaw("Vertical") > .45f)
            {
                if (axisNav)
                {
                    selectDown();
                    axisNav = false;
                    StartCoroutine("AxisDelay");
                }
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                selectConfirm();
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                selectBack();
            }
        }
    }

    void selectUp()
    {
        if (!promptOpen)
        {
            if (selectedIndex == 0)
            {
                selectedIndex = menuOptions.Count - 1;
            }
            else
            {
                selectedIndex--;
            }
            StartCoroutine("colorMenuOption");

            if (menuOptions[selectedIndex].name != "Exit")
            {
                StartCoroutine("FindColorClip");
                if (PlayerPrefs.GetInt(menuOptions[selectedIndex].name) == 1) //1 = True
                {
                    A_Text.color = new Color(1f, 1f, 1f, 1f);
                    A_Text.text = "Equip";
                    purchasedText.SetActive(true);
                    colorCost.SetActive(false);
                    notEnoughCoins.SetActive(false);
                }
                else
                {
                    if (PlayerPrefs.GetInt("Coins") >= 100)
                    {
                        A_Text.color = new Color(1f, 1f, 1f, 1f);
                        A_Text.text = "Purchase";
                        colorCost.SetActive(true);
                        notEnoughCoins.SetActive(false);
                        purchasedText.SetActive(false);
                    }
                    else
                    {
                        A_Text.color = new Color(1f, 1f, 1f, 0.5f);
                        A_Text.text = "Purchase";
                        colorCost.SetActive(true);
                        notEnoughCoins.SetActive(true);
                        purchasedText.SetActive(false);
                    }
                }
            }
            else
            {
                A_Text.color = new Color(1f, 1f, 1f, 1f);
                A_Text.text = "Main Menu";
            }
        }
    }

    void selectDown()
    {
        if (!promptOpen)
        {
            if (selectedIndex == menuOptions.Count - 1)
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex++;
            }
            StartCoroutine("colorMenuOption");

            if (menuOptions[selectedIndex].name != "Exit")
            {
                StartCoroutine("FindColorClip");
                if (PlayerPrefs.GetInt(menuOptions[selectedIndex].name) == 1) //1 = True
                {
                    A_Text.color = new Color(1f, 1f, 1f, 1f);
                    A_Text.text = "Equip";
                    purchasedText.SetActive(true);
                    colorCost.SetActive(false);
                    notEnoughCoins.SetActive(false);
                }
                else
                {
                    if (PlayerPrefs.GetInt("Coins") >= 100)
                    {
                        A_Text.color = new Color(1f, 1f, 1f, 1f);
                        A_Text.text = "Purchase";
                        colorCost.SetActive(true);
                        notEnoughCoins.SetActive(false);
                        purchasedText.SetActive(false);
                    }
                    else
                    {
                        A_Text.color = new Color(1f, 1f, 1f, 0.5f);
                        A_Text.text = "Purchase";
                        colorCost.SetActive(true);
                        notEnoughCoins.SetActive(true);
                        purchasedText.SetActive(false);
                    }
                }
            }
            else
            {
                A_Text.text = "Main Menu";
            }
        }
    }

    void selectConfirm()
    {
        if (!promptOpen)
        {
            if (menuOptions[selectedIndex].name == "Exit")
            {
                transition.playableAsset = transitionOutAsset;
                transition.Play();
                StartCoroutine("ReturnToMenu");
            }
            else
            {
                if (A_Text.text == "Purchase" && A_Text.color.a == 1.0f)
                {
                    Debug.Log("Are you sure you want to purchase " + menuOptions[selectedIndex].name + " for 100 Coins?");
                    promptOpen = true;
                    purchasePromptItemText.text = menuOptions[selectedIndex].name;
                    purchasePrompt.Play();
                }
                else if (A_Text.text == "Purchase" && A_Text.color.a == 0.5f)
                {
                    Debug.Log("Not enough coins.");
                }
                else if (A_Text.text == "Equip")
                {
                    PlayerPrefs.SetString("SonarColor", menuOptions[selectedIndex].name);
                    Debug.Log("Equipped " + menuOptions[selectedIndex].name+".");
                    currentlyEquipped.text = "Currently Equipped: " + PlayerPrefs.GetString("SonarColor");
                    equipped.Play();
                }
            }
        }
        else
        {
            //Confirm purchase
            Debug.Log("Previous Balance was: "+ PlayerPrefs.GetInt(menuOptions[selectedIndex].name) + " Coins.");
            int temp_coins = PlayerPrefs.GetInt("Coins") - 100;
            PlayerPrefs.SetInt("Coins", temp_coins);
            Debug.Log("Purchase of " + menuOptions[selectedIndex].name + " completed.");
            Debug.Log("New Balance is: " + PlayerPrefs.GetInt("Coins") + " Coins.");
            promptOpen = false;

            purchasePrompt.time = 0;
            purchasePrompt.Stop();
            purchasePrompt.Evaluate();
            purchased.Play();
            PlayerPrefs.SetInt(menuOptions[selectedIndex].name, 1);
            PlayerPrefs.Save();
            coinCount.text = PlayerPrefs.GetInt("Coins") + " Coins";
            A_Text.text = "Equip";
            purchasedText.SetActive(true);
            colorCost.SetActive(false);
            notEnoughCoins.SetActive(false);
        }
    }


    void selectBack()
    {
        if (!promptOpen)
        {
            transition.playableAsset = transitionOutAsset;
            transition.Play();
            StartCoroutine("ReturnToMenu");
        }
        else
        {
            //set state of prompt transition to 0.
            promptOpen = false;
            purchasePrompt.Play(); 
        }
    }



    IEnumerator colorMenuOption()
    {

        foreach (GameObject option in menuOptions)
        {
            option.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        menuOptions[selectedIndex].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

        yield return null;
    }

    IEnumerator enlargeMenuOption()
    {

        foreach (GameObject option in menuOptions)
        {
            option.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        menuOptions[selectedIndex].transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);

        yield return null;
    }


    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator FindColorClip()
    {
        foreach (VideoClip clip in colorClips)
        {
            if(clip.name == menuOptions[selectedIndex].name)
            {
                colorClipPlayer.clip = clip;
                colorClipPlayer.Play();
                colorClipPlayer.isLooping = true;
            }
        }


        yield return null;
    }

    IEnumerator AxisDelay()
    {
        yield return new WaitForSeconds(0.15f);
        axisNav = true;
    }
}
