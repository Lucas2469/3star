using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class HexTile : MonoBehaviour, IPointerClickHandler
{
    [Header("Colores")]
    public Color activeColor = new Color(1f, 0.2f, 0.2f, 1f);      // rojo encendido
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.25f);      // blanco translúcido

    [Header("Animación simple")]
    public float clickPunchScale = 1.1f;
    public float clickAnimTime = 0.08f;

    public bool IsActive { get; private set; } = true;

    Image img;
    Vector3 baseScale;

    public event Action<HexTile> OnBecameInactive;

    void Awake()
    {
        img = GetComponent<Image>();
        baseScale = transform.localScale;
    }

    void Start()
    {
        ApplyVisual();
    }

    public void SetState(bool active)
    {
        IsActive = active;
        ApplyVisual();
    }

    void ApplyVisual()
    {
        img.color = IsActive ? activeColor : inactiveColor;
        img.raycastTarget = true; // asegúrate de recibir clics
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive) return;

        // Apagar (rojo -> blanco translúcido)
        SetState(false);

        // Micro-animación de “pulsito”
        StopAllCoroutines();
        StartCoroutine(PunchScale());

        OnBecameInactive?.Invoke(this);
    }
    public void Hide()
    {
        // Vuelve invisible y ya no recibe clics
        var c = img.color;
        c.a = 0f;
        img.color = c;
        img.raycastTarget = false;
    }


    System.Collections.IEnumerator PunchScale()
    {
        float t = 0f;
        while (t < clickAnimTime)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Sin((t / clickAnimTime) * Mathf.PI); // va y vuelve
            transform.localScale = Vector3.Lerp(baseScale, baseScale * clickPunchScale, k);
            yield return null;
        }
        transform.localScale = baseScale;
    }
}
