using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlignEnginePuzzle : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform targetArea;
    public RectTransform greenLine;
    public Slider sliderY;
    public Image holdFill;

    [Header("Gameplay")]
    [Tooltip("Margen en píxeles desde el centro para considerar 'alineado'")]
    public float alignTolerancePx = 6f;
    [Tooltip("Tiempo que debes mantener alineado para completar")]
    public float holdToWinSeconds = 0.6f;
    [Tooltip("Oscilación automática de la línea (dificultad)")]
    public float wobbleAmplitudePx = 30f;
    public float wobbleSpeed = 1.2f;

    [Header("Eventos")]
    public UnityEvent onCompleted;

    float holdTimer;
    float baseCenterY;
    bool completed;

    void Start()
    {
        baseCenterY = 0f;
        if (sliderY != null)
        {
            sliderY.minValue = -0.5f;
            sliderY.maxValue = 0.5f;
            sliderY.value   = 0f;
        }
        if (holdFill) holdFill.fillAmount = 0f;
    }

    void Update()
    {
        if (completed || targetArea == null || greenLine == null || sliderY == null) return;

        float usableHalfHeight = (targetArea.rect.height * 0.5f) - 4f;
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmplitudePx;
        float yFromSlider = sliderY.value * 2f * usableHalfHeight;
        float finalY = Mathf.Clamp(baseCenterY + yFromSlider + wobble, -usableHalfHeight, usableHalfHeight);

        var gl = greenLine.anchoredPosition;
        gl.y = finalY;
        greenLine.anchoredPosition = gl;

        bool aligned = Mathf.Abs(finalY - baseCenterY) <= alignTolerancePx;

        if (aligned) holdTimer += Time.deltaTime;
        else         holdTimer = Mathf.Max(0f, holdTimer - Time.deltaTime * 0.5f);

        if (holdFill) holdFill.fillAmount = Mathf.Clamp01(holdTimer / holdToWinSeconds);

        if (holdTimer >= holdToWinSeconds)
        {
            completed = true;
            if (holdFill) holdFill.fillAmount = 1f;
            onCompleted?.Invoke();
            if (sliderY) sliderY.interactable = false;
        }
    }
}
