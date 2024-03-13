using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //technique for making sure there isn't a null reference during runtime if you are going to use get component
public class Bullet : MonoBehaviour
{
  public delegate void CanShoot();
  public static event CanShoot OnCanShoot;
  private Rigidbody2D myRigidbody2D;

  public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
      myRigidbody2D = GetComponent<Rigidbody2D>();
      Fire();
    }

    // Update is called once per frame
    private void Fire()
    {
      myRigidbody2D.velocity = Vector2.up * speed; 
      Debug.Log("Wwweeeeee");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
      Debug.Log($"2D: {other.gameObject.name}");
      OnCanShoot.Invoke();
    }

    void OnCollisionEnter(Collision other)
    {
      Debug.Log(other.gameObject.name);
      OnCanShoot.Invoke();
    }
}
