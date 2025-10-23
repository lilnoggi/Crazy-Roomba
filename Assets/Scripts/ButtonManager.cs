using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject tutorialPanel;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TutorialButton()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- SCENE MANAGEMENT --- \\

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Exit Application.");
        Application.Quit();
    }
}
