using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // UI elemanlarý
    public TextMeshProUGUI appleCountText;
    public TextMeshProUGUI pearCountText;
    public TextMeshProUGUI pineAppleCountText;

    // Baðlantýlar
    private GameManager gameManager;
    private List<FruitTree> fruitTrees;

    // Envanter deðiþkenleri
    private int appleCount = 0;
    private int pearCount = 0;
    private int pineappleCount = 0;

    /// <summary>
    /// Oyun baþladýðýnda envanteri baþlatýr ve verileri yükler.
    /// </summary>
    private void Start()
    {
        // GameManager ve FruitTree bileþenlerini bul
        gameManager = FindObjectOfType<GameManager>();
        fruitTrees = new List<FruitTree>(FindObjectsOfType<FruitTree>());

        // PlayerPrefs'ten kaydedilmiþ verileri yükle
        appleCount = PlayerPrefs.GetInt("AppleCount", 0);
        pearCount = PlayerPrefs.GetInt("PearCount", 0);
        pineappleCount = PlayerPrefs.GetInt("PineappleCount", 0);

        // UI'yi güncelle
        UpdateUI();

        Debug.Log("Inventory loaded - Apples: " + appleCount + ", Pears: " + pearCount + ", Pineapples: " + pineappleCount);
    }

    /// <summary>
    /// Elma miktarýný envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek elma miktarý</param>
    public void AddApples(int amount)
    {
        appleCount += amount;
        PlayerPrefs.SetInt("AppleCount", appleCount);
        appleCountText.text = appleCount.ToString();
        Debug.Log(appleCountText.text);
    }

    /// <summary>
    /// Armut miktarýný envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek armut miktarý</param>
    public void AddPears(int amount)
    {
        pearCount += amount;
        PlayerPrefs.SetInt("PearCount", pearCount);
        pearCountText.text = pearCount.ToString();
        Debug.Log("Added " + amount + " pears. Total: " + pearCount);
    }

    /// <summary>
    /// Ananas miktarýný envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek ananas miktarý</param>
    public void AddPineapples(int amount)
    {
        pineappleCount += amount;
        PlayerPrefs.SetInt("PineappleCount", pineappleCount);
        pineAppleCountText.text = pineappleCount.ToString();
        Debug.Log("Added " + amount + " pineapples. Total: " + pineappleCount);
    }

    /// <summary>
    /// Elmalarý satar ve envanteri sýfýrlar.
    /// </summary>
    public void SellApples()
    {
        if (appleCount > 0)
        {
            int moneyEarned = appleCount * 2;
            gameManager.ChangeMoney(moneyEarned);

            // Envanteri sýfýrla
            appleCount = 0;
            PlayerPrefs.SetInt("AppleCount", 0);
            appleCountText.text = "0";
            Debug.Log(appleCountText.text);
            // Aðaçlardaki toplanmýþ meyveleri sýfýrla
            foreach (var tree in fruitTrees)
            {
                tree.ResetCollectedFruits();
            }
        }
    }

    /// <summary>
    /// Armutlarý satar ve envanteri sýfýrlar.
    /// </summary>
    public void SellPears()
    {
        if (pearCount > 0)
        {
            int moneyEarned = pearCount * 8;
            gameManager.ChangeMoney(moneyEarned);


            // Envanteri sýfýrla
            pearCount = 0;
            PlayerPrefs.SetInt("PearCount", 0);
            pearCountText.text = "0";

            // Aðaçlardaki toplanmýþ meyveleri sýfýrla
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

    /// <summary>
    /// Ananaslarý satar ve envanteri sýfýrlar.
    /// </summary>
    public void SellPineapples()
    {
        if (pineappleCount > 0)
        {
            int moneyEarned = pineappleCount * 5;
            gameManager.ChangeMoney(moneyEarned);
            Debug.Log("Sold " + pineappleCount + " pineapples. Money added: " + moneyEarned);

            // Envanteri sýfýrla
            pineappleCount = 0;
            PlayerPrefs.SetInt("PineappleCount", 0);
            pineAppleCountText.text = "0";

            // Aðaçlardaki toplanmýþ meyveleri sýfýrla
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

    /// <summary>
    /// UI elemanlarýný günceller.
    /// </summary>
    private void UpdateUI()
    {
        appleCountText.text = appleCount.ToString();
        pearCountText.text = pearCount.ToString();
        pineAppleCountText.text = pineappleCount.ToString();
    }
}