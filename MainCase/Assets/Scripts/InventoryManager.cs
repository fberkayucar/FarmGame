using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public TextMeshProUGUI appleCountText;
    public TextMeshProUGUI pearCountText;
    public TextMeshProUGUI pineAppleCountText;

    private GameManager gameManager;
    private List<FruitTree> fruitTrees;

    private int appleCount = 0;
    private int pearCount = 0;
    private int pineappleCount = 0;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        fruitTrees = new List<FruitTree>(FindObjectsOfType<FruitTree>());

        appleCount = PlayerPrefs.GetInt("AppleCount", 0);
        pearCount = PlayerPrefs.GetInt("PearCount", 0);
        pineappleCount = PlayerPrefs.GetInt("PineappleCount", 0);

        UpdateUI();

        Debug.Log("Inventory loaded - Apples: " + appleCount + ", Pears: " + pearCount + ", Pineapples: " + pineappleCount);
    }

    public void AddApples(int amount)
    {
        appleCount += amount;
        PlayerPrefs.SetInt("AppleCount", appleCount);
        appleCountText.text = appleCount.ToString();
        Debug.Log(appleCountText.text);
    }

    public void AddPears(int amount)
    {
        pearCount += amount;
        PlayerPrefs.SetInt("PearCount", pearCount);
        pearCountText.text = pearCount.ToString();
        Debug.Log("Added " + amount + " pears. Total: " + pearCount);
    }

    public void AddPineapples(int amount)
    {
        pineappleCount += amount;
        PlayerPrefs.SetInt("PineappleCount", pineappleCount);
        pineAppleCountText.text = pineappleCount.ToString();
        Debug.Log("Added " + amount + " pineapples. Total: " + pineappleCount);
    }

    public void SellApples()
    {
        if (appleCount > 0)
        {
            int moneyEarned = appleCount * 2;
            gameManager.ChangeMoney(moneyEarned);

            appleCount = 0;
            PlayerPrefs.SetInt("AppleCount", 0);
            appleCountText.text = "0";
            Debug.Log(appleCountText.text);
            foreach (var tree in fruitTrees)
            {
                tree.ResetCollectedFruits();
            }
        }
    }

    public void SellPears()
    {
        if (pearCount > 0)
        {
            int moneyEarned = pearCount * 8;
            gameManager.ChangeMoney(moneyEarned);

            pearCount = 0;
            PlayerPrefs.SetInt("PearCount", 0);
            pearCountText.text = "0";

            foreach (var tree in fruitTrees)
            {
                tree.ResetCollectedFruits();
            }
        }
        else
        {
            Debug.Log("No pears to sell!");
        }
    }

    public void SellPineapples()
    {
        if (pineappleCount > 0)
        {
            int moneyEarned = pineappleCount * 5;
            gameManager.ChangeMoney(moneyEarned);
            Debug.Log("Sold " + pineappleCount + " pineapples. Money added: " + moneyEarned);

            pineappleCount = 0;
            PlayerPrefs.SetInt("PineappleCount", 0);
            pineAppleCountText.text = "0";

            foreach (var tree in fruitTrees)
            {
                tree.ResetCollectedFruits();
            }
        }
        else
        {
            Debug.Log("No pineapples to sell!");
        }
    }

    private void UpdateUI()
    {
        appleCountText.text = appleCount.ToString();
        pearCountText.text = pearCount.ToString();
        pineAppleCountText.text = pineappleCount.ToString();
    }
}