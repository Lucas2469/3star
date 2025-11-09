using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TemperatureTaskWithReturn : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text valueLeft;
    public TMP_Text valueRight;
    public TMP_Text statusText;
    public Button btnUp;
    public Button btnDown;

    [Header("Rango")]
    public int minValue = -50;
    public int maxValue = 50;

    [Header("Objetivo")]
    public bool randomTarget = false;
    public int fixedTarget = -29;

    [Header("Feedback")]
    public Color normalColor = Color.white;
    public Color matchColor = new Color(0.2f, 1f, 0.4f);
    public Color loseColor = new Color(1f, 0.6f, 0.6f);

    [Header("Mapa")]
    public string escenaMapa = "Mapa";
    public float tiempoMensaje = 2f;

    private int current;
    private int target;
    private bool finished;

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Objetivo
        target = randomTarget ? Random.Range(minValue, maxValue + 1) : fixedTarget;
        valueRight.text = target.ToString() + "°";

        // Estado inicial
        current = Mathf.Clamp(0, minValue, maxValue);
        valueLeft.text = current.ToString();
        valueLeft.color = normalColor;
        statusText.text = "";

        // Eventos
        btnUp.onClick.AddListener(OnUp);
        btnDown.onClick.AddListener(OnDown);

        finished = false;
        SetButtonsInteractable(true);
    }

    void OnUp()
    {
        if (finished) return;
        if (current < maxValue)
        {
            current++;
            UpdateLeft();
            CheckWin();
        }
    }

    void OnDown()
    {
        if (finished) return;
        if (current > minValue)
        {
            current--;
            UpdateLeft();
            CheckWin();
        }
    }

    void UpdateLeft()
    {
        valueLeft.text = current.ToString();
        if (current < target) valueLeft.color = new Color(1f, 0.75f, 0.6f); // bajo
        else if (current > target) valueLeft.color = new Color(0.6f, 0.8f, 1f); // alto
        else valueLeft.color = matchColor; // igual
    }

    void CheckWin()
    {
        if (current == target)
        {
            finished = true;
            statusText.text = "¡Tarea completada!";
            statusText.color = matchColor;
            SetButtonsInteractable(false);

            // Volver al mapa después de delay
            StartCoroutine(VolverAMapaConDelay());
        }
        else
        {
            statusText.text = "";
        }
    }

    void SetButtonsInteractable(bool v)
    {
        btnUp.interactable = v;
        btnDown.interactable = v;
    }

    IEnumerator VolverAMapaConDelay()
    {
        yield return new WaitForSeconds(tiempoMensaje);

        // Restaurar posición del jugador desde PlayerPrefs
        float x = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float y = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float z = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");

        // Cambiar a escena Mapa
        SceneManager.LoadScene(escenaMapa);

        // Nota: para colocar al jugador en la posición exacta, usar script en jugador
        // que lea PlayerPrefs al Start() en la escena Mapa
    }
}
