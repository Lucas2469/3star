using UnityEngine;
using TMPro;

public class PhoneAlertsTask : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform screen;        // para referencia (no obligatorio)
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    public AlertTile warnPrefab;
    public TMP_Text statusText;

    [Header("Status Colors")]
    public Color badColor = new Color(0.9f, 0.2f, 0.2f, 1f);
    public Color goodColor = new Color(0.2f, 0.9f, 0.3f, 1f);

    AlertTile[] tiles;

    void Start()
    {
        BuildTiles();
        SetBad();
    }

    void BuildTiles()
    {
        // Instancia 4 y los ubica en las anclas dadas
        tiles = new AlertTile[4];
        tiles[0] = Instantiate(warnPrefab, topLeft);
        tiles[1] = Instantiate(warnPrefab, topRight);
        tiles[2] = Instantiate(warnPrefab, bottomLeft);
        tiles[3] = Instantiate(warnPrefab, bottomRight);

        foreach (var t in tiles)
        {
            t.onVanished = OnTileVanished;
            t.ResetTile();
        }
    }

    void OnTileVanished(AlertTile _)
    {
        // ¿Todos desactivados?
        foreach (var t in tiles)
            if (t.gameObject.activeSelf) return;

        // Listo: GOOD
        SetGood();
        // Aquí puedes notificar al sistema de tareas y cerrar el panel
        // gameObject.SetActive(false);
    }

    void SetBad()
    {
        if (statusText)
        {
            statusText.text = "Status: BAD";
            statusText.color = badColor;
        }
    }

    void SetGood()
    {
        if (statusText)
        {
            statusText.text = "Status: GOOD";
            statusText.color = goodColor;
        }
    }
}
