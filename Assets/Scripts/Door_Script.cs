using Unity.VisualScripting;
using UnityEngine;

public class Door_Script : MonoBehaviour, IDoor
{
    [SerializeField] Animator Animator;
    [SerializeField] Collider DoorTrigger;
    [SerializeField] private IItem requiredKey;

    [SerializeField] private bool isLocked;

    private int triggerCache;

    private void Update()
    {
    }

    public void Open()
    {
        Animator.SetTrigger("OpenDoor");
    }

    public void Close()
    {
        Animator.SetTrigger("CloseDoor");
    }

    public float GetCurrentAnimationState()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(isLocked && !collision.CompareTag("Player")) return;

        if(collision.CompareTag("Player"))
        {
            triggerCache++;

            if (isLocked) 
            {
                foreach (IItem item in collision.GetComponent<InventoryManager>().keyItems)
                {
                    if (item.ID == requiredKey.ID)
                    {
                        isLocked = false;
                        return;
                    }
                    else
                    {
                        // Play locked animation
                        return;
                    }
                };
            }

            Open();
        }

        if(collision.CompareTag("Enemy") )
        {
            triggerCache++;
            Open();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            triggerCache--;
        }

        if(triggerCache > 0) return;

        Close();
    }

    
}
