using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Leveling")]
    public int currentLevel = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;
    public float xpMultiplier = 1.2f; // De plus en plus dur

    [Header("Skill Points")]
    public int skillPoints = 0;
    public int investedHP = 0;
    public int investedPhysique = 0;
    public int investedMagie = 0;

    public System.Action OnLevelUp;
    public System.Action OnXPChanged;
    public System.Action OnStatsChanged;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        
        OnXPChanged?.Invoke();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        skillPoints++; // 1 point par niveau comme Dungeon Quest
        
        // Calcul du prochain palier (exponentiel)
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpMultiplier);
        
        OnLevelUp?.Invoke();
        Debug.Log("LEVEL UP ! Niveau : " + currentLevel);
    }

    // Fonctions pour dépenser les points
    public void AddPointHP() 
    { 
        if (skillPoints > 0) 
        { 
            investedHP++; 
            skillPoints--; 
            Debug.Log($"LevelManager: Point HP ajouté. Nouveau total : {investedHP}, Points restants : {skillPoints}");
            UpdatePlayerStats(); 
        } 
        else Debug.LogWarning("LevelManager: Pas de points de compétence disponibles !");
    }

    public void AddPointPhysique() 
    { 
        if (skillPoints > 0) 
        { 
            investedPhysique++; 
            skillPoints--; 
            Debug.Log($"LevelManager: Point Physique ajouté. Nouveau total : {investedPhysique}, Points restants : {skillPoints}");
            UpdatePlayerStats(); 
        } 
        else Debug.LogWarning("LevelManager: Pas de points de compétence disponibles !");
    }

    public void AddPointMagie() 
    { 
        if (skillPoints > 0) 
        { 
            investedMagie++; 
            skillPoints--; 
            Debug.Log($"LevelManager: Point Magie ajouté. Nouveau total : {investedMagie}, Points restants : {skillPoints}");
            UpdatePlayerStats(); 
        } 
        else Debug.LogWarning("LevelManager: Pas de points de compétence disponibles !");
    }

    public int GetResetCost()
    {
        int totalPoints = investedHP + investedPhysique + investedMagie;
        return totalPoints * 50; 
    }

    public void ResetStats()
    {
        int cost = GetResetCost();
        Debug.Log($"LevelManager: Tentative de Reset. Coût : {cost}, Or actuel : {(CurrencyManager.Instance != null ? CurrencyManager.Instance.gold : -1)}");
        
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendGold(cost))
        {
            skillPoints += (investedHP + investedPhysique + investedMagie);
            investedHP = 0;
            investedPhysique = 0;
            investedMagie = 0;
            Debug.Log("LevelManager: Reset réussi ! Points récupérés.");
            UpdatePlayerStats();
        }
        else
        {
            Debug.LogWarning("LevelManager: Reset échoué (Pas assez d'or ou Manager manquant)");
        }
    }

    private void UpdatePlayerStats()
    {
        // On informe les autres managers (s'ils existent)
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UpdatePlayerStats();
        }

        // On informe l'UI
        OnStatsChanged?.Invoke(); 
        
        // --- NOUVEAUTÉ : AUTO-SAVE SÉCURISÉ ---
        if (SaveManager.Instance != null) 
        {
            SaveManager.Instance.SaveGame();
        }
    }
}
