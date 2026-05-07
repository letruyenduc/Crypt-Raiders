using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Panels")]
    public GameObject inventoryPanel;
    public Transform backpackGrid;
    private Transform originalGridParent;
    
    // NOUVEAU : Mémoire du RectTransform d'origine
    private Vector2 origAnchorMin;
    private Vector2 origAnchorMax;
    private Vector2 origSizeDelta;
    private Vector2 origAnchoredPos;

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
        // On force toujours l'instance sur l'objet actuel de la scène
        Instance = this;

        if (backpackGrid != null)
        {
            originalGridParent = backpackGrid.parent;
            
            // Mémoriser la forme exacte configurée dans Unity
            RectTransform rt = backpackGrid.GetComponent<RectTransform>();
            if (rt != null)
            {
                origAnchorMin = rt.anchorMin;
                origAnchorMax = rt.anchorMax;
                origSizeDelta = rt.sizeDelta;
                origAnchoredPos = rt.anchoredPosition;
            }
        }
    }

    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        }
        inventoryPanel.SetActive(false);
        RefreshUI();
    }

    private void OnDestroy()
    {
        // TRÈS IMPORTANT : On se désabonne pour ne pas laisser de références fantômes
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf) RefreshUI();
    }

    // --- NOUVEAUTÉ : DÉPLACER SEULEMENT LA GRILLE ---
    public void SetBackpackParent(Transform newParent)
    {
        if (backpackGrid == null || newParent == null) return;
        
        backpackGrid.SetParent(newParent);
        
        // Forcer l'étirement (Stretch) dans le panneau du PNJ
        RectTransform rt = backpackGrid.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        RefreshUI();
    }

    public void RestoreBackpackParent()
    {
        if (backpackGrid == null || originalGridParent == null) return;
        
        backpackGrid.SetParent(originalGridParent);
        
        // Restaurer l'apparence EXACTE d'origine
        RectTransform rt = backpackGrid.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = origAnchorMin;
            rt.anchorMax = origAnchorMax;
            rt.sizeDelta = origSizeDelta;
            rt.anchoredPosition = origAnchoredPos;
        }
    }

    public void RefreshUI()
    {
        if (InventoryManager.Instance == null) return;

        // 1. Vider proprement le sac à dos visuel
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
            if (item == null || item.template == null) continue;

            GameObject newSlot = Instantiate(slotPrefab, backpackGrid);
            newSlot.GetComponent<InventorySlotUI>().SetItem(item);
        }

        // 3. Mettre à jour les slots d'équipement
        UpdateEquipmentSlot(headSlot, ItemType.Casque);
        UpdateEquipmentSlot(chestSlot, ItemType.Plastron);
        UpdateEquipmentSlot(legsSlot, ItemType.Pantalon);
        UpdateEquipmentSlot(weaponSlot, ItemType.Arme_Melee); 
        
        // Spells
        if (spellASlot != null) spellASlot.SetItem(SpellManager.Instance != null ? SpellManager.Instance.slotA : null);
        if (spellESlot != null) spellESlot.SetItem(SpellManager.Instance != null ? SpellManager.Instance.slotE : null);
    }

    private void UpdateEquipmentSlot(InventorySlotUI slot, ItemType type)
    {
        if (slot == null || InventoryManager.Instance == null) return;
        
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
