using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> doors;
    public Transform nextRoomSpawnPoint; // Un objet vide placé à la sortie de la salle
    private bool isCleared = false;

    public void CheckEnemies()
    {
        if (isCleared) return;
        // On compte les objets avec le tag "Enemy" enfants de cette salle
        // (C'est plus simple que de maintenir une liste manuelle)
        int enemyCount = transform.GetComponentsInChildren<EnemyHealth>().Length;

        if (enemyCount <= 1) // On compte 1 car l'objet en train d'être détruit compte encore
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        isCleared = true;
        foreach (GameObject door in doors) door.SetActive(false);
        
        // On prévient le générateur qu'on a besoin d'une nouvelle salle
        DungeonGenerator.instance.SpawnNextRoom(nextRoomSpawnPoint.position);
    }
}