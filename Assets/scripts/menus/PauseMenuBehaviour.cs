using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuBehaviour : MonoBehaviour
{
    public static bool isPaused = false;
    
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    private GameObject activePanel;

    void Start()
    {
        activePanel = null;
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) && (activePanel == pauseMenuPanel || activePanel == null))
        {
            OpenClosePauseMenu(!pauseMenuPanel.activeSelf);
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) &&
                 (activePanel != pauseMenuPanel && activePanel != null))
        {
            OpenClosePauseMenu();
        }

        if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.JoystickButton1)) && isPaused == true)
        {
            ClosePanel(activePanel);
        }
    }

    public void OpenClosePauseMenu(bool menuState = false)
    {
        pauseMenuPanel.SetActive(menuState);
        settingsPanel.SetActive(false);
        if (menuState)
        {
            isPaused = true;
            activePanel = pauseMenuPanel;
            Time.timeScale = 0f;
        }

        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            activePanel = null;
        }
    }
    
    public void OpenPanel(GameObject panel)
    {
        pauseMenuPanel.SetActive(false);
        activePanel = panel;
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        if (activePanel == pauseMenuPanel)
        {
            OpenClosePauseMenu(false);
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            activePanel = pauseMenuPanel;
            panel.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
