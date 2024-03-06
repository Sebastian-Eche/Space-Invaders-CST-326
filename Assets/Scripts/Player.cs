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

  private Rigidbody2D rb;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    Enemy.OnEnemyDied += EnemyOnOnEnemyDied;
    Enemy.OnPastBarricade += OnPassedBarricade;


  }

  void OnDestroy()
  {
    Enemy.OnEnemyDied -= EnemyOnOnEnemyDied;
    Enemy.OnPastBarricade -= OnPassedBarricade;
  }

  void EnemyOnOnEnemyDied(int pointsWorth, float addingSpeed)
  {
    // can add a can shoot
     Debug.Log($"Player recieved 'EnemyDied' worth {pointsWorth}");
  }
    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        // GetComponent<Animator>().SetTrigger("Shoot Trigger");
        GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
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

    void SomeAnimationFrameCallback()
    {
      Debug.Log("something happens in animation!");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
      if(other.gameObject.CompareTag("Invader Bullet"))
      {
        Debug.Log("HIT");
        lives--;
        OnPlayerHit.Invoke(lives);
        Destroy(other.gameObject);
        checkForDeath();
      }
    }

    void checkForDeath()
    {
      if(lives <= 0)
      {
        Animator playerAni = GetComponent<Animator>();
        playerAni.SetTrigger("Dead");
      }
    }

    void PlayerDeathComplete()
    {
      Destroy(gameObject);
      SceneManager.LoadScene("Main Menu");
    }

    void OnPassedBarricade()
    {
      lives = 0;
      checkForDeath();
    }
}
