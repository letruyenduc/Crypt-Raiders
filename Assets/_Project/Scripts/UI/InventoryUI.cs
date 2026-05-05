using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Panels")]
    public GameObject inventoryPanel;
    public Transform backpackGrid;
    
    [Header("Prefabs")]
    public GameObject slotPrefab;

    [Header("Equipment Slots")]
    public InventorySlotUI headSlot;
    public InventorySlotUI chestSlot;
    public InventorySlotUI legsSlot;
    public InventorySlotUI weaponSlot; // Un seul slot pour les deux types
    public InventorySlotUI spellASlot;
    public InventorySlotUI spellESlot;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        inventoryPanel.SetActive(false);
        RefreshUI();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf) RefreshUI();
    }

    public void RefreshUI()
    {
        // 1. Vider proprement le sac à dos visuel
        // On détache les enfants avant de les détruire pour que la grille ne les compte plus
        List<GameObject> toDestroy = new List<GameObject>();
        foreach (Transform child in backpackGrid) toDestroy.Add(child.gameObject);
        
        foreach (GameObject child in toDestroy)
        {
            child.transform.SetParent(null);
            Destroy(child);
        }

        // 2. Remplir le sac à dos avec sécurité
        foreach (ItemInstance item in InventoryManager.Instance.backpack)
        {
            if (item == null || item.template == null) continue; // Sécurité anti-slot vide

            GameObject newSlot = Instantiate(slotPrefab, backpackGrid);
            newSlot.GetComponent<InventorySlotUI>().SetItem(item);
        }

        // 3. Mettre à jour les slots d'équipement
        UpdateEquipmentSlot(headSlot, ItemType.Casque);
        UpdateEquipmentSlot(chestSlot, ItemType.Plastron);
        UpdateEquipmentSlot(legsSlot, ItemType.Pantalon);
        UpdateEquipmentSlot(weaponSlot, ItemType.Arme_Melee); // Utilise la clé commune
        
        // Spells
        if (spellASlot != null) spellASlot.SetItem(SpellManager.Instance.slotA);
        if (spellESlot != null) spellESlot.SetItem(SpellManager.Instance.slotE);
    }

    private void UpdateEquipmentSlot(InventorySlotUI slot, ItemType type)
    {
        if (slot == null) return;
        
        if (InventoryManager.Instance.equipment.ContainsKey(type))
        {
            slot.SetItem(InventoryManager.Instance.equipment[type]);
        }
        else
        {
            slot.SetItem(null);
        }
    }
}
