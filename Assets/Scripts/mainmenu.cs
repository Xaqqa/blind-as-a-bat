using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class mainmenu : MonoBehaviour
{
    [SerializeField] GameObject playerAuthentication;
    bool axisNav = true;
    bool loggedIn;
    [SerializeField] PlayableDirector loginAnimation;
    [SerializeField] TextMeshProUGUI loginText;
    [SerializeField] RawImage loginButton;

    public static bool playerAuthXbox;
    public static bool checkSignedIn;
    public static bool confirmSignedIn;
    [SerializeField] Text xboxGamertag;

    [SerializeField] PlayableDirector transition;
    [SerializeField] PlayableAsset transitionOutAsset;

    [SerializeField] List<GameObject> optionsOptions;
    [SerializeField] int selectedOptionsIndex;
    bool optionsOpen = false;
    [SerializeField] PlayableDirector optionsWindow;
    [SerializeField] PlayableAsset optionsFadeIn;
    [SerializeField] PlayableAsset optionsFadeOut;

    [SerializeField] List<GameObject> menuOptions;
    [SerializeField] List<GameObject> toolbarText;
    [SerializeField] int selectedIndex;
    [SerializeField] GameObject guiSelector;
    [SerializeField] GameObject guiParticles;

    bool creditsOpen = false;
    [SerializeField] PlayableDirector creditsWindow;
    [SerializeField] PlayableAsset creditsFadeIn;
    [SerializeField] PlayableAsset creditsFadeOut;

    [SerializeField] AudioMixer masterMixer;

    private void Awake()
    {
        if (PlayerPrefs.GetString("defaultOptions") == "")
        {
            PlayerPrefs.SetInt("volumeLevel", 5);
            PlayerPrefs.SetInt("sensitivityLevel", 3);
            PlayerPrefs.SetString("invertYToggle", "  ");

            PlayerPrefs.SetString("SonarColor", "White"); //Sets default sonar color
            PlayerPrefs.SetInt("White", 1); //Purchased (Default)
            PlayerPrefs.SetInt("Yellow", 0); //Unpurchased
            PlayerPrefs.SetInt("Orange", 0); //Unpurchased
            PlayerPrefs.SetInt("Red", 0); //Unpurchased
            PlayerPrefs.SetInt("Green", 0); //Unpurchased
            PlayerPrefs.SetInt("Blue", 0); //Unpurchased
            PlayerPrefs.SetInt("Purple", 0); //Unpurchased
            PlayerPrefs.SetInt("Pink", 0); //Unpurchased

            PlayerPrefs.SetInt("Coins", 0); //Zero Coins

            PlayerPrefs.SetString("defaultOptions", "false");
            PlayerPrefs.Save();

        }
        
        selectedIndex = 0;
        selectedOptionsIndex = 0;
    }

    void Start()
    {

        checkAudio();
        
        //StartCoroutine("ReplaceTransition");

        StartCoroutine("enlargeMenuOption");
        StartCoroutine("colorMenuOption");
        StartCoroutine("particleMenuOption");

        StartCoroutine("CheckLogin");
    }

    void Update()
    {
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
            else if (Input.GetKeyDown(KeyCode.Tab) && loginButton.enabled)
            {
                playerAuthXbox = true;
                loginAnimation.Stop();
                loginText.color = new Color(1f, 1f, 1f, 0f);
                loginText.color = new Color(1f, 1f, 1f, 1f);
                loginText.text = "Loading...";
                //StartCoroutine("LoginGamertag");
                loginButton.enabled = false;
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
            else if (Input.GetKeyDown(KeyCode.JoystickButton9) && loginButton.enabled)
            {
                playerAuthXbox = true;
                loginAnimation.Stop();
                loginText.color = new Color(1f, 1f, 1f, 0f);
                loginText.color = new Color(1f, 1f, 1f, 1f);
                loginText.text = "Loading...";
                //StartCoroutine("LoginGamertag");
                loginButton.enabled = false;
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
            else if (Input.GetKeyDown(KeyCode.JoystickButton7) && loginButton.enabled)
            {
                playerAuthXbox = true;
                loginAnimation.Stop();
                loginText.color = new Color(1f, 1f, 1f, 0f);
                loginText.color = new Color(1f, 1f, 1f, 1f);
                loginText.text = "Loading...";
                //StartCoroutine("LoginGamertag");
                loginButton.enabled = false;
            }
        }
    }

    void selectUp()
    {
        if (!creditsOpen)
        {
            if (optionsOpen)
            {
                if (selectedOptionsIndex == 0)
                {
                    selectedOptionsIndex = optionsOptions.Count - 1;
                }
                else
                {
                    selectedOptionsIndex--;
                }
                StartCoroutine("colorOptionsMenuOption");
            }
            else
            {
                if (selectedIndex == 0)
                {
                    selectedIndex = menuOptions.Count - 1;
                }
                else
                {
                    selectedIndex--;
                }
                StartCoroutine("enlargeMenuOption");
                StartCoroutine("colorMenuOption");
                StartCoroutine("particleMenuOption");
            }
        }
    }

    void selectDown()
    {
        if (!creditsOpen)
        {
            if (optionsOpen)
            {
                if (selectedOptionsIndex == optionsOptions.Count - 1)
                {
                    selectedOptionsIndex = 0;
                }
                else
                {
                    selectedOptionsIndex++;
                }
                StartCoroutine("colorOptionsMenuOption");
            }
            else
            {
                if (selectedIndex == menuOptions.Count - 1)
                {
                    selectedIndex = 0;
                }
                else
                {
                    selectedIndex++;
                }
                StartCoroutine("enlargeMenuOption");
                StartCoroutine("colorMenuOption");
                StartCoroutine("particleMenuOption");
            }
        }
    }

    void selectConfirm()
    {
        if (!optionsOpen)
        {
            if (menuOptions[selectedIndex].name == "Play")
            {
                Debug.Log("Starting Game...");
                guiParticles.SetActive(false);
                transition.playableAsset = transitionOutAsset;
                transition.Play();
                StartCoroutine("StartGame");

            }

            else if (menuOptions[selectedIndex].name == "Store")
            {
                transition.playableAsset = transitionOutAsset;
                transition.Play();
                StartCoroutine("StartStore");
                
            }

            else if (menuOptions[selectedIndex].name == "Credits" && !creditsOpen)
            {
                Debug.Log("Opening Credits...");
                creditsWindow.playableAsset = creditsFadeIn;
                creditsWindow.Play();
                creditsOpen = true;
                StartCoroutine("particleMenuOption");

            }
            else if (menuOptions[selectedIndex].name == "Options")
            {
                optionsWindow.playableAsset = optionsFadeIn;
                optionsWindow.Play();
                optionsOpen = true;
                StartCoroutine("particleMenuOption");
                StartCoroutine("colorOptionsMenuOption");
                StartCoroutine("setOptions");
            }
            else if (menuOptions[selectedIndex].name == "Exit")
            {
                Debug.Log("Exiting Application...");
                Application.Quit();
            }
        }
        else
        {
            if (optionsOptions[selectedOptionsIndex].name == "Invert Look")
            {
                Debug.Log("Invert Look");
                string temp_inv = PlayerPrefs.GetString("invertYToggle");
                if (temp_inv == "X")
                {
                    temp_inv = "  ";
                }
                else
                {
                    temp_inv = "X";
                }
                optionsOptions[selectedOptionsIndex].GetComponent<TextMeshProUGUI>().text = "Invert Y Axis " + "[" + temp_inv + "]";
                PlayerPrefs.SetString("invertYToggle", temp_inv);
                PlayerPrefs.SetString("defaultOptions", "false");
                PlayerPrefs.Save();

            }
            else if (optionsOptions[selectedOptionsIndex].name == "Sensitivity")
            {
                Debug.Log("Change Sensitivity");
                int temp_sens = PlayerPrefs.GetInt("sensitivityLevel");
                if (temp_sens == 5)
                {
                    temp_sens = 1;
                }
                else
                {
                    temp_sens++;
                }
                optionsOptions[selectedOptionsIndex].GetComponent<TextMeshProUGUI>().text = "Sensitivity " + "[" + temp_sens + "]";
                PlayerPrefs.SetInt("sensitivityLevel", temp_sens);
                PlayerPrefs.SetString("defaultOptions", "false");
                PlayerPrefs.Save();
            }
            else if (optionsOptions[selectedOptionsIndex].name == "Volume")
            {
                Debug.Log("Change Volume");
                int temp_vol = PlayerPrefs.GetInt("volumeLevel");
                if (temp_vol == 5)
                {
                    temp_vol = 0;
                }
                else
                {
                    temp_vol++;
                }
                optionsOptions[selectedOptionsIndex].GetComponent<TextMeshProUGUI>().text = "Volume " + "[" + temp_vol + "]";
                PlayerPrefs.SetInt("volumeLevel", temp_vol);
                PlayerPrefs.SetString("defaultOptions", "false");
                PlayerPrefs.Save();
                checkAudio();
            }
        }
    }
  
    void selectBack()
    {
        if (creditsOpen)
        {
            creditsOpen = false;
            creditsWindow.playableAsset = creditsFadeOut;
            creditsWindow.Play();
            StartCoroutine("particleMenuOption");
        }
        else if (optionsOpen)
        {
            optionsOpen = false;
            optionsWindow.playableAsset = optionsFadeOut;
            optionsWindow.Play();
            StartCoroutine("particleMenuOption");
        }
    }

    void checkAudio()
    {
        if (PlayerPrefs.GetInt("volumeLevel") == 0)
        {
            masterMixer.SetFloat("masterVol", -80f);
        }
        else if (PlayerPrefs.GetInt("volumeLevel") == 1)
        {
            masterMixer.SetFloat("masterVol", -60f);
        }
        else if (PlayerPrefs.GetInt("volumeLevel") == 2)
        {
            masterMixer.SetFloat("masterVol", -40f);
        }
        else if (PlayerPrefs.GetInt("volumeLevel") == 3)
        {
            masterMixer.SetFloat("masterVol", -20f);
        }
        else if (PlayerPrefs.GetInt("volumeLevel") == 4)
        {
            masterMixer.SetFloat("masterVol", -15f);
        }
        else if (PlayerPrefs.GetInt("volumeLevel") == 5)
        {
            masterMixer.SetFloat("masterVol", 0f);
        }
    }



    IEnumerator setOptions()
    {
        foreach (GameObject option in optionsOptions)
        {
            if (option.name == "Invert Look")
            {
                string temp_inv = PlayerPrefs.GetString("invertYToggle");
                option.GetComponent<TextMeshProUGUI>().text = "Invert Y Axis " + "[" + temp_inv + "]";
            }
            else if (option.name == "Volume")
            {
                int temp_vol = PlayerPrefs.GetInt("volumeLevel");
                option.GetComponent<TextMeshProUGUI>().text = "Volume " + "[" + temp_vol + "]";
            }
            else if (option.name == "Sensitivity")
            {
                int temp_sens = PlayerPrefs.GetInt("sensitivityLevel");
                option.GetComponent<TextMeshProUGUI>().text = "Sensitivity " + "[" + temp_sens + "]";
            }
        }
        yield return null;
    }
    IEnumerator resizeGUIselector()
    {
        guiSelector.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char character in menuOptions[selectedIndex].GetComponent<TextMeshProUGUI>().text)
        {
            guiSelector.GetComponent<TextMeshProUGUI>().text += "_";
        }
        yield return null;
    }

    IEnumerator nudgeMenuOption()
    {
        foreach (GameObject option in menuOptions)
        {
            option.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        }
        menuOptions[selectedIndex].GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        yield return null;
    }

    IEnumerator colorMenuOption()
    {

        foreach (GameObject option in menuOptions)
        {
            option.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        menuOptions[selectedIndex].GetComponent<TextMeshProUGUI>().color = new Color(1f,1f,1f);

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

    IEnumerator particleMenuOption()
    {
        if (creditsOpen || optionsOpen)
        {
            guiParticles.SetActive(false);
        }
        else
        {
            guiParticles.SetActive(true);
            guiParticles.transform.position = menuOptions[selectedIndex].transform.position;
        }
        yield return null;
    }

    IEnumerator colorOptionsMenuOption()
    {

        foreach (GameObject option in optionsOptions)
        {
            option.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        optionsOptions[selectedOptionsIndex].GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f);

        yield return null;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator StartStore()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Store");
    }

    IEnumerator AxisDelay()
    {
        yield return new WaitForSeconds(0.15f);
        axisNav = true;
    }
    
    IEnumerator LoginGamertag()
    {
        loginText.color = new Color(1f, 1f, 1f, 1f);
        loginText.text = "Loading";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading.";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading..";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading...";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading.";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading..";
        yield return new WaitForSeconds(0.5f);
        loginText.text = "Loading...";
        yield return new WaitForSeconds(0.5f);

        loginText.text = "Logged in as " + xboxGamertag.text;
    }

    IEnumerator CheckLogin()
    {

        checkSignedIn = true;

        yield return new WaitForSeconds(0.1f);
        if (confirmSignedIn)
        {
            loginAnimation.Stop();
            loginText.color = new Color(1f, 1f, 1f, 0f);
            loginText.color = new Color(1f, 1f, 1f, 1f);
            loginText.text = "Loading...";
  
            StartCoroutine("LoginGamertag"); //change eventually
            loginButton.enabled = false;
        }
    }

    IEnumerator ReplaceTransition()
    {
        yield return new WaitForSeconds(.1f);
        transition.Play();
        yield return new WaitForSeconds(1.5f);
        //transition.playableAsset = transitionOut;
    }
}