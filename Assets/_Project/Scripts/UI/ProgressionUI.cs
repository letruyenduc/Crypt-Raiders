using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressionUI : MonoBehaviour
{
    [Header("General Info")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public Slider xpBar;
    public TextMeshProUGUI goldText;

    [Header("Skill Points")]
    public TextMeshProUGUI availablePointsText;
    public TextMeshProUGUI hpStatText;
    public TextMeshProUGUI physStatText;
    public TextMeshProUGUI magStatText;

    [Header("Reset Button")]
    public TextMeshProUGUI resetCostText;
    public Button resetButton;

    void Start()
    {
        // S'abonner aux événements pour rafraîchir l'UI
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnXPChanged += RefreshUI;
            LevelManager.Instance.OnLevelUp += RefreshUI;
            LevelManager.Instance.OnStatsChanged += RefreshUI; // On s'abonne ici
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged += RefreshUI;
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (LevelManager.Instance == null) return;

        // 1. Infos de base
        levelText.text = "NIVEAU " + LevelManager.Instance.currentLevel;
        xpText.text = $"{LevelManager.Instance.currentXP} / {LevelManager.Instance.xpToNextLevel}";
        xpBar.maxValue = LevelManager.Instance.xpToNextLevel;
        xpBar.value = LevelManager.Instance.currentXP;

        if (CurrencyManager.Instance != null)
            goldText.text = CurrencyManager.Instance.gold.ToString() + " G";

        // 2. Points et Stats investies
        availablePointsText.text = "Points : " + LevelManager.Instance.skillPoints;
        hpStatText.text = "HP : +" + (LevelManager.Instance.investedHP * 10);
        physStatText.text = "Phys : +" + (LevelManager.Instance.investedPhysique * 2);
        magStatText.text = "Spell : +" + (LevelManager.Instance.investedMagie * 2);

        // 3. Bouton Reset
        int cost = LevelManager.Instance.GetResetCost();
        resetCostText.text = $"RESET ({cost} G)";
        
        // Griser le bouton si on n'a pas assez d'argent
        if (CurrencyManager.Instance != null)
        {
            resetButton.interactable = CurrencyManager.Instance.gold >= cost && cost > 0;
        }
    }

    // Fonctions pour les boutons (à lier dans Unity)
    public void OnClickAddHP() { LevelManager.Instance.AddPointHP(); RefreshUI(); }
    public void OnClickAddPhys() { LevelManager.Instance.AddPointPhysique(); RefreshUI(); }
    public void OnClickAddMag() { LevelManager.Instance.AddPointMagie(); RefreshUI(); }
    
    public void OnClickReset() 
    { 
        LevelManager.Instance.ResetStats(); 
        RefreshUI(); 
    }
}
