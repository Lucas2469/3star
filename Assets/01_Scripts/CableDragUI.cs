using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CableDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referencias")]
    public RectTransform canvasRect;
    public RectTransform cableContainer;
    public Image cablePrefab;

    [Header("Gestor del Juego")]
    public TextMeshProUGUI mensajeCompleto;
    public string escenaPrincipal = "Mapa";

    private static CableDragUI[] todosLosCables; // Se llenará automáticamente
    private Image currentCable;
    private RectTransform currentRect;
    private Vector2 startPos;
    private string colorName;
    private bool connected = false;

    void Awake()
    {
        // Encuentra todos los cables en la escena
        todosLosCables = FindObjectsOfType<CableDragUI>();

        // Oculta el mensaje al inicio
        if (mensajeCompleto != null)
            mensajeCompleto.gameObject.SetActive(false);
    }

    void Start()
    {
         // Mostrar el cursor y desbloquearlo
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
        colorName = gameObject.name.Replace("Point", "");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (connected) return;

        currentCable = Instantiate(cablePrefab, cableContainer);
        currentRect = currentCable.rectTransform;
        currentCable.raycastTarget = false;

        startPos = transform.position;
        currentRect.position = startPos;
        currentCable.color = GetColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (connected || currentCable == null) return;

        Vector2 endPos = eventData.position;
        UpdateCableLine(startPos, endPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (connected || currentCable == null) return;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target == null)
        {
            Destroy(currentCable.gameObject);
            return;
        }

        if (target.name == "Target" + colorName)
        {
            connected = true;
            UpdateCableLine(startPos, target.transform.position);
            currentCable.transform.SetParent(cableContainer, true);
            GetComponent<Image>().raycastTarget = false;

            VerificarTodosConectados();
        }
        else
        {
            Destroy(currentCable.gameObject);
        }

        currentCable = null;
        currentRect = null;
    }

    private void UpdateCableLine(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        float distance = dir.magnitude;

        currentRect.position = start + dir * 0.5f;
        currentRect.sizeDelta = new Vector2(distance, 8f);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        currentRect.rotation = Quaternion.Euler(0, 0, angle);
    }

    private Color GetColor()
    {
        switch (colorName.ToLower())
        {
            case "red": return Color.red;
            case "blue": return Color.blue;
            case "yellow": return Color.yellow;
            case "green": return Color.green;
            case "pink": return Color.magenta;
            case "purple": return new Color(0.5f, 0, 0.5f);
            case "orange": return new Color(1f, 0.5f, 0);
            default: return Color.white;
        }
    }

    public bool EstaConectado()
    {
        return connected;
    }

    private void VerificarTodosConectados()
    {
        foreach (var cable in todosLosCables)
        {
            if (!cable.EstaConectado())
            {
                return; // Todavía falta alguno
            }
        }

        // Todos conectados ✅
        if (mensajeCompleto != null)
        {
            mensajeCompleto.text = "¡Reparación completada!";
            mensajeCompleto.gameObject.SetActive(true);
        }

        // Cambia de escena después de 2 segundos
        Invoke(nameof(VolverAlMapa), 2f);
    }

private void VolverAlMapa()
{
    // Guardar la posición del jugador/cámara
    GameObject jugador = GameObject.FindWithTag("Player"); // o la cámara principal si quieres
    if (jugador != null)
    {
        Vector3 pos = jugador.transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", pos.z);
        PlayerPrefs.Save();
    }

    SceneManager.LoadScene(escenaPrincipal);
}

}
