using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject tutorialPanel; // Reference to the tutorial panel UI

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if the Escape key is pressed
        {
            PauseGame(); // Call the method to pause the game and show the tutorial panel
        }
    }

    void PauseGame() 
    {
        tutorialPanel.SetActive(true); // Show the tutorial panel
        Time.timeScale = 0f; // Pause the game

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    public void TutorialButton()
    {
        tutorialPanel.SetActive(false); // Hide the tutorial panel
        Time.timeScale = 1f; // Resume the game

        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }

    // --- SCENE MANAGEMENT --- \\

    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Load the main game scene
    }

    public void QuitGame()
    {
        Debug.Log("Exit Application."); // Log message for quitting the game
        Application.Quit(); // Quit the application
    }
}
