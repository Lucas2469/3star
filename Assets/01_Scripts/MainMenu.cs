using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Jugar()
    {
        // Cambia "Nivel1" por el nombre exacto de tu escena del juego
        SceneManager.LoadScene("Game");
    }

    public void Opciones()
    {
        // Muestra el men� de ajustes (puedes activar un panel)
        Debug.Log("Abrir men� de opciones");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
