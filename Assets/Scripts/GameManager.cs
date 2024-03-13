using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void BackToMainMenu();
    public static event BackToMainMenu OnBackToMainMenu;  
    public TextMeshProUGUI score;
    public TextMeshProUGUI hiscore;
    public TextMeshProUGUI livesLeft;
    public GameObject ThirtyInvader;
    public GameObject TwentyInvader;
    public GameObject TenInvader;
    public GameObject invaderRoot;
    public GameObject invaderBullet;
    public GameObject mysteryRoot;
    public GameObject mysteryInvader;
    private GameObject newInvaderBullet;
    private int currentScore;
    private Vector3 currentPOS;
    private BoxCollider2D cd;
    private Vector3 invaderRootPOS;
    private float timePassed = 0f;
    private bool wallHit = false;
    private float speed = 0;
    private float timeElapsed = 0;
    private float spawnMystery = 0;
    private bool startCredits = false;
    private float adjustInvaderBulletSpawn;
    // private Animator Thirtyanimator;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // PlayerPrefs.DeleteKey("Hi-score");
        Debug.Log($"CURRENT POS: {currentPOS}");
        Enemy.OnEnemyDied += EnemyDied;
        Enemy.OnWallHit += OnWallHit;
        Player.OnPlayerHit += PlayerHit;
        MysteryInvader.OnMysteryDied += MysteryDied;
        Player.OnPlayerDeath += OnPlayerDeath;
        ReturnToMain.OnCreditsFinished += OnCreditsFinished;
    }

    void OnDestroy()
    {
        Player.OnPlayerHit -= PlayerHit;
        Enemy.OnEnemyDied -= EnemyDied;
        Enemy.OnWallHit -= OnWallHit;
        Player.OnPlayerDeath -= OnPlayerDeath;
        ReturnToMain.OnCreditsFinished -= OnCreditsFinished;
    }


    // Update is called once per frame
    void Update()
    {
        if(invaderRoot != null){
            invaderRootPOS = invaderRoot.transform.position;
            currentPOS = invaderRoot.transform.position;
            score.text = $"SCORE\n{currentScore.ToString("0000")}";
            hiscore.text = $"HISCORE\n {PlayerPrefs.GetInt("Hi-score", 0).ToString("0000")}";
            timePassed += Time.deltaTime;
            if(timePassed >= 1f && !wallHit){
                invaderRootPOS = invaderRoot.transform.position;
                invaderRootPOS.x = invaderRootPOS.x + 0.5f + speed;
                adjustInvaderBulletSpawn = invaderRootPOS.x;
                invaderRoot.transform.position = invaderRootPOS;
                timePassed = 0;
            }else if(timePassed >= 1f && wallHit)
            {
                invaderRootPOS = invaderRoot.transform.position;
                invaderRootPOS.x = invaderRootPOS.x - 0.5f - speed;
                adjustInvaderBulletSpawn = invaderRootPOS.x;
                invaderRoot.transform.position = invaderRootPOS;
                timePassed = 0;  
            }
            CheckInvaderGrid();
        }
        if(SceneManager.GetActiveScene().name == "Space Invaders")
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= 3f)
            {
                Debug.Log("Enemy Shot");
                Shoot();
                timeElapsed = 0;
            } 
        }

        if(SceneManager.GetActiveScene().name == "Space Invaders")
        {
            spawnMystery += Time.deltaTime;
            if(spawnMystery >= 12)
            {
                Instantiate(mysteryInvader, new Vector3(11f, 3.5f, 0f), Quaternion.identity, mysteryRoot.transform);
                mysteryInvader.GetComponent<AudioSource>().Play();
                spawnMystery = 0;
            }

        }

    }

    void EnemyDied(int points, float addingSpeed)
    {
        currentScore += points;
        if(currentScore > PlayerPrefs.GetInt("Hi-score", 0))
        {
            PlayerPrefs.SetInt("Hi-score", currentScore);

        }
        speed += addingSpeed;
    }

    void MysteryDied(int points)
    {
        currentScore += points;
        if(currentScore > PlayerPrefs.GetInt("Hi-score", 0))
        {
            PlayerPrefs.SetInt("Hi-score", currentScore);

        }
    }

    void PlayerHit(int lives)
    {
        livesLeft.text = $"Lives\n {lives.ToString()}";
    }

    public void OnWallHit(bool hitWall)
    {
        wallHit = hitWall;

        invaderRootPOS = invaderRoot.transform.position;
        invaderRootPOS.y = invaderRootPOS.y - 0.3f;
        invaderRoot.transform.position = invaderRootPOS;

    }

    public void Shoot()
    {
      GameObject[] thirtyInvaders = GameObject.FindGameObjectsWithTag("30 Invader");

      int randNum = UnityEngine.Random.Range(0, thirtyInvaders.Length);

    //   Debug.Log(gameObjects.Length);
      
      if(thirtyInvaders.Length > 0)
      {
        Debug.Log("ENEMY SHOT");
        GameObject thirtyInv = thirtyInvaders[randNum];

        Vector3 newPOS = thirtyInv.transform.position;

        // newPOS.x += adjustInvaderBulletSpawn;

        newPOS.z = 10;

        newInvaderBullet = Instantiate(invaderBullet, thirtyInv.transform.position, Quaternion.identity, transform);

        IgnoreCollisions();

        newInvaderBullet.GetComponent<Rigidbody2D>().gravityScale = 3f;

        newInvaderBullet.GetComponent<AudioSource>().Play();

        Destroy(newInvaderBullet, 3);
      }
      

    }


    void CheckInvaderGrid()
    {
        GameObject[] thirtyInvaders = GameObject.FindGameObjectsWithTag("30 Invader");
        if(thirtyInvaders.Length == 0 && !startCredits){
            startCredits = true;
            StartCoroutine(GoToCredits());
        }
    }

    IEnumerator GoToCredits()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("Credits");
        GameObject canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        // Destroy(gameObject);
        OnBackToMainMenu.Invoke();

    }

    void OnPlayerDeath()
    {
        spawnMystery = 0;
        StartCoroutine(GoToCredits());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Space Invaders");
        GameObject makeInvactive = GameObject.Find("Make Invactive");
        makeInvactive.SetActive(false);
        StartCoroutine(FindInvaderRoot());
    }

    IEnumerator FindInvaderRoot()
    {
        GameObject invaderRootObj = GameObject.Find("Invaders Root");
        while (invaderRootObj == null)
        {
            yield return null;
            invaderRootObj = GameObject.Find("Invaders Root");
        }
        invaderRoot = invaderRootObj;
        SpawnInvaders();
    }

    void SpawnInvaders()
    {
        cd = GetComponent<BoxCollider2D>();
        currentPOS = invaderRoot.transform.position;
        Vector3 oldPOS = invaderRoot.transform.position;
        oldPOS.y = oldPOS.y - 1;
        Vector3 oldPOS2 = invaderRoot.transform.position;
        oldPOS2.y = oldPOS2.y - 2;

        for(int i = 1; i <= 5; i++)
        {
            // Debug.Log($"Thirty Point Invader: {i}");
            Instantiate(ThirtyInvader, new Vector3(currentPOS.x + 1, currentPOS.y, 0), Quaternion.identity, invaderRoot.transform);
            currentPOS.x = currentPOS.x + 1;
        }

        for(int i = 1; i <= 5; i++)
        {
            // Debug.Log($"Thirty Point Invader: {i}");
            Instantiate(TwentyInvader, new Vector3(oldPOS.x + 1, oldPOS.y, 0), Quaternion.identity, invaderRoot.transform);
            oldPOS.x = oldPOS.x + 1;
        }

        for(int i = 1; i <= 5; i++)
        {
            // Debug.Log($"Thirty Point Invader: {i}");
            Instantiate(TenInvader, new Vector3(oldPOS2.x + 1, oldPOS2.y, 0), Quaternion.identity, invaderRoot.transform);
            oldPOS2.x = oldPOS2.x + 1;
        }
    }

    void OnCreditsFinished()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void IgnoreCollisions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Invaders");
        for(int i = 0; i < gameObjects.Length; i++)
        {
            Physics2D.IgnoreCollision(newInvaderBullet.GetComponent<BoxCollider2D>(), gameObjects[i].GetComponent<BoxCollider2D>());
        }
    }

    
}
