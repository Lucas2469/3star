using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CableDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public RectTransform canvasRect;
    public RectTransform cableContainer;
    public Image cablePrefab;

    private Image currentCable;
    private RectTransform currentRect;
    private Vector2 startPos;
    private string colorName;
    private bool connected = false; // <- nuevo

    void Start()
    {
        colorName = gameObject.name.Replace("Point", "");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (connected) return; // <- si ya está conectado, no se arrastra más

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
        Debug.LogWarning($"⚠️ No se detectó ningún target bajo el cursor al soltar el cable {colorName}");
        Destroy(currentCable.gameObject);
        return;
    }

    Debug.Log($"🎯 Se soltó sobre: {target.name}");

    if (target.name == "Target" + colorName)
    {
        connected = true;
        UpdateCableLine(startPos, target.transform.position);
        currentCable.transform.SetParent(cableContainer, true);
        GetComponent<Image>().raycastTarget = false;

        Debug.Log($"✅ Cable {colorName} conectado correctamente");
    }
    else
    {
        Destroy(currentCable.gameObject);
        Debug.Log($"❌ Cable {colorName} incorrecto");
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
            case "pink": return Color.pink;
            case "purple": return Color.purple;
            case "orange": return Color.orange;
            default: return Color.white;
        }
    }
}
