using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        // Leemos la escena anterior
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        // Si no existe, volverá al MainMenu por defecto
        SceneManager.LoadScene(previousScene);
        PlayerPrefs.DeleteKey("PreviousScene");
    }
}
