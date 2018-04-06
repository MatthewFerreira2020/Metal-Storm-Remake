using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public character Player;
    public Scene currentScene;
    static GameManager instance = null;
    public Canvas lifecanvas;
    public Text livetext;
    public Image canvasimage;
    public Sprite gameoversprite;
    bool levelobjectsfound;
    float timer = 400;
    public Text timertext;
    public Text currentscoretext;

    int finalscore;
    int stagebonusval = 10000;
    int timebonusval;
    float totalscoreval;
    public Text areascore;
    public Text stagebonus;
    public Text timebonus;
    public Text totalscore;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance && instance.GetInstanceID() != GetInstanceID())
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == "level 1")
        {
            livetext.text = "" + Player.lives;
        }


    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "level 1")
        {
            if (levelobjectsfound == false)
            {
                timer = 400;

                findlevelobjects();
            }
                timer -= Time.deltaTime;
                timertext.text = "" + (int)timer;
                currentscoretext.text = "" + PlayerPrefs.GetInt("score");
                if (timer <= 0)
                {
                    PlayerPrefs.SetInt("win", 0);
                    LoadScoreScreen();
                }
            

            if (Input.GetButtonDown("Fire1"))
            {
                lifecanvas.gameObject.SetActive(false);
            }

            if (Player.health <= 0 && Input.GetButtonDown("Fire1"))
            {
                lifecanvas.gameObject.SetActive(false);
                Player.anim.SetBool("Dead", false);
                if (Player.gravityFliped)
                {
                    Player.flipGravity();
                    Player.isFliping = true;
                }
                Player.transform.position = Player.SpawnPoint.position;
                Player.health = 1;
            }
            if (Player.lives <= 0 && Input.GetButtonDown("Fire1"))
            {
                PlayerPrefs.SetInt("win", 0);
                LoadScoreScreen();
            }
        }

        if (currentScene.name == "Score Screen" && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Credits");
        }
        if (currentScene.name == "Credits" && Input.GetButtonDown("Fire1"))
        {
            levelobjectsfound = false;
            SceneManager.LoadScene("Title");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
        if (currentScene.name == "Title" && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("level 1");
        }


        
    }

    public void ChangeLevelCanvas()
    {

        if(Player.lives > 0)
        {
            livetext.text = "" + Player.lives;
            StartCoroutine(waitforanim());
        }
        if (Player.lives == 0)
        {
            livetext.text = "";
            canvasimage.sprite = gameoversprite;
            lifecanvas.gameObject.SetActive(true);
        }
        
    } 

    public void QuitGame()
    {
        Application.Quit();

    }

    public void LoadScoreScreen()
    {
        SceneManager.LoadScene("Score Screen");

        StartCoroutine(loadscore());
        
    }

    public IEnumerator waitforanim()
    {
        yield return new WaitForSeconds(0.75f);
        lifecanvas.gameObject.SetActive(true);
    }

    public IEnumerator loadscore()
    {
        yield return new WaitForSeconds(2f);
        findscorescreentext();
    }

    public void findlevelobjects()
    {
        lifecanvas = GameObject.Find("LifeCanvas").GetComponent<Canvas>();
        Player = GameObject.Find("Player").GetComponent<character>();
        livetext = GameObject.Find("LifeText").GetComponent<Text>();
        canvasimage = GameObject.Find("LifeImage").GetComponent<Image>();
        timertext = GameObject.Find("TimerText").GetComponent<Text>();
        currentscoretext = GameObject.Find("CurrentScoreText").GetComponent<Text>();
        levelobjectsfound = true;
        Player.lives = 2;

    }

    public void findscorescreentext()
    {
        if (SceneManager.GetActiveScene().name == "Score Screen")
        {
            areascore = GameObject.Find("AreaScoreValue").GetComponent<Text>();
            stagebonus = GameObject.Find("StageBonusValue").GetComponent<Text>();
            timebonus = GameObject.Find("TimeBonusValue").GetComponent<Text>();
            totalscore = GameObject.Find("TotalScoreValue").GetComponent<Text>();

            if (PlayerPrefs.GetInt("win") == 1)
            {
                areascore.text = "" + PlayerPrefs.GetInt("score") + "PTS";
                stagebonus.text = "" + stagebonusval + "PTS";
                timebonusval = Mathf.FloorToInt(timer * 100);
                timebonus.text = "" + timebonusval + "PTS";
                totalscoreval = stagebonusval + timebonusval + PlayerPrefs.GetInt("score");
                totalscore.text = "" + totalscoreval + "PTS";

            }
            else if (PlayerPrefs.GetInt("win") == 0)
            {
                areascore.text = "" + PlayerPrefs.GetInt("score") + "PTS";
                stagebonus.text = "0PTS";
                timebonus.text = "0PTS";
                totalscore.text = "" + PlayerPrefs.GetInt("score") + "PTS";
            }
        }
    }
}
