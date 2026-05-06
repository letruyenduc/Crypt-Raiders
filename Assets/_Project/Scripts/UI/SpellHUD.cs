using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellHUD : MonoBehaviour
{
    [Header("Slot A")]
    public Image iconA;
    public Image cooldownOverlayA;
    public TextMeshProUGUI keyTextA;

    [Header("Slot E")]
    public Image iconE;
    public Image cooldownOverlayE;
    public TextMeshProUGUI keyTextE;

    void Update()
    {
        if (SpellManager.Instance == null) return;

        UpdateSlot(SpellManager.Instance.slotA, iconA, cooldownOverlayA, SpellManager.Instance.GetCooldownPctA());
        UpdateSlot(SpellManager.Instance.slotE, iconE, cooldownOverlayE, SpellManager.Instance.GetCooldownPctE());
    }

    private void UpdateSlot(ItemInstance spell, Image icon, Image overlay, float cooldownPct)
    {
        // Sécurité : si les références UI ne sont pas assignées dans l'inspecteur
        if (icon == null || overlay == null) return;

        if (spell == null || spell.template == null)
        {
            icon.enabled = false;
            overlay.fillAmount = 0;
            return;
        }

        icon.enabled = true;
        icon.sprite = spell.template.itemIcon;

        // L'overlay de cooldown (souvent une image noire semi-transparente en mode "Filled")
        overlay.fillAmount = cooldownPct;
    }
}
