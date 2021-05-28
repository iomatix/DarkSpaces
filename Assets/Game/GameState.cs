using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static int coins = 0;
    public static bool isPlayerDead = false, isVictory = false, isRunning = true;
    public static int currentLevel = 1;

    public static int wavesCounter = 0;
    public static int wavesPerLevel = 3;

    public static bool isFriendlyFire = false;

    public static bool isBossAlive = false;
    public static float bossBarHealth, bossBarMaxHealth;


    public static void addCoins(int amount = 1)
    {
        coins += amount;
    }

    public static int getCoins()
    {
        return coins;
    }

    public static void setBossBar(float currHP, float maxHP)
    {
        bossBarHealth = currHP;
        bossBarMaxHealth = maxHP;
    }
    public static void setIsPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }
    public static void setIsVictory(bool isDead)
    {
        isPlayerDead = isVictory;
    }
    public static void nextWave(int wave = 1)
    {
        addWaves();
    }
    public static void addWaves(int wave = 1)
    {
        wavesCounter += wave;
    }

    public static void ReInitialize()
    {
        loadGame();
        isPlayerDead = false;
        wavesCounter = 0;
    }
    // Save/Load Handler
    public static void loadGame()
    {

    }
    public static void saveGame()
    {

    }


}
