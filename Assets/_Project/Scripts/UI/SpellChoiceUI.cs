using UnityEngine;

public class SpellChoiceUI : MonoBehaviour
{
    public static SpellChoiceUI Instance { get; private set; }

    public GameObject choicePanel;
    private ItemInstance pendingSpell;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        choicePanel.SetActive(false);
    }

    public void OpenChoice(ItemInstance spell)
    {
        pendingSpell = spell;
        choicePanel.SetActive(true);
    }

    public void OnSelectSlotA()
    {
        SpellManager.Instance.EquipSpellToSlot(pendingSpell, "A");
        CloseChoice();
    }

    public void OnSelectSlotE()
    {
        SpellManager.Instance.EquipSpellToSlot(pendingSpell, "E");
        CloseChoice();
    }

    public void CloseChoice()
    {
        pendingSpell = null;
        choicePanel.SetActive(false);
    }
}
