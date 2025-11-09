using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberOrderTaskWithReturn : MonoBehaviour
{
    [Header("UI")]
    public RectTransform grid;    
    public NumberTile tilePrefab; 
    public TMP_Text statusText;   

    [Header("Sprites")]
    public Sprite tileNormal;
    public Sprite tileCorrect;
    public Sprite tileError;

    [Header("Config")]
    public float errorFlashSeconds = 2f;
    public string escenaMapa = "Mapa";       // Escena a la que volver
    public float tiempoMensaje = 2f;         // Tiempo antes de volver a mapa

    public int[] fixedOrder = new int[] { 5, 6, 4, 7, 9, 2, 8, 1, 3, 10 };

    List<NumberTile> tiles = new List<NumberTile>();
    int nextExpected = 1;
    bool locked = false;

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        BuildTiles();
        ApplyFixedOrder();
        ResetState();
    }

    void BuildTiles()
    {
        foreach (Transform c in grid) Destroy(c.gameObject);
        tiles.Clear();

        for (int n = 1; n <= 10; n++)
        {
            var t = Instantiate(tilePrefab, grid);
            t.Init(n, tileNormal, tileCorrect, tileError, OnTileClicked);
            tiles.Add(t);
        }
    }

    void ApplyFixedOrder()
    {
        var map = new Dictionary<int, NumberTile>(tiles.Count);
        foreach (var t in tiles) map[t.value] = t;

        int sibling = 0;
        for (int i = 0; i < fixedOrder.Length; i++)
        {
            int num = fixedOrder[i];
            if (map.TryGetValue(num, out var tile))
                tile.transform.SetSiblingIndex(sibling++);
        }
    }

    void ResetState()
    {
        nextExpected = 1;
        locked = false;
        if (statusText) statusText.text = "Tócalos en orden ascendente";

        foreach (var t in tiles)
        {
            t.SetNormal();
            t.SetInteractable(true);
        }
    }

    void OnTileClicked(NumberTile tile)
    {
        if (locked) return;

        if (tile.value == nextExpected)
        {
            tile.SetCorrect();
            tile.SetInteractable(false);
            nextExpected++;

            if (nextExpected > 10)
            {
                if (statusText) statusText.text = "¡Tarea completada! ✅";
                StartCoroutine(VolverAMapaConDelay());
            }
        }
        else
        {
            StartCoroutine(FlashErrorAndReset());
        }
    }

    IEnumerator FlashErrorAndReset()
    {
        locked = true;

        foreach (var t in tiles)
        {
            t.SetError();
            t.SetInteractable(false);
        }

        if (statusText) statusText.text = "¡Orden incorrecto! Reiniciando...";
        yield return new WaitForSeconds(errorFlashSeconds);

        ApplyFixedOrder();
        ResetState();
    }

    IEnumerator VolverAMapaConDelay()
    {
        locked = true;
        yield return new WaitForSeconds(tiempoMensaje);

        // Restaurar posición del jugador antes de entrar al minijuego
        float x = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float y = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float z = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");

        // Cambiar a escena Mapa
        SceneManager.LoadScene(escenaMapa);

        // Nota: para restaurar posición, usar script en jugador en Mapa
        // que lea PlayerPrefs al Start() como hicimos antes
    }
}
