using UnityEngine;

public class Door_Script : MonoBehaviour, IDoor
{
    [SerializeField] Animation Animation;

    public void Open()
    {
        Animation.Play();
    }

    public void Close()
    {
        Animation.Play();
    }
}
