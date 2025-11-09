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
        string previousScene = PlayerPrefs.GetString("PauseMenu", "MainMenu");

        SceneManager.LoadScene(previousScene);
        PlayerPrefs.DeleteKey("PauseMenu");
    }
}
