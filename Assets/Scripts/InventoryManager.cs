using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<IItem> inventory;

    public IItem equipedItem;

    public GameObject powercell;
}
