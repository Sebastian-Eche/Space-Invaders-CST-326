using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int barrierHitPoints = 5;
   

     void OnCollisionEnter2D(Collision2D collision)
    {
      if(collision.gameObject.CompareTag("Invader Bullet") || collision.gameObject.CompareTag("Bullet")){
        Debug.Log("BARRIER HIT!");
        barrierHitPoints--;
        Destroy(collision.gameObject);
        if(barrierHitPoints <= 0){
          Destroy(gameObject);
        }
      }
    }
}
