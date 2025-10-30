using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlignLinesTask : MonoBehaviour
{
    [Header("UI")]
    public Slider lever;
    public RectTransform rotatingLines;
    public Image guideLines;
    public Image rotatingLinesImage;
    public TMP_Text statusText;

    [Header("Config")]
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float targetAngle = 0f;      // donde debe quedar alineado
    public float tolerance = 3f;        // ± grados aceptados
    public Color colorOff = new Color(1f, 0.16f, 0.16f, 1f);  // rojo
    public Color colorOn = new Color(0.2f, 0.9f, 0.3f, 1f);  // verde

    bool completed;
    // en AlignLinesTask.cs
    [Header("Start")]
    public bool randomizeStart = true;   // o usa un startValue fijo
    [Range(0f, 1f)] public float startValue = 0.35f;
    public float startMinDelta = 8f;     // grados mínimos lejos del target

    void Awake()
    {
        statusText.text = "Alinea con la palanca";
        guideLines.color = colorOff;
        rotatingLinesImage.color = colorOff;

        lever.onValueChanged.RemoveAllListeners();
        lever.onValueChanged.AddListener(OnLeverChanged);

        // --- Forzar inicio no alineado ---
        if (randomizeStart)
        {
            // busca un valor inicial que deje el ángulo al menos 'startMinDelta' lejos del target
            for (int i = 0; i < 20; i++)
            {
                float v = Random.Range(0f, 1f);
                float ang = Mathf.Lerp(minAngle, maxAngle, v);
                if (Mathf.Abs(Mathf.DeltaAngle(ang, targetAngle)) > startMinDelta)
                {
                    lever.SetValueWithoutNotify(v);
                    break;
                }
            }
        }
        else
        {
            lever.SetValueWithoutNotify(startValue);
        }

        OnLeverChanged(lever.value);
    }


    void OnLeverChanged(float v)
    {
        float angle = Mathf.Lerp(minAngle, maxAngle, v);
        rotatingLines.localEulerAngles = new Vector3(0, 0, angle);

        if (Mathf.Abs(Mathf.DeltaAngle(angle, targetAngle)) <= tolerance)
        {
            if (!completed)
            {
                guideLines.color = colorOn;
                rotatingLinesImage.color = colorOn;
                statusText.text = "¡Alineado! Tarea completada.";
                completed = true;
                // Aquí podrías desactivar interacción o avisar al gestor de tareas:
                lever.interactable = false;
                // gameObject.SetActive(false); // si quieres cerrar
            }
        }
        else
        {
            if (completed) return;
            guideLines.color = colorOff;
            rotatingLinesImage.color = colorOff;
            statusText.text = "Alinea con la palanca";
        }
    }
}
