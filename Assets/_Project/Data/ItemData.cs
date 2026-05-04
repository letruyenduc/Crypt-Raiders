using UnityEngine;

[CreateAssetMenu(fileName = "New_Item", menuName = "RPG/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Informations de Base")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType type;
    public ItemRarity rarity;

    [Header("Statistique Principale (Arme / Sort)")]
    public StatScaling scalingType; 
    public int mainStatValue; // Dégâts de l'arme ou du sort

    [Header("Statistiques de Survie (Armures)")]
    public int bonusHP;       // Points de vie supplémentaires
    
    [Header("Bonus Offensifs d'Armure (Les 'Classes')")]
    public int bonusPhysique; // Augmente les dégâts physiques
    public int bonusMagie;    // Augmente les dégâts magiques

    [TextArea(2, 4)]
    public string description;
}

public enum ItemType { Arme_Melee, Arme_Magique, Casque, Plastron, Pantalon, Sort }
public enum ItemRarity { Commun, Rare, Epic, Legendaire }
public enum StatScaling { Physique, Magique, Aucun }