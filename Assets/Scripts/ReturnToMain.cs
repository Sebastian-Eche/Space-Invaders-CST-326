using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    public delegate void CreditsFinshed();
    public static event CreditsFinshed OnCreditsFinished;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnToMain()
    {
        Debug.Log("INVOKEED");
        OnCreditsFinished.Invoke();
    }

}
