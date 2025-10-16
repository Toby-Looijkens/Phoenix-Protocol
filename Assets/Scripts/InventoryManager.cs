using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject powercell;

    public IItem[,] inventory;

    public IItem[] keyItems;

    [SerializeField] private InventoryUIManager_Script inventoryUIManager;

    // Input actions
    private InputAction inventoryAction;

    private void Start()
    {
        inventoryAction = InputSystem.actions.FindAction("Inventory");
    }

    public void OpenInventory(IInteractable interacted, List<Guid> keyitems)
    {

    }

    private void UnlockKeyItems(Guid ID)
    {
        
    }
}
