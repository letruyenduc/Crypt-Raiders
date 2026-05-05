using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Settings")]
    public int maxBackpackSlots = 20;

    [Header("Current State")]
    public List<ItemInstance> backpack = new List<ItemInstance>();
    public Dictionary<ItemType, ItemInstance> equipment = new Dictionary<ItemType, ItemInstance>();

    // Event pour notifier l'UI des changements
    public System.Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Initialisation des slots d'équipement vides
        equipment[ItemType.Casque] = null;
        equipment[ItemType.Plastron] = null;
        equipment[ItemType.Pantalon] = null;
        equipment[ItemType.Arme_Melee] = null; // On utilisera un seul de ces types pour représenter l'arme
    }

    public void OnInventoryChangedNotify()
    {
        backpack.RemoveAll(item => item == null);
        OnInventoryChanged?.Invoke();
    }

    public bool AddItem(ItemData data)
    {
        if (backpack.Count >= maxBackpackSlots) return false;

        ItemInstance newInstance = new ItemInstance(data);
        backpack.Add(newInstance);
        OnInventoryChangedNotify();
        return true;
    }

    public void EquipItem(ItemInstance item)
    {
        if (item.template.type == ItemType.Sort)
        {
            SpellManager.Instance.EquipSpell(item);
            return;
        }

        ItemType slotType = item.template.type;

        // --- NOUVEAUTÉ : UN SEUL SLOT POUR LES ARMES ---
        // Si c'est une arme (Mêlée OU Magique), on utilise une clé commune
        bool isWeapon = slotType == ItemType.Arme_Melee || slotType == ItemType.Arme_Magique;
        ItemType targetSlot = isWeapon ? ItemType.Arme_Melee : slotType;

        // Déséquiper l'ancien si présent dans le slot cible
        if (equipment.ContainsKey(targetSlot) && equipment[targetSlot] != null)
        {
            backpack.Add(equipment[targetSlot]);
        }

        // Équiper le nouveau
        equipment[targetSlot] = item;
        backpack.Remove(item);

        OnInventoryChanged?.Invoke();
        UpdatePlayerStats();
    }

    public void UnequipItem(ItemType type)
    {
        // Si on demande de déséquiper une arme magique, on regarde le slot Arme_Melee (le slot commun)
        ItemType targetSlot = (type == ItemType.Arme_Magique) ? ItemType.Arme_Melee : type;

        if (equipment.ContainsKey(targetSlot) && equipment[targetSlot] != null)
        {
            if (backpack.Count < maxBackpackSlots)
            {
                backpack.Add(equipment[targetSlot]);
                equipment[targetSlot] = null;
                OnInventoryChanged?.Invoke();
                UpdatePlayerStats();
            }
        }
    }

    private void UpdatePlayerStats()
    {
        // Calculer les bonus totaux
        int totalHP = 0;
        int totalPhysique = 0;
        int totalMagie = 0;

        foreach (var item in equipment.Values)
        {
            if (item != null)
            {
                totalHP += item.GetCurrentHP();
                totalPhysique += item.GetCurrentPhysique();
                totalMagie += item.GetCurrentMagie();
            }
        }

        // Appliquer au joueur (On devra mettre à jour PlayerHealth et PlayerController)
        var playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.UpdateMaxHPBonus(totalHP); 
        }

        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.UpdateDamageBonus(totalPhysique, totalMagie);
        }

        if (SpellManager.Instance != null)
        {
            SpellManager.Instance.UpdateSpellStats(totalPhysique, totalMagie);
        }
    }
}
