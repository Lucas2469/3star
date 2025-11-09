using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTrigger : MonoBehaviour
{
    [SerializeField] public string nombreEscenaMinijuego;

public string GetNombreEscena()
{
    return nombreEscenaMinijuego;
}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar posición actual del jugador antes de cambiar de escena
            Vector3 pos = other.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", pos.x);
            PlayerPrefs.SetFloat("PlayerPosY", pos.y);
            PlayerPrefs.SetFloat("PlayerPosZ", pos.z);
            PlayerPrefs.Save();

            // Cargar escena del minijuego
            SceneManager.LoadScene(nombreEscenaMinijuego);
        }
    }
}
