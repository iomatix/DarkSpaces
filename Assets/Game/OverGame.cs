using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverGame : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (GameState.isPlayerDead) StopGame();
    }
    public void StopGame(){
            GameState.saveGame();
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.35f, 0.015f);
    }
}
