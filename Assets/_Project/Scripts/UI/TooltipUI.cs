using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI descriptionText;
    public Image rarityBg;

    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
        HideTooltip();
    }

    void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;

            // Décalage pour ne pas être pile sous le curseur
            float offsetX = 15f;
            float offsetY = 15f;

            // Inverser le pivot selon le côté de l'écran pour rester visible
            float pivotX = mousePos.x > Screen.width / 2 ? 1f : 0f;
            float pivotY = mousePos.y > Screen.height / 2 ? 1f : 0f;

            // Appliquer un petit décalage supplémentaire pour ne pas coller
            float posX = mousePos.x + (pivotX == 1f ? -offsetX : offsetX);
            float posY = mousePos.y + (pivotY == 1f ? -offsetY : offsetY);

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = new Vector3(posX, posY, 0);
        }
    }

    public void ShowTooltip(ItemInstance item)
    {
        if (item == null || item.template == null) return;

        if (tooltipPanel != null) tooltipPanel.SetActive(true);
        
        if (itemNameText != null)
        {
            itemNameText.text = item.template.itemName;
            itemNameText.color = GetRarityColor(item.template.rarity);
        }
        
        if (itemTypeText != null) itemTypeText.text = item.template.type.ToString().Replace("_", " ");
        if (descriptionText != null) descriptionText.text = item.template.description;

        // Construction des stats
        string stats = "";
        
        if (item.actualMainStat > 0)
        {
            string label = (item.template.type == ItemType.Sort || item.template.type == ItemType.Arme_Magique) ? "Dégâts Magiques" : "Dégâts";
            stats += $"{label}: {item.GetCurrentMainStat()}\n";
        }

        if (item.actualBonusHP > 0) stats += $"PV Max: +{item.GetCurrentHP()}\n";
        if (item.actualBonusPhysique > 0) stats += $"Force: +{item.GetCurrentPhysique()}\n";
        if (item.actualBonusMagie > 0) stats += $"Magie: +{item.GetCurrentMagie()}\n";

        if (item.maxUpgrades > 0)
        {
            stats += $"\nAmélioration: {item.currentUpgrades}/{item.maxUpgrades}";
        }

        // --- NOUVEAUTÉ : PRIX DE VENTE ---
        if (LobbyNPCManager.Instance != null)
        {
            int price = LobbyNPCManager.Instance.GetSellPrice(item);
            stats += $"\n\n<color=yellow>Valeur : {price} Or</color>";
        }

        if (statsText != null) statsText.text = stats;

        // Ajuster la couleur de fond selon rareté
        if (rarityBg != null)
        {
            Color c = GetRarityColor(item.template.rarity);
            c.a = 0.2f; // Très transparent
            rarityBg.color = c;
        }
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private Color GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Commun: return Color.white;
            case ItemRarity.Rare: return new Color(0.2f, 0.6f, 1f); // Bleu clair
            case ItemRarity.Epic: return new Color(0.7f, 0.2f, 1f); // Violet
            case ItemRarity.Legendaire: return new Color(1f, 0.6f, 0f); // Orange
            default: return Color.white;
        }
    }
}
