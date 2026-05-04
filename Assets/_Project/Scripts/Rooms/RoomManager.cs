using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
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
        foreach (GameObject door in doors) door.SetActive(false);
        Debug.Log("Salle nettoyée !");

        // Si la salle a une sortie, on continue le donjon
        if (DungeonGenerator.instance != null && nextRoomSpawnPoint != null)
        {
            DungeonGenerator.instance.SpawnNextRoom(nextRoomSpawnPoint.position);
        }
        // Si la salle N'A PAS de sortie, c'est implacablement la salle du Boss
        else if (nextRoomSpawnPoint == null)
        {
            Debug.Log("BOSS VAINCU ! Fin du donjon.");

            // C'est exactement ici que nous coderons l'apparition de l'XP et le drop d'équipement à l'Étape 3.
        }
    }
}