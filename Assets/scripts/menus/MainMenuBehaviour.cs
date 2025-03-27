using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;

    void Start()
    {
        MainMenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenPanel(GameObject panel)
    {
        MainMenuPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        MainMenuPanel.SetActive(true);
        panel.SetActive(false);
    }
}
