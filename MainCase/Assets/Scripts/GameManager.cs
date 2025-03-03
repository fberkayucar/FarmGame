using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<TextMeshProUGUI> moneyTexts;
    public XPManager xpManager; 

    private int playerMoney;
    private const string VERSION_KEY = "GameVersion";
    private void Start()
    {

        string savedVersion = PlayerPrefs.GetString(VERSION_KEY, "");
        if (savedVersion != Application.version)
        {
            PlayerPrefs.DeleteAll(); 
            PlayerPrefs.SetString(VERSION_KEY, Application.version); 
            PlayerPrefs.Save(); 
            Debug.Log("Yeni build algýlandý, PlayerPrefs sýfýrlandý.");
        }
        LoadMoney();
    }


    private void LoadMoney()
    {
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            playerMoney = 200;
            PlayerPrefs.SetInt("PlayerMoney", playerMoney);
            PlayerPrefs.Save();
        }
        else
        {
            playerMoney = PlayerPrefs.GetInt("PlayerMoney");
        }
        UpdateMoneyUI();
    }

    public void ChangeMoney(int amount)
    {
        playerMoney += amount;
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
        PlayerPrefs.Save();
        UpdateMoneyUI();
    }

    public int GetPlayerMoney()
    {
        return playerMoney;
    }

    private void UpdateMoneyUI()
    {
        foreach (var moneyText in moneyTexts)
        {
            if (moneyText != null)
                moneyText.text = playerMoney.ToString();
        }
    }

    public void CollectFruit(string fruitType)
    {
        switch (fruitType)
        {
            case "Apple":
                xpManager.AddXP(2);
                break;
            case "Pineapple":
                xpManager.AddXP(5);
                break;
            case "Pear":
                xpManager.AddXP(10);
                break;
        }
    }
}
