using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public List<GameObject> fruits = new List<GameObject>();
    public TextMeshProUGUI fruitCounterText;
    public float interval = 10f;
    public string treeID;

    private int visibleFruitCount = 0;  // Þu anda aktif olan meyve sayýsý
    private int collectedFruits = 0;    // Bu aðaçtan satýþa kadar toplanan meyve sayýsý (UI için)
    private float timer = 0f;
    private GameManager gameManager;
    private InventoryManager inventoryManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (!IsTreePurchased(treeID))
        {
            gameObject.SetActive(false);
            return;
        }

        LoadPreviousProgress();
    }

    private void Update()
    {
        // Süreyle yeni meyveler aktif ediliyor
        if (visibleFruitCount < fruits.Count)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                fruits[visibleFruitCount].SetActive(true);
                visibleFruitCount++;
                timer = 0f;
            }
        }
    }

    private void OnMouseDown()
    {
        if (visibleFruitCount > 0)
        {
            int newlyCollectedFruits = 0;

            // Tüm aktif meyveler üzerinden geçiliyor
            foreach (var fruit in fruits)
            {
                if (fruit.activeSelf)
                {
                    // Her meyve için ödül ver (envanter güncellemesi ve XP eklemesi)
                    GiveRewards(fruit.name);
                    fruit.SetActive(false);
                    newlyCollectedFruits++;
                }
            }

            // Satýþ yapýlmadýysa, toplanan meyveleri biriktiriyoruz.
            collectedFruits += newlyCollectedFruits;

            // Toplama sayaçlarýný sýfýrlýyoruz ki, sonraki toplamalarda yalnýzca o an aktif meyveler hesaba katsýn.
            visibleFruitCount = 0;
            timer = 0f;
        }
    }

    private void GiveRewards(string fruitName)
    {
        if (gameManager == null || inventoryManager == null)
            return;

        // Her meyve için ilgili envanter güncelleniyor ve XP ekleniyor.
        if (fruitName.StartsWith("Apple"))
        {
            inventoryManager.AddApples(1);   // Inventory'deki elma sayýsýný artýrýr
            gameManager.CollectFruit("Apple"); // XPManager.AddXP(2) çaðrýlýr
        }
        else if (fruitName.StartsWith("Pineapple"))
        {
            inventoryManager.AddPineapples(1);
            gameManager.CollectFruit("Pineapple");
        }
        else if (fruitName.StartsWith("Pear"))
        {
            inventoryManager.AddPears(1);
            gameManager.CollectFruit("Pear");
        }
    }

    // Satýþ sonrasý çaðrýlan metod; bu aðaçtaki UI sayýsýný sýfýrlar.
    public void ResetCollectedFruits()
    {
        collectedFruits = 0;
        visibleFruitCount = 0;
        timer = 0f;
        foreach (var fruit in fruits)
        {
            fruit.SetActive(false);
        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastExitTime", DateTime.UtcNow.ToString());
        PlayerPrefs.SetInt($"VisibleFruitCount_{treeID}", visibleFruitCount);
        PlayerPrefs.Save();
    }

    private void LoadPreviousProgress()
    {
        if (PlayerPrefs.HasKey("LastExitTime"))
        {
            string lastExitTimeString = PlayerPrefs.GetString("LastExitTime");
            if (DateTime.TryParse(lastExitTimeString, out DateTime lastExitTime))
            {
                TimeSpan elapsedTime = DateTime.UtcNow - lastExitTime;
                int fruitsToGenerate = Mathf.FloorToInt((float)elapsedTime.TotalSeconds / interval);
                visibleFruitCount = Mathf.Clamp(PlayerPrefs.GetInt($"VisibleFruitCount_{treeID}", 0) + fruitsToGenerate, 0, fruits.Count);

                for (int i = 0; i < visibleFruitCount; i++)
                {
                    fruits[i].SetActive(true);
                }
            }
        }
    }

    private bool IsTreePurchased(string treeKey)
    {
        return PlayerPrefs.GetInt(treeKey, 0) == 1;
    }
}