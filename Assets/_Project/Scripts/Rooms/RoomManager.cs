using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public GameObject bossLootPrefab;
    public List<GameObject> doors;
    public Transform nextRoomSpawnPoint; // L'objet vide à la sortie
    private bool isCleared = false;
    private bool playerInside = false;

    // Appelé quand le joueur entre dans la zone de combat
    public void OnPlayerEnter()
    {
        if (!isCleared && !playerInside)
        {
            playerInside = true;
            foreach (GameObject door in doors) door.SetActive(true); // Ferme les portes
            Debug.Log("Portes fermées, tuez les ennemis !");
        }
    }

    public void CheckEnemies()
    {
        if (isCleared) return;

        // On compte les scripts EnemyHealth encore vivants dans les enfants de la salle
        int enemyCount = transform.GetComponentsInChildren<EnemyHealth>().Length;
        // Si 1 ou 0, c'est que c'est fini (l'objet en destruction compte parfois encore)
        if (enemyCount == 0)
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        isCleared = true;
        foreach (GameObject door in doors) if (door != null) door.SetActive(false);

        if (DungeonGenerator.instance != null && nextRoomSpawnPoint != null)
        {
            DungeonGenerator.instance.SpawnNextRoom(nextRoomSpawnPoint.position);
        }
        else if (nextRoomSpawnPoint == null) // Condition de Boss[cite: 1]
        {
            // 1. On fait tomber le loot (ton code actuel)
            if (bossLootPrefab != null)
            {
                Instantiate(bossLootPrefab, transform.position, Quaternion.identity);
            }

            // 2. On affiche les boutons de victoire
            if (VictoryManager.instance != null)
            {
                // On peut ajouter un petit délai via Invoke si on veut que le joueur 
                // ait le temps de voir le Boss mourir avant l'affichage.
                VictoryManager.instance.Invoke("ShowVictory", 1.5f);
            }
        }
    }
}