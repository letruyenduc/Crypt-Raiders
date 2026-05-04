using UnityEngine;
using UnityEditor;
using System.IO;

public class WeaponSpellDatabaseGenerator
{
    private const string FolderPath = "Assets/Items/Weapons_And_Spells";

    [MenuItem("RPG/Générer Armes et Sorts")]
    public static void GenerateDatabase()
    {
        // Vérifier et créer le dossier si nécessaire
        if (!AssetDatabase.IsValidFolder(FolderPath))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Items"))
            {
                AssetDatabase.CreateFolder("Assets", "Items");
            }
            AssetDatabase.CreateFolder("Assets/Items", "Weapons_And_Spells");
        }

        // --- 1. ARMES DE MÊLÉE ---
        CreateItem("Lame de Bronze", ItemType.Arme_Melee, ItemRarity.Commun, StatScaling.Physique, 5, 0, 2, 0);
        CreateItem("Tranche-Sable", ItemType.Arme_Melee, ItemRarity.Rare, StatScaling.Physique, 20, 0, 8, 0);
        CreateItem("Hache de la Crypte", ItemType.Arme_Melee, ItemRarity.Epic, StatScaling.Physique, 55, 0, 20, 0);
        CreateItem("Lame du Soleil", ItemType.Arme_Melee, ItemRarity.Legendaire, StatScaling.Physique, 140, 0, 50, 0);

        // --- 2. ARMES MAGIQUES ---
        CreateItem("Bâton de Roseau", ItemType.Arme_Magique, ItemRarity.Commun, StatScaling.Magique, 6, 0, 0, 3);
        CreateItem("Parchemin Ancien", ItemType.Arme_Magique, ItemRarity.Rare, StatScaling.Magique, 22, 0, 0, 10);
        CreateItem("Sceptre Royal", ItemType.Arme_Magique, ItemRarity.Epic, StatScaling.Magique, 60, 0, 0, 25);
        CreateItem("Oeil de Râ", ItemType.Arme_Magique, ItemRarity.Legendaire, StatScaling.Magique, 155, 0, 0, 60);

        // --- 3. SORTS PHYSIQUES ---
        CreateItem("Choc de Pierre", ItemType.Sort, ItemRarity.Commun, StatScaling.Physique, 12, 0, 0, 0);
        CreateItem("Frappe des Dunes", ItemType.Sort, ItemRarity.Rare, StatScaling.Physique, 35, 0, 0, 0);
        CreateItem("Tourbillon de Sable", ItemType.Sort, ItemRarity.Epic, StatScaling.Physique, 85, 0, 0, 0);

        // --- 4. SORTS MAGIQUES ---
        CreateItem("Brûlure Solaire", ItemType.Sort, ItemRarity.Commun, StatScaling.Magique, 15, 0, 0, 0);
        CreateItem("Éclat de Verre", ItemType.Sort, ItemRarity.Rare, StatScaling.Magique, 40, 0, 0, 0);
        CreateItem("Souffle de la Momie", ItemType.Sort, ItemRarity.Epic, StatScaling.Magique, 100, 0, 0, 0);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Base de données d'armes et sorts générée avec succès !");
    }

    private static void CreateItem(string name, ItemType type, ItemRarity rarity, StatScaling scaling, int mainStat, int bonusHP, int bonusPhys, int bonusMag)
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.itemName = name;
        item.type = type;
        item.rarity = rarity;
        item.scalingType = scaling;
        item.mainStatValue = mainStat;
        item.bonusHP = bonusHP;
        item.bonusPhysique = bonusPhys;
        item.bonusMagie = bonusMag;

        // Format de nom : [Type]_[Rareté]_[Nom_Sans_Espaces].asset
        string fileName = $"{type}_{rarity}_{name.Replace(" ", "")}.asset";
        string path = Path.Combine(FolderPath, fileName);

        AssetDatabase.CreateAsset(item, path);
    }
}
