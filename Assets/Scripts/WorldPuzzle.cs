using Unity.VisualScripting;
using UnityEngine;

public class WorldPuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;
    [SerializeField] Animation ZoomIn;
    [SerializeField] Animation ZoomOut;

    [SerializeField] Animation PuzzleComplete;

    public void Interact(GameObject player)
    {
        ZoomIn.Play();


    }
}
