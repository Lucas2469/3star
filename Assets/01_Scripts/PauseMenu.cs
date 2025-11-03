using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Continue()
    {
        // Cambiar lógica de pausa, retornando al juego sin recargar toda la escena, recuperando el progreso
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        // Muestra el menú de ajustes (puedes activar un panel)
        Debug.Log("Settings Scene loading...");
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ReturnMainMenu()
    {
        Debug.Log("Main Menu loading...");
        SceneManager.LoadScene("MainMenu");
    }
}
