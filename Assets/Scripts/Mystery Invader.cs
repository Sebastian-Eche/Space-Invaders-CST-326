using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryInvader : MonoBehaviour
{
    public delegate void MysteryDied(int pointsWorth);
    public static event MysteryDied OnMysteryDied;

    int points = 0;

    float timeElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        PickPoints();
        // Debug.Log();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= 5)
        {
            Debug.Log("MYSTERY COMING");
            Animator animator = GetComponent<Animator>();
            animator.Play("MoveAcrossScreen", 0);
            timeElapsed = 0;
        }
        
    }

    void PickPoints()
    {
        int[] mysteryPoints = {25, 50, 100, 150, 200};
        int randomNum = Random.Range(0, mysteryPoints.Length-1);
        points = mysteryPoints[randomNum];
        // Debug.Log(points);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            GetComponent<Animator>().SetTrigger("Dead");
        }
    }

    public void MoveAcrossScreenComplete()
    {
        Destroy(gameObject);
    }

    public void DeathAnimationComplete()
    {
        OnMysteryDied.Invoke(points);
        Destroy(gameObject);
    }


}
