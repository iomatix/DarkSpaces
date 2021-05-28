using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryGame : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (GameState.isVictory) Victory();
    }
    public void Victory(){
            GameState.saveGame();
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.35f, 0.015f);
    }
}
