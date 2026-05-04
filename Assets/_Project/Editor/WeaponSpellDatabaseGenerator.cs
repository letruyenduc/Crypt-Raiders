using UnityEngine;
using UnityEditor;
using System.IO;

public class WeaponSpellDatabaseGenerator : EditorWindow
{
    [MenuItem("RPG/Générer Armes et Sorts")]
    public static void GenerateDatabase()
    {
        string folderPath = "Assets/Items/Weapons_And_Spells";

        // S'assurer que le dossier existe
        if (!Directory.Exists(Application.dataPath + "/Items/Weapons_And_Spells"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Items/Weapons_And_Spells");
            AssetDatabase.Refresh();
        }

        // --- 1. ARMES DE MÊLÉE (Type: Arme_Melee, Scaling: Physique) ---
        CreateItem("Arme_Melee_Commun_Dague", "Dague Émoussée", ItemType.Arme_Melee, ItemRarity.Commun, StatScaling.Physique, 5, 0, 2, 0);
        CreateItem("Arme_Melee_Rare_Epee", "Épée Longue", ItemType.Arme_Melee, ItemRarity.Rare, StatScaling.Physique, 20, 0, 8, 0);
        CreateItem("Arme_Melee_Epic_Hache", "Hache du Bourreau", ItemType.Arme_Melee, ItemRarity.Epic, StatScaling.Physique, 55, 0, 20, 0);
        CreateItem("Arme_Melee_Legendaire_Lame", "Lame Divine", ItemType.Arme_Melee, ItemRarity.Legendaire, StatScaling.Physique, 140, 0, 50, 0);

        // --- 2. ARMES MAGIQUES (Type: Arme_Magique, Scaling: Magique) ---
        CreateItem("Arme_Magique_Commun_Baton", "Bâton Noueux", ItemType.Arme_Magique, ItemRarity.Commun, StatScaling.Magique, 6, 0, 0, 3);
        CreateItem("Arme_Magique_Rare_Grimoire", "Grimoire d'Adepte", ItemType.Arme_Magique, ItemRarity.Rare, StatScaling.Magique, 22, 0, 0, 10);
        CreateItem("Arme_Magique_Epic_Sceptre", "Sceptre Astral", ItemType.Arme_Magique, ItemRarity.Epic, StatScaling.Magique, 60, 0, 0, 25);
        CreateItem("Arme_Magique_Legendaire_Orbe", "Orbe d'Éternité", ItemType.Arme_Magique, ItemRarity.Legendaire, StatScaling.Magique, 155, 0, 0, 60);

        // --- 3. SORTS PHYSIQUES (Type: Sort, Scaling: Physique) ---
        CreateItem("Sort_Commun_Coup", "Coup de Pommeau", ItemType.Sort, ItemRarity.Commun, StatScaling.Physique, 12, 0, 0, 0);
        CreateItem("Sort_Rare_Frappe", "Frappe Sismique", ItemType.Sort, ItemRarity.Rare, StatScaling.Physique, 35, 0, 0, 0);
        CreateItem("Sort_Epic_Tourbillon", "Tourbillon de Lames", ItemType.Sort, ItemRarity.Epic, StatScaling.Physique, 85, 0, 0, 0);
        CreateItem("Sort_Legendaire_Saut", "Saut Dévastateur", ItemType.Sort, ItemRarity.Legendaire, StatScaling.Physique, 220, 0, 0, 0);

        // --- 4. SORTS MAGIQUES (Type: Sort, Scaling: Magique) ---
        CreateItem("Sort_Commun_Etincelle", "Étincelle", ItemType.Sort, ItemRarity.Commun, StatScaling.Magique, 15, 0, 0, 0);
        CreateItem("Sort_Rare_Eclair", "Éclair Givreux", ItemType.Sort, ItemRarity.Rare, StatScaling.Magique, 40, 0, 0, 0);
        CreateItem("Sort_Epic_Explosion", "Explosion Arcanique", ItemType.Sort, ItemRarity.Epic, StatScaling.Magique, 100, 0, 0, 0);
        CreateItem("Sort_Legendaire_Meteore", "Pluie de Météores", ItemType.Sort, ItemRarity.Legendaire, StatScaling.Magique, 250, 0, 0, 0);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Base de données d'armes et de sorts générée avec succès !");
    }

    private static void CreateItem(string fileName, string itemName, ItemType type, ItemRarity rarity, StatScaling scaling, int mainStat, int hp, int phys, int mag)
    {
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.itemName = itemName;
        item.type = type;
        item.rarity = rarity;
        item.scalingType = scaling;
        item.mainStatValue = mainStat;
        item.bonusHP = hp;
        item.bonusPhysique = phys;
        item.bonusMagie = mag;

        string path = $"Assets/Items/Weapons_And_Spells/{fileName}.asset";
        AssetDatabase.CreateAsset(item, path);
    }
}