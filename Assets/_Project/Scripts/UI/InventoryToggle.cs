using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public KeyCode toggleKey = KeyCode.I;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            inventoryUI.ToggleInventory();
        }
    }

    // Utilisable par le bouton UI
    public void OnButtonClick()
    {
        inventoryUI.ToggleInventory();
    }
}
