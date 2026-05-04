using UnityEngine;
using UnityEditor;
using System.IO;

public class ArmorDatabaseGenerator
{
    private const string FolderPath = "Assets/Items/Armors";

    [MenuItem("RPG/Générer Armures du Désert")]
    public static void GenerateDatabase()
    {
        // Vérifier et créer le dossier si nécessaire
        if (!AssetDatabase.IsValidFolder(FolderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Items"))
            {
                AssetDatabase.CreateFolder("Assets", "Items");
            }
            AssetDatabase.CreateFolder("Assets/Items", "Armors");
        }

        // --- Rareté : COMMUN ---
        // Tank (Bandelettes)
        CreateArmor("Bandelettes Épaisses", ItemType.Casque, ItemRarity.Commun, 10, 0, 0, "Bandelettes", "Tank");
        CreateArmor("Torse Embaumé", ItemType.Plastron, ItemRarity.Commun, 20, 0, 0, "Bandelettes", "Tank");
        CreateArmor("Jambes Embaumées", ItemType.Pantalon, ItemRarity.Commun, 10, 0, 0, "Bandelettes", "Tank");
        // Warrior (CuirSeche)
        CreateArmor("Chèche en Cuir Séché", ItemType.Casque, ItemRarity.Commun, 6, 3, 0, "CuirSeche", "Warrior");
        CreateArmor("Harnais en Cuir Séché", ItemType.Plastron, ItemRarity.Commun, 12, 6, 0, "CuirSeche", "Warrior");
        CreateArmor("Bottes en Cuir Séché", ItemType.Pantalon, ItemRarity.Commun, 6, 3, 0, "CuirSeche", "Warrior");
        // Mage (Lin)
        CreateArmor("Capuche en Lin", ItemType.Casque, ItemRarity.Commun, 4, 0, 4, "Lin", "Mage");
        CreateArmor("Robe Poussiéreuse", ItemType.Plastron, ItemRarity.Commun, 8, 0, 8, "Lin", "Mage");
        CreateArmor("Sandales Usées", ItemType.Pantalon, ItemRarity.Commun, 4, 0, 4, "Lin", "Mage");

        // --- Rareté : RARE ---
        // Tank (Bronze)
        CreateArmor("Masque de Bronze", ItemType.Casque, ItemRarity.Rare, 30, 0, 0, "Bronze", "Tank");
        CreateArmor("Cuirasse de Bronze", ItemType.Plastron, ItemRarity.Rare, 60, 0, 0, "Bronze", "Tank");
        CreateArmor("Plaques de Bronze", ItemType.Pantalon, ItemRarity.Rare, 30, 0, 0, "Bronze", "Tank");
        // Warrior (Pillard)
        CreateArmor("Masque du Pillard", ItemType.Casque, ItemRarity.Rare, 18, 10, 0, "Pillard", "Warrior");
        CreateArmor("Manteau du Pillard", ItemType.Plastron, ItemRarity.Rare, 36, 20, 0, "Pillard", "Warrior");
        CreateArmor("Grèves du Pillard", ItemType.Pantalon, ItemRarity.Rare, 18, 10, 0, "Pillard", "Warrior");
        // Mage (Cultiste)
        CreateArmor("Voile du Cultiste", ItemType.Casque, ItemRarity.Rare, 12, 0, 14, "Cultiste", "Mage");
        CreateArmor("Habit du Cultiste", ItemType.Plastron, ItemRarity.Rare, 24, 0, 28, "Cultiste", "Mage");
        CreateArmor("Chausses du Cultiste", ItemType.Pantalon, ItemRarity.Rare, 12, 0, 14, "Cultiste", "Mage");

        // --- Rareté : EPIC ---
        // Tank (Gardien)
        CreateArmor("Heaume du Gardien", ItemType.Casque, ItemRarity.Epic, 80, 0, 0, "Gardien", "Tank");
        CreateArmor("Armure de Pierre", ItemType.Plastron, ItemRarity.Epic, 160, 0, 0, "Gardien", "Tank");
        CreateArmor("Piliers du Gardien", ItemType.Pantalon, ItemRarity.Epic, 80, 0, 0, "Gardien", "Tank");
        // Warrior (Anubis)
        CreateArmor("Masque d'Anubis", ItemType.Casque, ItemRarity.Epic, 50, 25, 0, "Anubis", "Warrior");
        CreateArmor("Plastron d'Anubis", ItemType.Plastron, ItemRarity.Epic, 100, 50, 0, "Anubis", "Warrior");
        CreateArmor("Marche d'Anubis", ItemType.Pantalon, ItemRarity.Epic, 50, 25, 0, "Anubis", "Warrior");
        // Mage (Solaire)
        CreateArmor("Coiffe Solaire", ItemType.Casque, ItemRarity.Epic, 30, 0, 35, "Solaire", "Mage");
        CreateArmor("Manteau Solaire", ItemType.Plastron, ItemRarity.Epic, 60, 0, 70, "Solaire", "Mage");
        CreateArmor("Traces Solaires", ItemType.Pantalon, ItemRarity.Epic, 30, 0, 35, "Solaire", "Mage");

        // --- Rareté : LEGENDAIRE ---
        // Tank (Scarabee)
        CreateArmor("Heaume du Scarabée d'Or", ItemType.Casque, ItemRarity.Legendaire, 200, 0, 0, "Scarabee", "Tank");
        CreateArmor("Carapace du Scarabée", ItemType.Plastron, ItemRarity.Legendaire, 400, 0, 0, "Scarabee", "Tank");
        CreateArmor("Grèves du Scarabée", ItemType.Pantalon, ItemRarity.Legendaire, 200, 0, 0, "Scarabee", "Tank");
        // Warrior (Seigneur)
        CreateArmor("Couronne des Dunes", ItemType.Casque, ItemRarity.Legendaire, 120, 60, 0, "Seigneur", "Warrior");
        CreateArmor("Manteau des Dunes", ItemType.Plastron, ItemRarity.Legendaire, 240, 120, 0, "Seigneur", "Warrior");
        CreateArmor("Foulée des Dunes", ItemType.Pantalon, ItemRarity.Legendaire, 120, 60, 0, "Seigneur", "Warrior");
        // Mage (Vizir)
        CreateArmor("Halo du Vizir Immortel", ItemType.Casque, ItemRarity.Legendaire, 80, 0, 90, "Vizir", "Mage");
        CreateArmor("Linceul du Vizir", ItemType.Plastron, ItemRarity.Legendaire, 160, 0, 180, "Vizir", "Mage");
        CreateArmor("Lévitation du Vizir", ItemType.Pantalon, ItemRarity.Legendaire, 80, 0, 90, "Vizir", "Mage");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Base de données de 36 armures générée avec succès (avec indicateurs de classe) !");
    }

    private static void CreateArmor(string name, ItemType type, ItemRarity rarity, int hp, int phys, int mag, string setSuffix, string className)
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.itemName = $"[{className}] {name}";
        item.type = type;
        item.rarity = rarity;
        item.scalingType = StatScaling.Aucun;
        item.mainStatValue = 0;
        item.bonusHP = hp;
        item.bonusPhysique = phys;
        item.bonusMagie = mag;

        // Règle de nommage : [Type]_[Rareté]_[Classe]_[Nom_Sans_Espaces].asset
        string fileName = $"{type}_{rarity}_{className}_{setSuffix}.asset";
        string path = Path.Combine(FolderPath, fileName);

        AssetDatabase.CreateAsset(item, path);
    }
}
