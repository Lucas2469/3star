using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemperatureTask : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text valueLeft;     // “0”
    public TMP_Text valueRight;    // “-29°”
    public TMP_Text statusText;    // mensajes
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

    private int current;
    private int target;
    private bool finished;

    void Start()
    {
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
        // color de “pista”: si estás por debajo, rojizo; si por encima, azulado (opcional)
        if (current < target) valueLeft.color = new Color(1f, 0.75f, 0.6f); // bajo
        else if (current > target) valueLeft.color = new Color(0.6f, 0.8f, 1f);  // alto
        else valueLeft.color = matchColor;                 // igual
    }

    void CheckWin()
    {
        if (current == target)
        {
            finished = true;
            statusText.text = "¡Tarea completada!";
            statusText.color = matchColor;
            SetButtonsInteractable(false);

            // Aquí puedes notificar al GameManager o cerrar la tarea.
            // StartCoroutine(CloseAfter(1.0f));
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
}
