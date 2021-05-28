using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    private bool isLevelLoaded = false;

    public GameObject objectSpawner;
    private EnemySpawner enemySpawner;
    public GameObject bossSpawner;


    public GameObject player;
    public GameObject bases;
    public GameObject playerHud;
    private EntityHandler playerHandler;
    private List<EntityHandler> basesHandler;
    private PlayerHud hudHandler;

    // Parameters
    public int bonusCoinsPerLevel = 35;
    public float playerBonusHealthPerLevel = 135, playerBonusShieldPerLevel = 85;
    public float baseBonusHealthPerLevel = 250, baseBonusShieldPerLevel = 450;

    void Start()
    {
        basesHandler = new List<EntityHandler>();
        enemySpawner = objectSpawner.GetComponent<EnemySpawner>();
        playerHandler = player.GetComponent<EntityHandler>();
        hudHandler = playerHud.GetComponent<PlayerHud>();
        foreach (GameObject e in ExtensionMethods.getChilds(bases))
        {
            basesHandler.Add(e.GetComponent<EntityHandler>());
        }
        LoadLevel();
    }

    void FixedUpdate()
    {
        if (isLevelLoaded) GameState.isRunning = true;
        else LoadLevel();

        // To proceed win with boss and waves
        if (GameState.wavesCounter > GameState.wavesPerLevel && GameState.isBossAlive == false)
        {
            GameState.isRunning = false;
            NextLevel(bonusCoinsPerLevel * (GameState.currentLevel - 1), playerBonusHealthPerLevel * GameState.currentLevel, playerBonusShieldPerLevel * GameState.currentLevel, baseBonusHealthPerLevel * GameState.currentLevel, baseBonusShieldPerLevel * GameState.currentLevel);
        }
        if (GameState.isBossAlive) UpdateBoss();
        if (GameState.isPlayerDead) showDeadInfo();

    }






    // Scripted level difficulty

    public void LoadLevel()
    {
        if (GameState.currentLevel <= 0) GameState.currentLevel = 1;
        else if (GameState.currentLevel == 1)
        {
            SetupLevel(9, 2.33f, 20, wavesCountLimit: 4);
            SetupSkybox(Resources.Load("Env/SpaceSkies/Pink1K") as Material);
            levelIntegrationHandler();

        }
        else if (GameState.currentLevel == 2)
        {
            SetupLevel(7, 3.33f, 30, wavesCountLimit: 5);
            SetupSkybox(Resources.Load("Env/SpaceSkies/Purple1K") as Material);
            levelIntegrationHandler();
        }
        else if (GameState.currentLevel == 3)
        {
            SetupLevel(3, 2.5f, 16, wavesCountLimit: 4, isBossLevel: true);
            SpawnBoss(Resources.Load("Enemies/Prefabs/EnemyBoss") as GameObject);
            SetupSkybox(Resources.Load("Env/SpaceSkies/Green1K") as Material);
            levelIntegrationHandler();
        }

        // Victory always on the bottom 
        else
        {
            GameState.isRunning = false;
            hudHandler.showInfo("Victory!");
            GameState.isVictory = true;
        }


    }
    private void levelIntegrationHandler()
    {
        if (GameState.isBossAlive) hudHandler.showInfo("Level " + GameState.currentLevel, "Kill the Boss and clear " + GameState.wavesPerLevel + " waves to proceed!");
        else hudHandler.showInfo("Level " + GameState.currentLevel, "Clear " + GameState.wavesPerLevel + " waves to proceed!");
        isLevelLoaded = true;
    }
    public void NextLevel(int bonusCoins = 1, float bonusHeatlh = 75, float bonusShield = 35, float bonusBaseHeatlh = 350, float bonusBaseShield = 750)
    {
        GameState.currentLevel++;
        GameState.wavesCounter = 1;
        addBonusRewards(bonusCoins, bonusHeatlh, bonusShield, bonusBaseHeatlh, bonusBaseShield);
        LoadLevel();
    }

    private void SetupLevel(int waveStart = 7, float waveStep = 2.33f, int waveMax = 26,
        float healthBonusWave = 25f, float shieldBonusWave = 15f, float shieldRegenBonusWave = 0.03f,
        float healthBonusLevel = 115f, float shieldBonusLevel = 95f, float shieldRegenBonusLevel = 0.26f,
        int wavesCountLimit = 3, bool isBossLevel = false)
    {
        GameState.isBossAlive = isBossLevel;
        GameState.wavesPerLevel = wavesCountLimit;
        enemySpawner.waveCountStart = waveStart;
        enemySpawner.waveCountStep = waveStep;
        enemySpawner.waveCountMax = waveMax;

        enemySpawner.HealthBonusPerLevel = healthBonusLevel;
        enemySpawner.ShieldBonusPerLevel = shieldBonusLevel;
        enemySpawner.ShieldRegenBonusPerLevel = shieldRegenBonusLevel;
        enemySpawner.HealthBonusPerWave = healthBonusWave;
        enemySpawner.ShieldBonusPerWave = shieldBonusWave;
        enemySpawner.ShieldRegenBonusPerWave = shieldRegenBonusWave;


        playerHandler.SoftInitialize();
        foreach (EntityHandler e in basesHandler) e.SoftInitialize();
    }
    private void SpawnBoss(GameObject boss)
    {
        Instantiate(boss, bossSpawner.GetComponent<Transform>());
    }
    private void SetupSkybox(Material sky)
    {
        RenderSettings.skybox = sky;
    }
    public void addBonusRewards(int coins = 1, float heatlh = 75, float shield = 35, float baseHeatlh = 350, float baseShield = 750, float sheldRegenForce = 0.77f)
    {

        GameState.addCoins(coins);
        SpawnRandomReward();
        playerHandler.shieldsForce += shield;
        playerHandler.healthForce += heatlh;
        playerHandler.shieldRegenAmount += sheldRegenForce;
        foreach (EntityHandler e in basesHandler)
        {
            e.healthForce += baseHeatlh;
            e.shieldsForce += baseShield;
            e.shieldRegenAmount += sheldRegenForce;
        }
    }

    private void SpawnRandomReward()
    {
        switch (UnityEngine.Random.Range(0, 7))
        {
            case 5:
                Instantiate(Resources.Load("PowerUps/Prefabs/BasesPowerEgg") as GameObject, bossSpawner.transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(Resources.Load("PowerUps/Prefabs/HealthEgg") as GameObject, bossSpawner.transform.position, Quaternion.identity);
                break;
            case 3:
                Instantiate(Resources.Load("PowerUps/Prefabs/PowerEgg") as GameObject, bossSpawner.transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(Resources.Load("PowerUps/Prefabs/ShieldEgg") as GameObject, bossSpawner.transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(Resources.Load("PowerUps/Prefabs/Coin") as GameObject, bossSpawner.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }

    }
    public void showDeadInfo()
    {
        hudHandler.showInfo("YOU DIED", "Press R to restart current level");
    }
    public void UpdateBoss()
    {
        List<GameObject> BossesHolder = ExtensionMethods.getChilds(bossSpawner, "Enemy");
        if (!(BossesHolder.Count > 0)) GameState.isBossAlive = false;
        //update BossBar
        else
        {
            GameState.bossBarHealth = 0;
            GameState.bossBarMaxHealth = 0;
            foreach (GameObject e in BossesHolder)
            {

                try
                {
                    EntityHandler eh = e.GetComponent<EntityHandler>();

                    GameState.bossBarHealth += e.GetComponent<EntityHandler>().health + e.GetComponent<EntityHandler>().shields;
                    GameState.bossBarMaxHealth += e.GetComponent<EntityHandler>().healthForce + e.GetComponent<EntityHandler>().shieldsForce;

                }
                catch (NullReferenceException err)
                {
                    // Handle Exception
                }
            }
        }
    }
}

