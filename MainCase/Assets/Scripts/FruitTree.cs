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

    private int visibleFruitCount = 0;  // �u anda aktif olan meyve say�s�
    private int collectedFruits = 0;    // Bu a�a�tan sat��a kadar toplanan meyve say�s� (UI i�in)
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
        // S�reyle yeni meyveler aktif ediliyor
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

            // T�m aktif meyveler �zerinden ge�iliyor
            foreach (var fruit in fruits)
            {
                if (fruit.activeSelf)
                {
                    // Her meyve i�in �d�l ver (envanter g�ncellemesi ve XP eklemesi)
                    GiveRewards(fruit.name);
                    fruit.SetActive(false);
                    newlyCollectedFruits++;
                }
            }

            // Sat�� yap�lmad�ysa, toplanan meyveleri biriktiriyoruz.
            collectedFruits += newlyCollectedFruits;

            // Toplama saya�lar�n� s�f�rl�yoruz ki, sonraki toplamalarda yaln�zca o an aktif meyveler hesaba kats�n.
            visibleFruitCount = 0;
            timer = 0f;
        }
    }

    private void GiveRewards(string fruitName)
    {
        if (gameManager == null || inventoryManager == null)
            return;

        // Her meyve i�in ilgili envanter g�ncelleniyor ve XP ekleniyor.
        if (fruitName.StartsWith("Apple"))
        {
            inventoryManager.AddApples(1);   // Inventory'deki elma say�s�n� art�r�r
            gameManager.CollectFruit("Apple"); // XPManager.AddXP(2) �a�r�l�r
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

    // Sat�� sonras� �a�r�lan metod; bu a�a�taki UI say�s�n� s�f�rlar.
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