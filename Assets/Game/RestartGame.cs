using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameState.ReInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Restart") && GameState.isPlayerDead)
        {
            GameState.ReInitialize();
            Time.timeScale = 1;
            SceneManager.LoadScene("GameLoop");
        }
    }
}
