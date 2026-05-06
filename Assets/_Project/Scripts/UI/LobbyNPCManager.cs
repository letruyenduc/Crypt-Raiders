using UnityEngine;
using TMPro;

public class LobbyNPCManager : MonoBehaviour
{
    public static LobbyNPCManager Instance { get; private set; }

    [Header("Merchant UI")]
    public GameObject merchantPanel;
    public TextMeshProUGUI merchantActionText;
    public bool isMerchantOpen = false;

    [Header("Blacksmith UI")]
    public GameObject blacksmithPanel;
    public UnityEngine.UI.Image blacksmithItemIcon; // Nouveau
    public TextMeshProUGUI blacksmithItemNameText;
    public TextMeshProUGUI blacksmithCostText;
    public TextMeshProUGUI blacksmithStatsText;
    public bool isBlacksmithOpen = false;
    
    private ItemInstance currentBlacksmithItem;

    [Header("General UI Configuration")]
    public Transform mainInventoryCanvasParent; // Là où l'inventaire habite normalement
    public Transform merchantInventoryAnchor;   // Zone de droite du marchand
    public Transform blacksmithInventoryAnchor; // Zone de droite du forgeron

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        CloseAllNPCs();
    }

    public void OpenMerchant()
    {
        isMerchantOpen = true;
        isBlacksmithOpen = false;
        
        if (merchantPanel != null) merchantPanel.SetActive(true);
        if (blacksmithPanel != null) blacksmithPanel.SetActive(false);
        
        // --- NOUVEAUTÉ : DÉPLACER LA GRILLE SEULE ---
        if (InventoryUI.Instance != null && merchantInventoryAnchor != null)
        {
            InventoryUI.Instance.SetBackpackParent(merchantInventoryAnchor);
        }
    }

    public void OpenBlacksmith()
    {
        isBlacksmithOpen = true;
        isMerchantOpen = false;
        currentBlacksmithItem = null;

        if (merchantPanel != null) merchantPanel.SetActive(false);
        if (blacksmithPanel != null) blacksmithPanel.SetActive(true);

        // --- NOUVEAUTÉ : DÉPLACER LA GRILLE SEULE ---
        if (InventoryUI.Instance != null && blacksmithInventoryAnchor != null)
        {
            InventoryUI.Instance.SetBackpackParent(blacksmithInventoryAnchor);
        }
        
        RefreshBlacksmithUI();
    }

    public void CloseAllNPCs()
    {
        isMerchantOpen = false;
        isBlacksmithOpen = false;
        currentBlacksmithItem = null;
        
        if (merchantPanel != null) merchantPanel.SetActive(false);
        if (blacksmithPanel != null) blacksmithPanel.SetActive(false);

        // --- NOUVEAUTÉ : REMETTRE LA GRILLE À SA PLACE ---
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RestoreBackpackParent();
        }
    }

    // --- LOGIQUE DU MARCHAND ---
    public int GetSellPrice(ItemInstance item)
    {
        int basePrice = 10;
        switch (item.template.rarity)
        {
            case ItemRarity.Commun: basePrice = 10; break;
            case ItemRarity.Rare: basePrice = 50; break;
            case ItemRarity.Epic: basePrice = 200; break;
            case ItemRarity.Legendaire: basePrice = 1000; break;
        }
        // L'item vaut plus cher s'il a été amélioré
        return basePrice + (item.currentUpgrades * basePrice / 2);
    }

    public void SellItem(ItemInstance item)
    {
        if (!isMerchantOpen) return;

        int price = GetSellPrice(item);
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddGold(price);
        }

        InventoryManager.Instance.backpack.Remove(item);
        InventoryUI.Instance.RefreshUI();
        
        Debug.Log($"Vendu {item.template.itemName} pour {price} Or !");
    }

    // --- LOGIQUE DU FORGERON ---
    public void SelectItemForBlacksmith(ItemInstance item)
    {
        if (!isBlacksmithOpen) return;
        currentBlacksmithItem = item;
        RefreshBlacksmithUI();
    }

    private int GetUpgradeCost(ItemInstance item)
    {
        int baseCost = 20;
        switch (item.template.rarity)
        {
            case ItemRarity.Commun: baseCost = 20; break;
            case ItemRarity.Rare: baseCost = 100; break;
            case ItemRarity.Epic: baseCost = 400; break;
            case ItemRarity.Legendaire: baseCost = 2000; break;
        }
        return baseCost * (item.currentUpgrades + 1);
    }

    private void RefreshBlacksmithUI()
    {
        if (currentBlacksmithItem == null)
        {
            if (blacksmithItemIcon != null) blacksmithItemIcon.enabled = false;
            if (blacksmithItemNameText != null) blacksmithItemNameText.text = "Sélectionnez un objet dans votre inventaire";
            if (blacksmithCostText != null) blacksmithCostText.text = "Coût : -- G";
            if (blacksmithStatsText != null) blacksmithStatsText.text = "";
            return;
        }

        if (blacksmithItemIcon != null)
        {
            blacksmithItemIcon.enabled = true;
            blacksmithItemIcon.sprite = currentBlacksmithItem.template.itemIcon;
        }

        if (blacksmithItemNameText != null) 
            blacksmithItemNameText.text = $"{currentBlacksmithItem.template.itemName} <color=yellow>(+{currentBlacksmithItem.currentUpgrades})</color> [{currentBlacksmithItem.currentUpgrades}/{currentBlacksmithItem.maxUpgrades}]";

        if (currentBlacksmithItem.currentUpgrades >= currentBlacksmithItem.maxUpgrades)
        {
            if (blacksmithCostText != null) blacksmithCostText.text = "<color=red>AMÉLIORATION MAXIMALE ATTEINTE</color>";
        }
        else
        {
            int cost = GetUpgradeCost(currentBlacksmithItem);
            if (blacksmithCostText != null) blacksmithCostText.text = $"Coût : <color=yellow>{cost} G</color>";
        }
        
        if (blacksmithStatsText != null)
        {
            // Calcul des gains futurs (10% par niveau selon la formule de ItemInstance)
            float multiplier = currentBlacksmithItem.GetUpgradeMultiplier();
            float nextMultiplier = 1.0f + ((currentBlacksmithItem.currentUpgrades + 1) * 0.1f);
            
            int curMain = currentBlacksmithItem.GetCurrentMainStat();
            int nextMain = Mathf.RoundToInt(currentBlacksmithItem.actualMainStat * nextMultiplier);
            int diffMain = nextMain - curMain;

            int curHP = currentBlacksmithItem.GetCurrentHP();
            int nextHP = Mathf.RoundToInt(currentBlacksmithItem.actualBonusHP * nextMultiplier);
            int diffHP = nextHP - curHP;

            int curPhys = currentBlacksmithItem.GetCurrentPhysique();
            int nextPhys = Mathf.RoundToInt(currentBlacksmithItem.actualBonusPhysique * nextMultiplier);
            int diffPhys = nextPhys - curPhys;

            int curMag = currentBlacksmithItem.GetCurrentMagie();
            int nextMag = Mathf.RoundToInt(currentBlacksmithItem.actualBonusMagie * nextMultiplier);
            int diffMag = nextMag - curMag;

            string stats = "";
            if (curMain > 0) stats += $"Dégâts : {curMain} <color=green>(+{diffMain})</color>\n";
            if (curHP > 0) stats += $"PV : {curHP} <color=green>(+{diffHP})</color>\n";
            if (curPhys > 0) stats += $"Force : {curPhys} <color=green>(+{diffPhys})</color>\n";
            if (curMag > 0) stats += $"Magie : {curMag} <color=green>(+{diffMag})</color>\n";

            blacksmithStatsText.text = stats;
        }
    }

    public void ConfirmUpgrade()
    {
        if (currentBlacksmithItem == null || currentBlacksmithItem.currentUpgrades >= currentBlacksmithItem.maxUpgrades) return;

        int cost = GetUpgradeCost(currentBlacksmithItem);

        if (CurrencyManager.Instance != null && CurrencyManager.Instance.gold >= cost)
        {
            CurrencyManager.Instance.SpendGold(cost);
            currentBlacksmithItem.TryUpgrade();
            RefreshBlacksmithUI();
            
            // Rafraîchir l'inventaire complet pour mettre à jour les icônes/stats
            InventoryUI.Instance.RefreshUI();
            InventoryManager.Instance.UpdatePlayerStats();
            
            Debug.Log($"Objet amélioré au niveau {currentBlacksmithItem.currentUpgrades} !");
        }
        else
        {
            Debug.Log("Pas assez d'or pour améliorer !");
        }
    }
}
