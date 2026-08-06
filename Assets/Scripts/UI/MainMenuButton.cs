using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // na wypadek pauzy lub Game Over
        SceneManager.LoadScene("MainMenu");
    }
}