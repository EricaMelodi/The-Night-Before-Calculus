using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;    
    public GameObject settingsPanel; 
    public MonoBehaviour playerController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key detected!");

            if (pausePanel.activeSelf || settingsPanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;

        playerController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Game Resumed");
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f;

        playerController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Game Paused");
    }

    public void ShowSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);

        if (GetComponent<SettingsUI>() != null)
        {
            GetComponent<SettingsUI>().SetupSliders();
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}