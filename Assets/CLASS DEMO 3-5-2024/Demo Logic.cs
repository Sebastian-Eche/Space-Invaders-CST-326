using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoLogic : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void ConsoleTest()
    {
        Debug.Log("CONSOLE TETS INVOKES");
    }

    public void StartGame()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Space Invaders");
        GameObject playerObj = GameObject.Find("Player");
        while(!asyncOp.isDone){
            yield return null;
            playerObj = GameObject.Find("Player");
        }

        Debug.Log(playerObj);

    }
}
