using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CoolantStationTask : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI refs")]
    public Image tubeFill;        // coolant_fill_196x696 (Filled Vertical)
    public RectTransform coolantCan; // icono bidón
    public Button holdButton;
    public TMP_Text statusText;
    public Image readyLight;      // opcional

    [Header("Tuning")]
    public float fillSeconds = 3.5f;   // tiempo sosteniendo para llenar
    public float emptySeconds = 3.5f;  // tiempo que tarda en vaciar el bidón (escala Y)
    public float idleLeakPerSecond = 0.0f; // si quieres que baje cuando sueltas

    float fill; // 0..1
    float canLevel = 1f; // escala Y del bidón 1..0
    bool isHolding;

    void Start()
    {
        SetStatus("Mantén presionado para bombear");
        if (readyLight) readyLight.enabled = false;
        UpdateUI();
        // Asegura que el botón propaga eventos al script:
        var trigger = holdButton.gameObject.GetComponent<EventTrigger>();
        if (!trigger)
            trigger = holdButton.gameObject.AddComponent<EventTrigger>();
        AddEvent(trigger, EventTriggerType.PointerDown, (e) => OnPointerDown(null));
        AddEvent(trigger, EventTriggerType.PointerUp, (e) => OnPointerUp(null));
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
        if (IsCompleted) return;

        if (isHolding && canLevel > 0f)
        {
            // Llenar tubo
            fill += Time.deltaTime / Mathf.Max(0.01f, fillSeconds);
            // Vaciar bidón
            canLevel -= Time.deltaTime / Mathf.Max(0.01f, emptySeconds);
        }
        else
        {
            // fuga opcional del tubo cuando no sostienes
            if (idleLeakPerSecond > 0f) fill -= idleLeakPerSecond * Time.deltaTime;
        }

        fill = Mathf.Clamp01(fill);
        canLevel = Mathf.Clamp01(canLevel);

        UpdateUI();

        if (!IsCompleted && fill >= 1f)
        {
            CompleteTask();
        }
    }

    bool IsCompleted => fill >= 1f;

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
        SetStatus("¡Tarea completada!");
        if (readyLight) readyLight.enabled = true;
        if (holdButton) holdButton.interactable = false;
        // TODO: aquí puedes notificar a tu GameManager
    }
}
