using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // UI elemanlar�
    public TextMeshProUGUI appleCountText;
    public TextMeshProUGUI pearCountText;
    public TextMeshProUGUI pineAppleCountText;

    // Ba�lant�lar
    private GameManager gameManager;
    private List<FruitTree> fruitTrees;

    // Envanter de�i�kenleri
    private int appleCount = 0;
    private int pearCount = 0;
    private int pineappleCount = 0;

    /// <summary>
    /// Oyun ba�lad���nda envanteri ba�lat�r ve verileri y�kler.
    /// </summary>
    private void Start()
    {
        // GameManager ve FruitTree bile�enlerini bul
        gameManager = FindObjectOfType<GameManager>();
        fruitTrees = new List<FruitTree>(FindObjectsOfType<FruitTree>());

        // PlayerPrefs'ten kaydedilmi� verileri y�kle
        appleCount = PlayerPrefs.GetInt("AppleCount", 0);
        pearCount = PlayerPrefs.GetInt("PearCount", 0);
        pineappleCount = PlayerPrefs.GetInt("PineappleCount", 0);

        // UI'yi g�ncelle
        UpdateUI();

        Debug.Log("Inventory loaded - Apples: " + appleCount + ", Pears: " + pearCount + ", Pineapples: " + pineappleCount);
    }

    /// <summary>
    /// Elma miktar�n� envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek elma miktar�</param>
    public void AddApples(int amount)
    {
        appleCount += amount;
        PlayerPrefs.SetInt("AppleCount", appleCount);
        appleCountText.text = appleCount.ToString();
        Debug.Log(appleCountText.text);
    }

    /// <summary>
    /// Armut miktar�n� envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek armut miktar�</param>
    public void AddPears(int amount)
    {
        pearCount += amount;
        PlayerPrefs.SetInt("PearCount", pearCount);
        pearCountText.text = pearCount.ToString();
        Debug.Log("Added " + amount + " pears. Total: " + pearCount);
    }

    /// <summary>
    /// Ananas miktar�n� envantere ekler.
    /// </summary>
    /// <param name="amount">Eklenecek ananas miktar�</param>
    public void AddPineapples(int amount)
    {
        pineappleCount += amount;
        PlayerPrefs.SetInt("PineappleCount", pineappleCount);
        pineAppleCountText.text = pineappleCount.ToString();
        Debug.Log("Added " + amount + " pineapples. Total: " + pineappleCount);
    }

    /// <summary>
    /// Elmalar� satar ve envanteri s�f�rlar.
    /// </summary>
    public void SellApples()
    {
        if (appleCount > 0)
        {
            int moneyEarned = appleCount * 2;
            gameManager.ChangeMoney(moneyEarned);

            // Envanteri s�f�rla
            appleCount = 0;
            PlayerPrefs.SetInt("AppleCount", 0);
            appleCountText.text = "0";
            Debug.Log(appleCountText.text);
            // A�a�lardaki toplanm�� meyveleri s�f�rla
            foreach (var tree in fruitTrees)
            {
                tree.ResetCollectedFruits();
            }
        }
    }

    /// <summary>
    /// Armutlar� satar ve envanteri s�f�rlar.
    /// </summary>
    public void SellPears()
    {
        if (pearCount > 0)
        {
            int moneyEarned = pearCount * 8;
            gameManager.ChangeMoney(moneyEarned);


            // Envanteri s�f�rla
            pearCount = 0;
            PlayerPrefs.SetInt("PearCount", 0);
            pearCountText.text = "0";

            // A�a�lardaki toplanm�� meyveleri s�f�rla
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
    /// Ananaslar� satar ve envanteri s�f�rlar.
    /// </summary>
    public void SellPineapples()
    {
        if (pineappleCount > 0)
        {
            int moneyEarned = pineappleCount * 5;
            gameManager.ChangeMoney(moneyEarned);
            Debug.Log("Sold " + pineappleCount + " pineapples. Money added: " + moneyEarned);

            // Envanteri s�f�rla
            pineappleCount = 0;
            PlayerPrefs.SetInt("PineappleCount", 0);
            pineAppleCountText.text = "0";

            // A�a�lardaki toplanm�� meyveleri s�f�rla
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
    /// UI elemanlar�n� g�nceller.
    /// </summary>
    private void UpdateUI()
    {
        appleCountText.text = appleCount.ToString();
        pearCountText.text = pearCount.ToString();
        pineAppleCountText.text = pineappleCount.ToString();
    }
}