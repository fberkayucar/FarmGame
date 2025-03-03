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

    private int visibleFruitCount = 0;  
    private int collectedFruits = 0;    
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

            foreach (var fruit in fruits)
            {
                if (fruit.activeSelf)
                {
                    GiveRewards(fruit.name);
                    fruit.SetActive(false);
                    newlyCollectedFruits++;
                }
            }

            collectedFruits += newlyCollectedFruits;

            visibleFruitCount = 0;
            timer = 0f;
        }
    }

    private void GiveRewards(string fruitName)
    {
        if (gameManager == null || inventoryManager == null)
            return;

        if (fruitName.StartsWith("Apple"))
        {
            inventoryManager.AddApples(1);  
            gameManager.CollectFruit("Apple"); 
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