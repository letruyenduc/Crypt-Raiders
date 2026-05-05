using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance { get; private set; }

    [Header("Spell Slots")]
    public ItemInstance slotA;
    public ItemInstance slotE;

    private float nextCastTimeA = 0f;
    private float nextCastTimeE = 0f;
    public float globalCooldown = 1.0f; 
    public LayerMask enemyLayers; 
    public LayerMask wallLayers; // Pour que les projectiles sachent où s'arrêter

    private int currentBonusMagie = 0;
    private int currentBonusPhysique = 0;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // On cherche le joueur dans la scène au démarrage
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    public void UpdateSpellStats(int bonusPhysique, int bonusMagie)
    {
        currentBonusPhysique = bonusPhysique;
        currentBonusMagie = bonusMagie;
    }

    private void Update()
    {
        // Si on n'a pas trouvé le joueur, on réessaie (utile si spawn dynamique)
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            CastSpell(slotA, ref nextCastTimeA);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CastSpell(slotE, ref nextCastTimeE);
        }
    }
    public void EquipSpell(ItemInstance spell)
    {
        if (spell == null) return;

        // Remplir le premier slot vide, sinon remplace le A par défaut
        if (slotA == null) EquipSpellToSlot(spell, "A");
        else if (slotE == null) EquipSpellToSlot(spell, "E");
        else EquipSpellToSlot(spell, "A");
    }

    public void EquipSpellToSlot(ItemInstance spell, string slotTag)
    {
        if (spell == null) return;

        // 1. On l'enlève de là où il était (sac ou autre slot)
        if (slotA == spell) slotA = null;
        if (slotE == spell) slotE = null;
        InventoryManager.Instance.backpack.Remove(spell);

        // 2. On l'équipe dans le slot choisi
        ItemInstance oldItem = null;
        if (slotTag == "A")
        {
            oldItem = slotA;
            slotA = spell;
        }
        else if (slotTag == "E")
        {
            oldItem = slotE;
            slotE = spell;
        }

        // 3. Si un objet était déjà là, on le renvoie dans le sac
        if (oldItem != null)
        {
            InventoryManager.Instance.backpack.Add(oldItem);
        }

        InventoryManager.Instance.OnInventoryChangedNotify();
    }

// Ajout d'une fonction pour déséquiper manuellement
public void UnequipSpell(ItemInstance spell)
{
    if (spell == null) return;

    if (slotA == spell) slotA = null;
    else if (slotE == spell) slotE = null;
    else return;

    InventoryManager.Instance.backpack.Add(spell);
    InventoryManager.Instance.OnInventoryChanged?.Invoke();
}

    private void CastSpell(ItemInstance spell, ref float cooldownTimer)
    {
        if (spell == null || spell.template.spellPrefab == null || playerTransform == null) return;
        if (Time.time < cooldownTimer) return;

        // 1. Calcul de la direction (vers la souris depuis le joueur)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector2 direction = (mouseWorldPos - playerTransform.position).normalized;

        // 2. Instanciation au niveau du joueur
        // Pour un sort de zone au corps à corps, on peut le décaler un peu devant le joueur
        float spawnOffset = (spell.template.type == ItemType.Sort && !spell.template.spellPrefab.GetComponent<Projectile>()) ? 1.5f : 0f;
        Vector3 spawnPos = playerTransform.position + (Vector3)(direction * spawnOffset);
        
        GameObject projObj = Instantiate(spell.template.spellPrefab, spawnPos, Quaternion.identity);
        
        // --- NOUVEAUTÉ : CALCUL DU BONUS SELON LE SCALING ---
        int appliedBonus = 0;
        if (spell.template.scalingType == StatScaling.Physique) appliedBonus = currentBonusPhysique;
        else if (spell.template.scalingType == StatScaling.Magique) appliedBonus = currentBonusMagie;

        int totalDamage = spell.GetCurrentMainStat() + appliedBonus;

        // 3. Configuration selon le type de script sur le prefab
        Projectile proj = projObj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Setup(totalDamage, 10f, spell.template.lifetime, direction, enemyLayers, wallLayers);
        }

        DelayedAoE aoe = projObj.GetComponent<DelayedAoE>();
        if (aoe != null)
        {
            aoe.Setup(totalDamage, spell.template.aoeDelay, spell.template.aoeRadius, enemyLayers);
        }

        cooldownTimer = Time.time + spell.template.cooldown;
    }
}
