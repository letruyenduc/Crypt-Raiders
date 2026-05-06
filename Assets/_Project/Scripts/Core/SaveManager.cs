using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ItemSaveData
{
    public string assetName;
    public int mainStat;
    public int hp;
    public int phys;
    public int mag;
    public int currentUpgrades;
    public int maxUpgrades;

    public ItemSaveData(ItemInstance item)
    {
        assetName = item.template.name;
        mainStat = item.actualMainStat;
        hp = item.actualBonusHP;
        phys = item.actualBonusPhysique;
        mag = item.actualBonusMagie;
        currentUpgrades = item.currentUpgrades;
        maxUpgrades = item.maxUpgrades;
    }
}

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> backpack = new List<ItemSaveData>();
    public List<ItemType> equipSlots = new List<ItemType>();
    public List<ItemSaveData> equipItems = new List<ItemSaveData>();
    public ItemSaveData slotA;
    public ItemSaveData slotE;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public ItemDatabase itemDatabase;
    private string savePath;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
    }

    private void Start()
    {
        // On charge la partie au début de chaque scène
        LoadGame();
    }

    public void SaveGame()
    {
        // 1. Sauvegarde des stats simples (PlayerPrefs)
        if (LevelManager.Instance != null)
        {
            PlayerPrefs.SetInt("PlayerLevel", LevelManager.Instance.currentLevel);
            PlayerPrefs.SetFloat("PlayerXP", LevelManager.Instance.currentXP);
            PlayerPrefs.SetFloat("PlayerXPToNext", LevelManager.Instance.xpToNextLevel);
            PlayerPrefs.SetInt("SkillPoints", LevelManager.Instance.skillPoints);
            PlayerPrefs.SetInt("InvestedHP", LevelManager.Instance.investedHP);
            PlayerPrefs.SetInt("InvestedPhys", LevelManager.Instance.investedPhysique);
            PlayerPrefs.SetInt("InvestedMagie", LevelManager.Instance.investedMagie);
        }

        if (CurrencyManager.Instance != null)
        {
            PlayerPrefs.SetInt("PlayerGold", CurrencyManager.Instance.gold);
        }

        PlayerPrefs.Save();

        // 2. Sauvegarde de l'inventaire (JSON)
        if (InventoryManager.Instance != null && SpellManager.Instance != null)
        {
            InventorySaveData data = new InventorySaveData();

            foreach (var item in InventoryManager.Instance.backpack)
            {
                if (item != null && item.template != null)
                    data.backpack.Add(new ItemSaveData(item));
            }

            foreach (var pair in InventoryManager.Instance.equipment)
            {
                if (pair.Value != null && pair.Value.template != null)
                {
                    data.equipSlots.Add(pair.Key);
                    data.equipItems.Add(new ItemSaveData(pair.Value));
                }
            }

            if (SpellManager.Instance.slotA != null && SpellManager.Instance.slotA.template != null) 
                data.slotA = new ItemSaveData(SpellManager.Instance.slotA);
                
            if (SpellManager.Instance.slotE != null && SpellManager.Instance.slotE.template != null) 
                data.slotE = new ItemSaveData(SpellManager.Instance.slotE);

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Jeu sauvegardé dans : " + savePath);
        }
    }

    public void LoadGame()
    {
        // 1. Chargement des stats
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
            LevelManager.Instance.currentXP = PlayerPrefs.GetFloat("PlayerXP", 0);
            LevelManager.Instance.xpToNextLevel = PlayerPrefs.GetFloat("PlayerXPToNext", 100);
            LevelManager.Instance.skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);
            LevelManager.Instance.investedHP = PlayerPrefs.GetInt("InvestedHP", 0);
            LevelManager.Instance.investedPhysique = PlayerPrefs.GetInt("InvestedPhys", 0);
            LevelManager.Instance.investedMagie = PlayerPrefs.GetInt("InvestedMagie", 0);
            
            LevelManager.Instance.OnXPChanged?.Invoke();
            LevelManager.Instance.OnLevelUp?.Invoke();
            LevelManager.Instance.OnStatsChanged?.Invoke();
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.gold = PlayerPrefs.GetInt("PlayerGold", 0);
            CurrencyManager.Instance.OnGoldChanged?.Invoke();
        }

        // 2. Chargement de l'inventaire
        if (File.Exists(savePath) && itemDatabase != null)
        {
            string json = File.ReadAllText(savePath);
            InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

            if (InventoryManager.Instance != null && SpellManager.Instance != null)
            {
                InventoryManager.Instance.backpack.Clear();
                InventoryManager.Instance.equipment.Clear();
                // Réinitialiser les slots d'équipement
                InventoryManager.Instance.equipment[ItemType.Casque] = null;
                InventoryManager.Instance.equipment[ItemType.Plastron] = null;
                InventoryManager.Instance.equipment[ItemType.Pantalon] = null;
                InventoryManager.Instance.equipment[ItemType.Arme_Melee] = null;

                foreach (var s in data.backpack)
                {
                    ItemData template = itemDatabase.GetItemByName(s.assetName);
                    if (template != null)
                        InventoryManager.Instance.backpack.Add(new ItemInstance(template, s.mainStat, s.hp, s.phys, s.mag, s.currentUpgrades, s.maxUpgrades));
                }

                for (int i = 0; i < data.equipSlots.Count; i++)
                {
                    ItemData template = itemDatabase.GetItemByName(data.equipItems[i].assetName);
                    if (template != null)
                        InventoryManager.Instance.equipment[data.equipSlots[i]] = new ItemInstance(template, data.equipItems[i].mainStat, data.equipItems[i].hp, data.equipItems[i].phys, data.equipItems[i].mag, data.equipItems[i].currentUpgrades, data.equipItems[i].maxUpgrades);
                }

                SpellManager.Instance.slotA = null;
                SpellManager.Instance.slotE = null;

                if (data.slotA != null)
                {
                    ItemData template = itemDatabase.GetItemByName(data.slotA.assetName);
                    if (template != null) SpellManager.Instance.slotA = new ItemInstance(template, data.slotA.mainStat, data.slotA.hp, data.slotA.phys, data.slotA.mag, data.slotA.currentUpgrades, data.slotA.maxUpgrades);
                }
                if (data.slotE != null)
                {
                    ItemData template = itemDatabase.GetItemByName(data.slotE.assetName);
                    if (template != null) SpellManager.Instance.slotE = new ItemInstance(template, data.slotE.mainStat, data.slotE.hp, data.slotE.phys, data.slotE.mag, data.slotE.currentUpgrades, data.slotE.maxUpgrades);
                }

                InventoryManager.Instance.UpdatePlayerStats();
                InventoryManager.Instance.OnInventoryChangedNotify();
            }
        }
    }

    public void ClearSave()
    {
        // 1. Supprime les PlayerPrefs
        PlayerPrefs.DeleteAll();
        
        // 2. Supprime le fichier JSON d'inventaire
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        
        Debug.Log("SAVE SYSTEM: Sauvegarde effacée (Permadeath active) !");
    }
}
