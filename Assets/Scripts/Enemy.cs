using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public int points = 3;
  public delegate void EnemyDied(int pointsWorth, float plusSpeed);
  public static event EnemyDied OnEnemyDied;

  public delegate void PastBarricade();
  public static event PastBarricade OnPastBarricade;
  public delegate void WallHit(bool hitWall);
  public static event WallHit OnWallHit;
  public GameObject invaderBullet;
  public GameObject TwentyInvader;
  public GameObject TenInvader;
  public GameObject invaderGrid;
  private GameObject newInvaderBullet;
  public bool hitWall = false;
  private Vector3 currentPOS;
  // private Vector3 invaderGridPOS;
  private BoxCollider2D myBoxCollider;
  private float addingSpeed = 0;
  // private float timeElapsed = 0;
  void Update()
  {
    currentPOS = transform.position;
    myBoxCollider = GetComponent<BoxCollider2D>();

  }


    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D collision)
    {
      // Debug.Log(collision.gameObject.name);

      if(collision.gameObject.name == "Right Wall")
      {
        hitWall = true;
        OnWallHit.Invoke(hitWall);
      }else if(collision.gameObject.name == "Left Wall")
      {
        hitWall = false;
        OnWallHit.Invoke(hitWall);
      }else
      if(collision.gameObject.name == "Game Over")
      {
        Debug.Log("GAME OVER");
      }else if(collision.gameObject.CompareTag("Invader Bullet"))
      {
        // Debug.Log("INVADER BULLET");
        
      }
      else{
        Debug.Log("Ouch!");
        Destroy(collision.gameObject);
        addingSpeed = addingSpeed + 0.1f;
        OnEnemyDied.Invoke(points, addingSpeed);
        GetComponent<Animator>().SetTrigger("Dead");
      }

    }

    public void DeathAnimationComplete()
    {
      Destroy(gameObject);
    }

    public void DeathAnimationBegins()
    {
      Destroy(gameObject.GetComponent<Collider2D>());
    }

    // public void Shoot()
    // {
    //   GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("30 Invader");

    //   int randNum = Random.Range(0, gameObjects.Length);
      
    //   GameObject thirtyInv = gameObjects[randNum];

    //   newInvaderBullet = Instantiate(invaderBullet, thirtyInv.transform.position, Quaternion.identity, transform);

    //   newInvaderBullet.GetComponent<Rigidbody2D>().gravityScale = 3f;
      

    //   // for(int i = 0; i < gameObjects.Length; i++)
    //   // {
    //   //   Debug.Log($"gameObject at {i}: {gameObjects[i].name}");
    //   //   Physics2D.IgnoreCollision(newInvaderBullet.GetComponent<BoxCollider2D>(), gameObjects[i].GetComponent<BoxCollider2D>());
    //   // }

    // }

    void OnTriggerEnter2D(Collider2D other)
    {
      Debug.Log("Went passed the barricade");
      OnPastBarricade.Invoke();
      
    }

}
