using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHud : MonoBehaviour
{
    public GameObject PlayerObject;
    private EntityHandler playerHandler;
    public Transform HealthProgressBar, ShieldProgressBar;
    public Transform ScoreHolder;
    public Transform BossCanvasGroup, BossProgressBar;
    public Transform BasesProgressBar;
    [SerializeField] private float healthAmount, shieldAmount;
    [SerializeField] private float scoreAmount;

    public Transform LevelInfoCanvasGroup, LevelInfoText, LevelInfoSubText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setCoins();
        updatePlayer();
        updateBoss();
        updateBases();
        updateLevelInfo();
    }

    private void setCoins()
    {
        ScoreHolder.GetComponent<Text>().text = GameState.getCoins().ToString();
    }
    private void updatePlayer()
    {
        try
        {
            playerHandler = PlayerObject.GetComponent<EntityHandler>();
            HealthProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(HealthProgressBar.GetComponent<Image>().fillAmount, playerHandler.health / playerHandler.healthForce, 0.05f);
            ShieldProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(ShieldProgressBar.GetComponent<Image>().fillAmount, playerHandler.shields / playerHandler.shieldsForce, 0.05f);
        }
        catch (MissingReferenceException e)
        {
            HealthProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(HealthProgressBar.GetComponent<Image>().fillAmount, 0, 0.05f);
            ShieldProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(ShieldProgressBar.GetComponent<Image>().fillAmount, 0, 0.05f);
        }
    }

    private void updateBoss()
    {
        if (GameState.isBossAlive)
        {
            BossProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(BossProgressBar.GetComponent<Image>().fillAmount, GameState.bossBarHealth / GameState.bossBarMaxHealth, 0.05f);
            BossCanvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(BossCanvasGroup.GetComponent<CanvasGroup>().alpha, 1, 0.03f);
        }
        else
        {
            BossProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(BossProgressBar.GetComponent<Image>().fillAmount, 0, 0.05f);
            BossCanvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(BossCanvasGroup.GetComponent<CanvasGroup>().alpha, 0, 0.03f);

        }

    }

    private void updateBases()
    {
        List<GameObject> bases = new List<GameObject>();
        bases.AddRange(ExtensionMethods.getChilds(GameObject.FindGameObjectWithTag("Base")));
        float hp = 0, maxhp = 1;
        try
        {
            foreach (GameObject e in bases)
            {
                EntityHandler eh = e.GetComponent<EntityHandler>();
                hp += eh.health + eh.shields;
                maxhp += eh.healthForce + eh.shieldsForce;
            }

        }
        catch (NullReferenceException ex)
        {
            // Handle Exception
        }
        BasesProgressBar.GetComponent<Image>().fillAmount = Mathf.Lerp(BasesProgressBar.GetComponent<Image>().fillAmount, hp / maxhp, 0.03f);
    }

    private void updateLevelInfo()
    {
        LevelInfoCanvasGroup.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(LevelInfoCanvasGroup.GetComponent<CanvasGroup>().alpha, 0, 0.02f);
    }
    public void showInfo(string text, string subtext = "")
    {
        LevelInfoText.GetComponent<Text>().text = text;
        LevelInfoSubText.GetComponent<Text>().text = subtext;
        LevelInfoCanvasGroup.GetComponent<CanvasGroup>().alpha = 1;
    }
}
