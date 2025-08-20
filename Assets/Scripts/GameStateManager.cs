using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] string[] tasks;
    [SerializeField] TMP_Text hud;
    private int gameState = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hud.text = tasks[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CompleteTask()
    {
        gameState++;
        hud.text = tasks[gameState];
    }
}
