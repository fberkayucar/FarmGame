using UnityEngine;
using TMPro;

public class XPManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    private int currentLevel = 1;
    private int currentXP = 0;
    private int requiredXP = 100; // Ýlk seviye için gerekli XP

    private void Start()
    {
        LoadXPData();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= requiredXP)
        {
            LevelUp();
        }
        SaveXPData();
        UpdateXPUI();
    }

    private void LevelUp()
    {
        currentXP -= requiredXP;
        currentLevel++;
        requiredXP = Mathf.CeilToInt(requiredXP * 1.2f); // Gereken XP %20 artar
    }

    private void LoadXPData()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetInt("PlayerXP", 0);
        requiredXP = PlayerPrefs.GetInt("RequiredXP", 100);
        UpdateXPUI();
    }

    private void SaveXPData()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("RequiredXP", requiredXP);
        PlayerPrefs.Save();
    }

    private void UpdateXPUI()
    {
        if (levelText != null) levelText.text = currentLevel.ToString();
        if (xpText != null) xpText.text = currentXP + " / " + requiredXP;
    }
}
