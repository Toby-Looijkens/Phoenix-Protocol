using JetBrains.Annotations;
using UnityEngine;

public class FusePanel_Script : MonoBehaviour, IInteractable
{
    [SerializeField] IDoor door;
    [SerializeField] GameObject FusePrefab;
    [SerializeField] Animation FuseInsertionAnimation;
    [SerializeField] InventoryManager InventoryManager;

    public void Interact(GameObject player)
    {

    }

    public void InsertKeyItem(KeyItem item) 
    { 
        FuseInsertionAnimation.Play();
    }

    public void EndAnimationTrigger()
    {
        door.Open();
    }
}
