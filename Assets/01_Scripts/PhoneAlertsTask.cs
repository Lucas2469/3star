using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhoneAlertsTaskWithReturn : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform screen;
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    public AlertTile warnPrefab;
    public TMP_Text statusText;

    [Header("Status Colors")]
    public Color badColor = new Color(0.9f, 0.2f, 0.2f, 1f);
    public Color goodColor = new Color(0.2f, 0.9f, 0.3f, 1f);

    [Header("Mapa")]
    public string escenaMapa = "Mapa";
    public float tiempoMensaje = 2f;

    AlertTile[] tiles;

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        BuildTiles();
        SetBad();
    }

    void BuildTiles()
    {
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
        // Si todos los tiles están desactivados
        foreach (var t in tiles)
            if (t.gameObject.activeSelf) return;

        // Listo: GOOD
        SetGood();

        // Mostrar mensaje de completado y volver al mapa
        if (statusText != null)
        {
            statusText.text = "¡Tarea completada!";
            statusText.color = goodColor;
        }

        StartCoroutine(VolverAMapaConDelay());
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
