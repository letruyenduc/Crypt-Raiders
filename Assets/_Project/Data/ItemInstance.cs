using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData template;
    
    [Header("Statistiques Actuelles (Après Tirage)")]
    public int actualMainStat; 
    public int actualBonusHP;
    public int actualBonusPhysique;
    public int actualBonusMagie;

    [Header("Améliorations")]
    public int maxUpgrades;    
    public int currentUpgrades = 0;

    public ItemInstance(ItemData sourceData)
    {
        this.template = sourceData;

        // --- 1. TIRAGE INDÉPENDANT DES STATISTIQUES (+/- 10%) ---
        this.actualMainStat = RollStat(this.template.mainStatValue);
        this.actualBonusHP = RollStat(this.template.bonusHP);
        this.actualBonusPhysique = RollStat(this.template.bonusPhysique);
        this.actualBonusMagie = RollStat(this.template.bonusMagie);

        // --- 2. TIRAGE DU NOMBRE D'AMÉLIORATIONS ---
        int baseMaxUpgrades = GetBaseMaxUpgrades(this.template.rarity);
        int upgradeVariance = Random.Range(-1, 2); 
        this.maxUpgrades = Mathf.Max(0, baseMaxUpgrades + upgradeVariance);

        this.currentUpgrades = 0;
    }

    // Fonction utilitaire stricte pour calculer la variance
    private int RollStat(int baseStat)
    {
        if (baseStat == 0) return 0; // On ignore les statistiques vides
        float rollMultiplier = Random.Range(0.90f, 1.10f);
        return Mathf.RoundToInt(baseStat * rollMultiplier);
    }

    private int GetBaseMaxUpgrades(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Commun: return 3;
            case ItemRarity.Rare: return 5;
            case ItemRarity.Epic: return 8;
            case ItemRarity.Legendaire: return 12;
            default: return 0;
        }
    }

    // --- CALCUL DES MULTIPLICATEURS D'AMÉLIORATION ---
    // Les améliorations augmentent TOUTES les stats de l'objet de 10% par niveau.
    
    public float GetUpgradeMultiplier()
    {
        return 1f + (currentUpgrades * 0.10f);
    }
    
    public float GetPotentialMultiplier()
    {
        return 1f + (maxUpgrades * 0.10f);
    }

    // --- FONCTIONS POUR LIRE LES STATS DANS L'UI ---
    public int GetCurrentHP() => Mathf.RoundToInt(actualBonusHP * GetUpgradeMultiplier());
    public int GetCurrentPhysique() => Mathf.RoundToInt(actualBonusPhysique * GetUpgradeMultiplier());
    public int GetCurrentMagie() => Mathf.RoundToInt(actualBonusMagie * GetUpgradeMultiplier());
    public int GetCurrentMainStat() => Mathf.RoundToInt(actualMainStat * GetUpgradeMultiplier());

    public bool TryUpgrade()
    {
        if (currentUpgrades < maxUpgrades)
        {
            currentUpgrades++;
            return true;
        }
        return false;
    }
}