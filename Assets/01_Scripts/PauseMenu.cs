using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Continue()
    {
        // Cambiar l�gica de pausa, retornando al juego sin recargar toda la escena, recuperando el progreso
        SceneManager.LoadScene("Mapa");
    }

    public void Settings()
    {
        // Muestra el men� de ajustes (puedes activar un panel)
        Debug.Log("Settings Scene loading...");
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ReturnMainMenu()
    {
        Debug.Log("Main Menu loading...");
        SceneManager.LoadScene("MainMenu");
    }
}
