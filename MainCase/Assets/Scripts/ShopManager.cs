using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI appleTreePriceText;
    public TextMeshProUGUI pearTreePriceText;
    public TextMeshProUGUI pineappleTreePriceText;
    public List<GameObject> trees;
    public List<GameObject> treesToDelete;
    private GameManager gameManager;
    private int appleTreePrice;
    private int pearTreePrice;
    private int pineappleTreePrice;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdatePricesFromUI();
        LoadPurchasedTrees();
    }

    public void BuyAppleTree()
    {
        if (gameManager.GetPlayerMoney() >= appleTreePrice && !IsTreePurchased("AppleTree"))
        {
            gameManager.ChangeMoney(-appleTreePrice);
            trees[0].SetActive(true);
            treesToDelete[0].SetActive(false);
            SaveTreePurchase("AppleTree");
        }
    }

    public void BuyPearTree()
    {
        if (gameManager.GetPlayerMoney() >= pearTreePrice && !IsTreePurchased("PearTree"))
        {
            gameManager.ChangeMoney(-pearTreePrice);
            trees[1].SetActive(true);
            treesToDelete[1].SetActive(false);
            SaveTreePurchase("PearTree");
        }
    }

    public void BuyPineappleTree()
    {
        if (gameManager.GetPlayerMoney() >= pineappleTreePrice && !IsTreePurchased("PineappleTree"))
        {
            gameManager.ChangeMoney(-pineappleTreePrice);
            trees[2].SetActive(true);
            treesToDelete[2].SetActive(false);
            SaveTreePurchase("PineappleTree");
        }
    }

    private void UpdatePricesFromUI()
    {
        int.TryParse(appleTreePriceText.text, out appleTreePrice);
        int.TryParse(pearTreePriceText.text, out pearTreePrice);
        int.TryParse(pineappleTreePriceText.text, out pineappleTreePrice);
    }

    private void SaveTreePurchase(string treeKey)
    {
        PlayerPrefs.SetInt(treeKey, 1); // Aðacýn satýn alýndýðýný kaydet
        PlayerPrefs.Save();
    }

    private bool IsTreePurchased(string treeKey)
    {
        return PlayerPrefs.GetInt(treeKey, 0) == 1;
    }

    private void LoadPurchasedTrees()
    {
        if (IsTreePurchased("AppleTree"))
        {
            trees[0].SetActive(true);
            treesToDelete[0].SetActive(false);
        }
        if (IsTreePurchased("PearTree"))
        {
            trees[1].SetActive(true);
            treesToDelete[1].SetActive(false);
        }
        if (IsTreePurchased("PineappleTree"))
        {
            trees[2].SetActive(true);
            treesToDelete[2].SetActive(false);
        }
    }
}
