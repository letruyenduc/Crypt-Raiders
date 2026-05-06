using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    [Header("Item Databases")]
    public List<ItemData> allAvailableItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // Optionnel : Charger tous les items automatiquement depuis Resources
        // allAvailableItems.AddRange(Resources.LoadAll<ItemData>("Items"));
    }

    [Header("Rarity Weights (0-100)")]
    public int weightCommon = 60;
    public int weightRare = 25;
    public int weightEpic = 10;
    public int weightLegendary = 5;

    public List<ItemInstance> GenerateRewards(int count)
    {
        List<ItemInstance> rewards = new List<ItemInstance>();
        
        // Nettoyage de sécurité au cas où des cases vides soient dans l'inspecteur
        allAvailableItems.RemoveAll(x => x == null);

        if (allAvailableItems.Count == 0)
        {
            Debug.LogWarning("LootManager: Aucune donnée d'item disponible !");
            return rewards;
        }

        for (int i = 0; i < count; i++)
        {
            ItemRarity rolledRarity = RollRarity();
            
            // Trouver tous les items qui correspondent à cette rareté (avec check null supplémentaire)
            List<ItemData> matchingItems = allAvailableItems.FindAll(x => x != null && x.rarity == rolledRarity);
            
            // Si aucun item de cette rareté n'existe, on prend n'importe lequel par sécurité
            if (matchingItems.Count == 0)
            {
                matchingItems = allAvailableItems;
            }

            ItemData randomTemplate = matchingItems[Random.Range(0, matchingItems.Count)];
            rewards.Add(new ItemInstance(randomTemplate));
        }

        return rewards;
    }

    private ItemRarity RollRarity()
    {
        int totalWeight = weightCommon + weightRare + weightEpic + weightLegendary;
        int roll = Random.Range(0, totalWeight);

        if (roll < weightCommon) return ItemRarity.Commun;
        if (roll < weightCommon + weightRare) return ItemRarity.Rare;
        if (roll < weightCommon + weightRare + weightEpic) return ItemRarity.Epic;
        return ItemRarity.Legendaire;
    }

    public void GiveLootToPlayer(ItemInstance item)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.backpack.Add(item);
            InventoryManager.Instance.OnInventoryChanged?.Invoke();
            Debug.Log($"Loot ajouté : {item.template.itemName}");
        }
    }
}
