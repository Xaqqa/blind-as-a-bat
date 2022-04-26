using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class player : MonoBehaviour
{
    [SerializeField] RawImage controlScheme;
    [SerializeField] Texture controls_pc;
    [SerializeField] Texture controls_xbox;

    [SerializeField] AudioSource coinCollect;
    [SerializeField] AudioSource batSqueak;
    [SerializeField] AudioSource highscore;
    [SerializeField] AudioSource batFlap;
    [SerializeField] AudioMixer masterMixer;

    public static bool countdownComplete;

    bool turningLeft;
    bool turningRight;
    bool turningDown;
    bool turningUp;
    bool sonarFired;

    bool buttonsPrompted;
    [SerializeField] PlayableAsset buttonsOut;
    [SerializeField] PlayableDirector buttons;

    [SerializeField] PlayableDirector eye_glow;
    [SerializeField] PlayableAsset eye_out;
    [SerializeField] PlayableAsset eye_dead;

    int coinsInSession = 0;
    bool highscorePopped;
    int longestDistance;
    int distanceTravelled;
    [SerializeField] TextMeshProUGUI distanceCounter;
    [SerializeField] PlayableDirector highscorePopup;

    [SerializeField] TextMeshProUGUI deathHighscore;
    [SerializeField] TextMeshProUGUI deathScore;
    [SerializeField] PlayableDirector death;

    [SerializeField] PlayableDirector transition;

    [SerializeField] float speed;
    [SerializeField] float turnAngle;

    [SerializeField] GameObject sonarLight;
    [SerializeField] RawImage eyeGlow;
    public static float sonarSpeed = 0.3f;

    void Awake()
    {
        string temp_color = PlayerPrefs.GetString("SonarColor");

        if (temp_color == "White")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(1f, 1f, 1f);
            eyeGlow.color = new Color(1f, 1f, 1f);
        }
        else if (temp_color == "Yellow")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(.98f, 1f, 0f);
            eyeGlow.color = new Color(.98f, 1f, 0f);
        }
        else if (temp_color == "Orange")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(1f, .5f, 0f);
            eyeGlow.color = new Color(1f, .5f, 0f);
        }
        else if (temp_color == "Red")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(1f, .2f, .2f);
            eyeGlow.color = new Color(1f, .2f, .2f);
        }
        else if (temp_color == "Green")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(.31f, 1f, 0f);
            eyeGlow.color = new Color(.31f, 1f, 0f);
        }
        else if (temp_color == "Blue")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(0f, .74f, 1f);
            eyeGlow.color = new Color(0f, .74f, 1f);
        }
        else if (temp_color == "Purple")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(.54f, 0f, 1f);
            eyeGlow.color = new Color(.54f, 0f, 1f);
        }
        else if (temp_color == "Pink")
        {
            sonarLight.GetComponentInChildren<Light>().color = new Color(1f, 0f, 1f);
            eyeGlow.color = new Color(1f, 0f, 1f);
        }



        buttonsPrompted = true;
        #region Sensitivity Config
        if (PlayerPrefs.GetInt("sensitivityLevel") == 1)
        {
            turnAngle = 0.3f;
        }
        else if (PlayerPrefs.GetInt("sensitivityLevel") == 2)
        {
            turnAngle = 0.4f;
        }
        else if (PlayerPrefs.GetInt("sensitivityLevel") == 3)
        {
            turnAngle = 0.5f;
        }
        else if (PlayerPrefs.GetInt("sensitivityLevel") == 4)
        {
            turnAngle = 0.6f;
        }
        else if (PlayerPrefs.GetInt("sensitivityLevel") == 5)
        {
            turnAngle = 0.7f;
        }
        #endregion

        #region Volume Config
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
        #endregion


        if (PlayerPrefs.GetString("buttonPrompts") == "PC")
        {
            controlScheme.texture = controls_pc;
        }
        else if (PlayerPrefs.GetString("buttonPrompts") == "Xbox")
        {
            controlScheme.texture = controls_xbox;
        }
    }

    void Start()
    {
        longestDistance = PlayerPrefs.GetInt("Highscore");
        countdownComplete = false;
    }

    void Update()
    {

        distanceCounter.text = distanceTravelled.ToString();
        sonarSpeed = speed + 0.24f;


        if(PlayerPrefs.GetString("buttonPrompts") == "PC")
        {
            if (Input.GetKeyDown(KeyCode.Space) && !sonarFired && speed > 0f && countdownComplete)
            {
                StartCoroutine("fireSonar");
                if (buttonsPrompted)
                {
                    buttons.playableAsset = buttonsOut;
                    buttons.Play();
                    buttonsPrompted = false;
                }
                eye_glow.playableAsset = eye_out;
                eye_glow.Play();
            }
            if(Input.GetKeyDown(KeyCode.Space) && death.state == PlayState.Playing)
            {
                StartCoroutine("skipDeath");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && speed > 0f)
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningDown = true;
                }
                else
                {
                    turningUp = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningDown = false;
                }
                else
                {
                    turningUp = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && speed > 0f)
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningUp = true;
                }
                else
                {
                    turningDown = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningUp = false;
                }
                else
                {
                    turningDown = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && speed > 0f)
            {
                turningLeft = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                transform.eulerAngles += new Vector3(0f, 0f, -transform.eulerAngles.z);
                turningLeft = false;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && speed > 0f)
            {
                turningRight = true;
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                transform.eulerAngles += new Vector3(0f, 0f, -transform.eulerAngles.z);
                turningRight = false;
            }
        }

        else if (PlayerPrefs.GetString("buttonPrompts") == "Xbox")
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) && !sonarFired && speed > 0f && countdownComplete)
            {
                StartCoroutine("fireSonar");
                if (buttonsPrompted)
                {
                    buttons.playableAsset = buttonsOut;
                    buttons.Play();
                    buttonsPrompted = false;
                }
                eye_glow.playableAsset = eye_out;
                eye_glow.Play();
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton0) && death.state == PlayState.Playing)
            {
                StartCoroutine("skipDeath");
            }

            if (Input.GetAxisRaw("Vertical") > 0 && speed > 0f)
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningUp = true;
                    turningDown = false;
                }
                else
                {
                    turningUp = false;
                    turningDown = true;
                }
            }

            if (Input.GetAxisRaw("Vertical") < 0 && speed > 0f)
            {
                if (PlayerPrefs.GetString("invertYToggle") == "X")
                {
                    turningUp = false;
                    turningDown = true;
                }
                else
                {
                    turningUp = true;
                    turningDown = false;
                }

            }

            if (Input.GetAxisRaw("Horizontal") < 0 && speed > 0f)
            {
                turningRight = false;
                transform.eulerAngles += new Vector3(0f, 0f, -transform.eulerAngles.z);
                turningLeft = true;
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && speed > 0f)
            {
                turningLeft = false;
                transform.eulerAngles += new Vector3(0f, 0f, -transform.eulerAngles.z);
                turningRight = true;
            }
            
            if(Input.GetAxisRaw("Horizontal") == 0)
            {
                turningLeft = false;
                turningRight = false;
                transform.eulerAngles += new Vector3(0f, 0f, -transform.eulerAngles.z);
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                turningUp = false;
                turningDown = false;
            }
        } 
    }

    void FixedUpdate()
    {
        if (countdownComplete)
        {
            transform.Translate(Vector3.forward * speed);
        }

        if (turningUp)
        {
            transform.eulerAngles -= new Vector3(turnAngle, 0f, 0f);
        }
        else if (turningDown)
        {
            transform.eulerAngles += new Vector3(turnAngle, 0f, 0f);
        }

        if (turningLeft)
        {
            transform.eulerAngles -= new Vector3(0f, turnAngle, 0f);
        }
        else if (turningRight)
        {
            transform.eulerAngles += new Vector3(0f, turnAngle, 0f);
        }
    }

    IEnumerator fireSonar()
    {
        batSqueak.Play();
        Instantiate(sonarLight, transform.position, transform.rotation);
        sonarFired = true;

        yield return new WaitForSeconds(2.5f);

        sonarFired = false;
    }

    IEnumerator deathSequence()
    {
        eye_glow.playableAsset = eye_dead;
        eye_glow.Play();
        speed = 0;
        if (highscorePopped)
        {
            longestDistance = distanceTravelled;
        }
        PlayerPrefs.SetInt("Highscore", longestDistance);
        PlayerPrefs.Save();
        deathHighscore.text = "Highscore - " + longestDistance;
        deathScore.text = "Score - " + distanceTravelled;
        death.Play();

        yield return new WaitForSeconds(5f);
        transition.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator skipDeath()
    {
        transition.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("MainMenu");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            distanceTravelled++;
            speed += 0.001f;
            batFlap.pitch = (Random.Range(0.8f, 1.2f));
            batFlap.Play();
            if (distanceTravelled > longestDistance && !highscorePopped)
            {
                highscorePopped = true;
                highscore.Play();
                highscorePopup.Play();
            }
        }
        else if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinsInSession++;
            coinCollect.Play();
        }
        else if (other.CompareTag("Death"))
        {
            int temp_coin = PlayerPrefs.GetInt("Coins");
            temp_coin += coinsInSession;
            PlayerPrefs.SetInt("Coins", temp_coin);
            PlayerPrefs.Save();

            StartCoroutine("deathSequence");
        }
        else
        {
            //Do Nothing
        }
    }
}
