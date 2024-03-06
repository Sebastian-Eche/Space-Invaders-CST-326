using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGrid : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 currentPOS;
    void Start()
    {
        currentPOS = transform.position;
        // Enemy.HitWallMove += MovingGrid;
    }

    void OnDestroy()
    {
        // Enemy.HitWallMove -= MovingGrid;
    }

    // void MovingGrid()
    // {
    //     currentPOS.y = currentPOS.y - 0.5f;
    //     transform.Translate(currentPOS);
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
