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

    private void OnDestroy()
    {
        // TRÈS IMPORTANT : On se désabonne pour éviter de garder des liens morts
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnXPChanged -= RefreshUI;
            LevelManager.Instance.OnLevelUp -= RefreshUI;
            LevelManager.Instance.OnStatsChanged -= RefreshUI;
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged -= RefreshUI;
        }
    }

    public void RefreshUI()
    {
        if (LevelManager.Instance == null) return;

        // 1. Infos de base
        if (levelText != null) levelText.text = "NIVEAU " + LevelManager.Instance.currentLevel;
        if (xpText != null) xpText.text = $"{LevelManager.Instance.currentXP} / {LevelManager.Instance.xpToNextLevel}";
        if (xpBar != null)
        {
            xpBar.maxValue = LevelManager.Instance.xpToNextLevel;
            xpBar.value = LevelManager.Instance.currentXP;
        }

        if (CurrencyManager.Instance != null && goldText != null)
            goldText.text = CurrencyManager.Instance.gold.ToString() + " G";

        // 2. Points et Stats investies
        if (availablePointsText != null) availablePointsText.text = "Points : " + LevelManager.Instance.skillPoints;
        if (hpStatText != null) hpStatText.text = "HP : +" + (LevelManager.Instance.investedHP * 10);
        if (physStatText != null) physStatText.text = "Phys : +" + (LevelManager.Instance.investedPhysique * 2);
        if (magStatText != null) magStatText.text = "Spell : +" + (LevelManager.Instance.investedMagie * 2);

        // 3. Bouton Reset
        int cost = LevelManager.Instance.GetResetCost();
        if (resetCostText != null) resetCostText.text = $"RESET ({cost} G)";
        
        // Griser le bouton si on n'a pas assez d'argent
        if (CurrencyManager.Instance != null && resetButton != null)
        {
            resetButton.interactable = CurrencyManager.Instance.gold >= cost && cost > 0;
        }
    }

    // Fonctions pour les boutons (à lier dans Unity)
    public void OnClickAddHP() 
    { 
        if (LevelManager.Instance != null)
        {
            Debug.Log("ProgressionUI: Clic Add HP. Points restants : " + LevelManager.Instance.skillPoints);
            LevelManager.Instance.AddPointHP(); 
            // Note: RefreshUI() est appelé automatiquement par l'évent OnStatsChanged
        }
        else Debug.LogError("ProgressionUI: LevelManager.Instance est INTROUVABLE !");
    }

    public void OnClickAddPhys() 
    { 
        if (LevelManager.Instance != null)
        {
            Debug.Log("ProgressionUI: Clic Add Physique. Points restants : " + LevelManager.Instance.skillPoints);
            LevelManager.Instance.AddPointPhysique(); 
        }
        else Debug.LogError("ProgressionUI: LevelManager.Instance est INTROUVABLE !");
    }

    public void OnClickAddMag() 
    { 
        if (LevelManager.Instance != null)
        {
            Debug.Log("ProgressionUI: Clic Add Magie. Points restants : " + LevelManager.Instance.skillPoints);
            LevelManager.Instance.AddPointMagie(); 
        }
        else Debug.LogError("ProgressionUI: LevelManager.Instance est INTROUVABLE !");
    }
    
    public void OnClickReset() 
    { 
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetStats(); 
        }
    }
}
