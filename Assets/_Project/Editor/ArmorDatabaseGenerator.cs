using UnityEngine;
using UnityEditor;
using System.IO;

public class ArmorDatabaseGenerator : EditorWindow
{
    [MenuItem("RPG/Générer Armures")]
    public static void GenerateArmorDatabase()
    {
        string folderPath = "Assets/Items/Armors";

        // S'assurer que le dossier existe
        if (!Directory.Exists(Application.dataPath + "/Items/Armors"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Items/Armors");
            AssetDatabase.Refresh();
        }

        // --- Rareté : COMMUN ---
        // Tank
        CreateArmor("Casque_Commun_Tank", "Bonnet Lourd", ItemType.Casque, ItemRarity.Commun, 10, 0, 0);
        CreateArmor("Plastron_Commun_Tank", "Tunique Épaisse", ItemType.Plastron, ItemRarity.Commun, 20, 0, 0);
        CreateArmor("Pantalon_Commun_Tank", "Pantalon Rembourré", ItemType.Pantalon, ItemRarity.Commun, 10, 0, 0);
        // Warrior
        CreateArmor("Casque_Commun_Guerrier", "Heaume Rouillé", ItemType.Casque, ItemRarity.Commun, 6, 3, 0);
        CreateArmor("Plastron_Commun_Guerrier", "Cuirasse Fendue", ItemType.Plastron, ItemRarity.Commun, 12, 6, 0);
        CreateArmor("Pantalon_Commun_Guerrier", "Jambières Rouillées", ItemType.Pantalon, ItemRarity.Commun, 6, 3, 0);
        // Mage
        CreateArmor("Casque_Commun_Mage", "Chaperon Usé", ItemType.Casque, ItemRarity.Commun, 4, 0, 4);
        CreateArmor("Plastron_Commun_Mage", "Robe Poussiéreuse", ItemType.Plastron, ItemRarity.Commun, 8, 0, 8);
        CreateArmor("Pantalon_Commun_Mage", "Chausses Usées", ItemType.Pantalon, ItemRarity.Commun, 4, 0, 4);

        // --- Rareté : RARE ---
        // Tank
        CreateArmor("Casque_Rare_Tank", "Casque Solide", ItemType.Casque, ItemRarity.Rare, 30, 0, 0);
        CreateArmor("Plastron_Rare_Tank", "Armure Solide", ItemType.Plastron, ItemRarity.Rare, 60, 0, 0);
        CreateArmor("Pantalon_Rare_Tank", "Plaques Solides", ItemType.Pantalon, ItemRarity.Rare, 30, 0, 0);
        // Warrior
        CreateArmor("Casque_Rare_Guerrier", "Heaume de Mercenaire", ItemType.Casque, ItemRarity.Rare, 18, 10, 0);
        CreateArmor("Plastron_Rare_Guerrier", "Plastron de Mercenaire", ItemType.Plastron, ItemRarity.Rare, 36, 20, 0);
        CreateArmor("Pantalon_Rare_Guerrier", "Grèves de Mercenaire", ItemType.Pantalon, ItemRarity.Rare, 18, 10, 0);
        // Mage
        CreateArmor("Casque_Rare_Mage", "Chapeau d'Adepte", ItemType.Casque, ItemRarity.Rare, 12, 0, 14);
        CreateArmor("Plastron_Rare_Mage", "Tunique d'Adepte", ItemType.Plastron, ItemRarity.Rare, 24, 0, 28);
        CreateArmor("Pantalon_Rare_Mage", "Bottes d'Adepte", ItemType.Pantalon, ItemRarity.Rare, 12, 0, 14);

        // --- Rareté : EPIC ---
        // Tank
        CreateArmor("Casque_Epic_Tank", "Masque de Fer", ItemType.Casque, ItemRarity.Epic, 80, 0, 0);
        CreateArmor("Plastron_Epic_Tank", "Forteresse de Fer", ItemType.Plastron, ItemRarity.Epic, 160, 0, 0);
        CreateArmor("Pantalon_Epic_Tank", "Bastion de Fer", ItemType.Pantalon, ItemRarity.Epic, 80, 0, 0);
        // Warrior
        CreateArmor("Casque_Epic_Guerrier", "Visage du Chef", ItemType.Casque, ItemRarity.Epic, 50, 25, 0);
        CreateArmor("Plastron_Epic_Guerrier", "Harnais du Chef", ItemType.Plastron, ItemRarity.Epic, 100, 50, 0);
        CreateArmor("Pantalon_Epic_Guerrier", "Marche du Chef", ItemType.Pantalon, ItemRarity.Epic, 50, 25, 0);
        // Mage
        CreateArmor("Casque_Epic_Mage", "Diadème Mystique", ItemType.Casque, ItemRarity.Epic, 30, 0, 35);
        CreateArmor("Plastron_Epic_Mage", "Manteau Mystique", ItemType.Plastron, ItemRarity.Epic, 60, 0, 70);
        CreateArmor("Pantalon_Epic_Mage", "Traces Mystiques", ItemType.Pantalon, ItemRarity.Epic, 30, 0, 35);

        // --- Rareté : LEGENDAIRE ---
        // Tank
        CreateArmor("Casque_Legendaire_Tank", "Heaume Inébranlable", ItemType.Casque, ItemRarity.Legendaire, 200, 0, 0);
        CreateArmor("Plastron_Legendaire_Tank", "Muraille Inébranlable", ItemType.Plastron, ItemRarity.Legendaire, 400, 0, 0);
        CreateArmor("Pantalon_Legendaire_Tank", "Piliers Inébranlables", ItemType.Pantalon, ItemRarity.Legendaire, 200, 0, 0);
        // Warrior
        CreateArmor("Casque_Legendaire_Guerrier", "Couronne de Carnage", ItemType.Casque, ItemRarity.Legendaire, 120, 60, 0);
        CreateArmor("Plastron_Legendaire_Guerrier", "Manteau de Carnage", ItemType.Plastron, ItemRarity.Legendaire, 240, 120, 0);
        CreateArmor("Pantalon_Legendaire_Guerrier", "Foulée de Carnage", ItemType.Pantalon, ItemRarity.Legendaire, 120, 60, 0);
        // Mage
        CreateArmor("Casque_Legendaire_Mage", "Halo Omniscient", ItemType.Casque, ItemRarity.Legendaire, 80, 0, 90);
        CreateArmor("Plastron_Legendaire_Mage", "Robe Omnisciente", ItemType.Plastron, ItemRarity.Legendaire, 160, 0, 180);
        CreateArmor("Pantalon_Legendaire_Mage", "Lévitation Omnisciente", ItemType.Pantalon, ItemRarity.Legendaire, 80, 0, 90);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Base de données d'armures générée avec succès !");
    }

    private static void CreateArmor(string fileName, string itemName, ItemType type, ItemRarity rarity, int hp, int phys, int mag)
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.itemName = itemName;
        item.type = type;
        item.rarity = rarity;
        item.scalingType = StatScaling.Aucun;
        item.mainStatValue = 0;
        item.bonusHP = hp;
        item.bonusPhysique = phys;
        item.bonusMagie = mag;

        string path = $"Assets/Items/Armors/{fileName}.asset";
        AssetDatabase.CreateAsset(item, path);
    }
}