using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Requis pour la détection de souris

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public Image rarityFrame; 
    public TextMeshProUGUI countText; 
    
    private ItemInstance currentItem;

    private void Awake()
    {
        // Si on a oublié de glisser l'image dans l'inspecteur, on essaie de la trouver
        if (iconImage == null) iconImage = GetComponent<Image>();
        if (iconImage == null) iconImage = GetComponentInChildren<Image>();
    }

    public void SetItem(ItemInstance item)
    {
        currentItem = item;
        
        if (item == null || item.template == null)
        {
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
            if (countText != null) countText.text = "";
            return;
        }

        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = item.template.itemIcon;
        }
        
        if (rarityFrame != null)
        {
            rarityFrame.color = GetRarityColor(item.template.rarity);
        }
    }

    public void OnSlotClicked()
    {
        if (currentItem == null) return;

        // --- NOUVEAUTÉ : INTERACTION AVEC LES PNJ DU LOBBY ---
        if (LobbyNPCManager.Instance != null)
        {
            if (LobbyNPCManager.Instance.isMerchantOpen)
            {
                // Ne vendre que les objets dans le sac à dos (pas l'équipement)
                if (transform.parent == InventoryUI.Instance.backpackGrid)
                {
                    LobbyNPCManager.Instance.SellItem(currentItem);
                }
                return; // On arrête là pour ne pas équiper l'objet par erreur
            }
            else if (LobbyNPCManager.Instance.isBlacksmithOpen)
            {
                LobbyNPCManager.Instance.SelectItemForBlacksmith(currentItem);
                return;
            }
        }

        // On vérifie si ce slot appartient au sac à dos ou à l'équipement
        // Si le parent du slot n'est pas la grille du sac à dos, c'est que c'est un slot d'équipement
        bool isEquipmentSlot = transform.parent != InventoryUI.Instance.backpackGrid;

        if (isEquipmentSlot)
        {
            // Déséquipement
            if (currentItem.template.type == ItemType.Sort)
            {
                SpellManager.Instance.UnequipSpell(currentItem);
            }
            else
            {
                // Pour les armures/armes, on demande à l'InventoryManager de déséquiper ce type
                // Note: On utilise le type du template, le manager gèrera la clé commune pour l'arme
                InventoryManager.Instance.UnequipItem(currentItem.template.type);
            }
        }
        else
        {
            // Équipement classique
            if (currentItem.template.type == ItemType.Sort)
            {
                // Pour les sorts, on demande le choix du slot
                SpellChoiceUI.Instance.OpenChoice(currentItem);
            }
            else
            {
                InventoryManager.Instance.EquipItem(currentItem);
            }
        }

        // Cacher le tooltip après l'action
        if (TooltipUI.Instance != null) TooltipUI.Instance.HideTooltip();
    }

    // --- TOOLTIP LOGIC ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && TooltipUI.Instance != null)
        {
            TooltipUI.Instance.ShowTooltip(currentItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipUI.Instance != null)
        {
            TooltipUI.Instance.HideTooltip();
        }
    }

    private void OnDisable() 
    {
        // Sécurité : cacher le tooltip si le slot est désactivé
        if (TooltipUI.Instance != null) TooltipUI.Instance.HideTooltip();
    }

    private Color GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Commun: return Color.white;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return new Color(0.6f, 0f, 1f); // Violet
            case ItemRarity.Legendaire: return new Color(1f, 0.5f, 0f); // Orange
            default: return Color.white;
        }
    }
}
