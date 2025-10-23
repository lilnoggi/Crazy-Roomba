using UnityEngine;

public class GameStarts : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PrintInstructions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintInstructions()
    {
        Debug.Log("Welcome to Crazy Roomba!");
        Debug.Log("Use WASD or arrow keys to move the Roomba.");
    }
}
