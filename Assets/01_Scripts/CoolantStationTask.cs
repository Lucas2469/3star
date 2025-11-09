using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class CoolantStationTaskWithReturn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI refs")]
    public Image tubeFill;
    public RectTransform coolantCan;
    public Button holdButton;
    public TMP_Text statusText;
    public Image readyLight;

    [Header("Tuning")]
    public float fillSeconds = 3.5f;
    public float emptySeconds = 3.5f;
    public float idleLeakPerSecond = 0.0f;

    [Header("Mapa")]
    public string escenaMapa = "Mapa";
    public float tiempoMensaje = 2f;

    float fill;
    float canLevel = 1f;
    bool isHolding;
    bool completed;

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetStatus("Mantén presionado para bombear");
        if (readyLight) readyLight.enabled = false;
        UpdateUI();

        // Asegura que el botón propague eventos al script:
        var trigger = holdButton.gameObject.GetComponent<EventTrigger>();
        if (!trigger)
            trigger = holdButton.gameObject.AddComponent<EventTrigger>();
        AddEvent(trigger, EventTriggerType.PointerDown, (e) => OnPointerDown(null));
        AddEvent(trigger, EventTriggerType.PointerUp, (e) => OnPointerUp(null));

        if (holdButton) holdButton.interactable = true;
    }

    void AddEvent(EventTrigger trg, EventTriggerType type, System.Action<BaseEventData> cb)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(cb));
        trg.triggers.Add(entry);
    }

    public void OnPointerDown(PointerEventData eventData) { isHolding = true; }
    public void OnPointerUp(PointerEventData eventData) { isHolding = false; }

    void Update()
    {
        if (completed) return;

        if (isHolding && canLevel > 0f)
        {
            fill += Time.deltaTime / Mathf.Max(0.01f, fillSeconds);
            canLevel -= Time.deltaTime / Mathf.Max(0.01f, emptySeconds);
        }
        else if (idleLeakPerSecond > 0f)
        {
            fill -= idleLeakPerSecond * Time.deltaTime;
        }

        fill = Mathf.Clamp01(fill);
        canLevel = Mathf.Clamp01(canLevel);

        UpdateUI();

        if (!completed && fill >= 1f)
        {
            CompleteTask();
        }
    }

    void UpdateUI()
    {
        if (tubeFill) tubeFill.fillAmount = fill;
        if (coolantCan) coolantCan.localScale = new Vector3(1f, Mathf.Max(0.05f, canLevel), 1f);
    }

    void SetStatus(string msg)
    {
        if (statusText) statusText.text = msg;
    }

    void CompleteTask()
    {
        completed = true;
        SetStatus("¡Tarea completada!");
        if (readyLight) readyLight.enabled = true;
        if (holdButton) holdButton.interactable = false;

        StartCoroutine(VolverAMapaConDelay());
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

        // Nota: para colocar al jugador en la posición exacta,
        // usar script en jugador que lea PlayerPrefs al Start() en la escena Mapa
    }
}
