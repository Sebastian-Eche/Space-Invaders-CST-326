using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
  public GameObject bullet;
  public Transform shottingOffset;
  public float acceleration;
  static public int lives = 3;
  public delegate void PlayerHit(int lives);
  public static event PlayerHit OnPlayerHit; 
  public delegate void PlayerDeath();
  public static event PlayerDeath OnPlayerDeath; 

  public AudioClip playerDeath;
  public AudioClip playerShoots;

  private Rigidbody2D rb;
  private bool canShoot;
  private bool passedAlready;

  void Start()
  {
    canShoot = true;
    rb = GetComponent<Rigidbody2D>();
    Enemy.OnPastBarricade += OnPassedBarricade;
    Bullet.OnCanShoot += OnCanShoot;
    GameManager.OnBackToMainMenu += OnBackToMainMenu;


  }

  void OnDestroy()
  {
    Enemy.OnPastBarricade -= OnPassedBarricade;
    Bullet.OnCanShoot -= OnCanShoot;
    GameManager.OnBackToMainMenu += OnBackToMainMenu;
  }
    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space) && canShoot)
      {
        canShoot = false;
        GetComponent<Animator>().SetTrigger("Shoot");
        GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.PlayOneShot(playerShoots);
        Debug.Log("Bang!");

        Destroy(shot, 3f);
      }

      

      float horizontalMovement = Input.GetAxis("Horizontal");

      rb.velocity += Vector2.right * horizontalMovement * Time.deltaTime * acceleration;

      if(Math.Abs(horizontalMovement) < .50f)
        {
            Vector3 newVel = rb.velocity;
            newVel.x *= 0.7f - Time.deltaTime;
            rb.velocity = newVel;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
      if(other.gameObject.CompareTag("Invader Bullet"))
      {
        Debug.Log("HIT");
        lives--;
        GetComponent<AudioSource>().Play();
        OnPlayerHit.Invoke(lives);
        Destroy(other.gameObject);
        checkForDeath();
      }
    }

    void checkForDeath()
    {
      if(lives <= 0)
      {
        AudioSource audioSrc = GetComponent<AudioSource>();
        Animator playerAni = GetComponent<Animator>();
        playerAni.SetTrigger("Dead");
        audioSrc.PlayOneShot(playerDeath);
        OnPlayerDeath.Invoke();
      }
    }

    public void PlayerDeathComplete()
    {
      Destroy(gameObject);
    }

    void OnPassedBarricade()
    {
      if(!passedAlready){
        passedAlready = true;
        lives = 0;
        checkForDeath(); 
      }
    }

    void OnCanShoot()
    {
      canShoot = true;
    }

    void OnBackToMainMenu()
    {
      lives = 3;
    }
}
