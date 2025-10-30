using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberOrderTask : MonoBehaviour
{
    [Header("UI")]
    public RectTransform grid;    // contenedor con GridLayoutGroup
    public NumberTile tilePrefab; // ¡prefab con NumberTile en la raíz!
    public TMP_Text statusText;   // opcional

    [Header("Sprites")]
    public Sprite tileNormal;
    public Sprite tileCorrect;
    public Sprite tileError;

    [Header("Config")]
    public float errorFlashSeconds = 2f;

    // Orden fijo estilo Among Us (ajústalo a gusto)
    // Ejemplo como tu referencia: 5 6 4 7 9  |  2 8 1 3 10
    public int[] fixedOrder = new int[] { 5, 6, 4, 7, 9, 2, 8, 1, 3, 10 };

    List<NumberTile> tiles = new List<NumberTile>();
    int nextExpected = 1;
    bool locked = false;

    void Start()
    {
        BuildTiles();
        ApplyFixedOrder();
        ResetState();
    }

    void BuildTiles()
    {
        // Limpia hijos previos del grid
        foreach (Transform c in grid) Destroy(c.gameObject);
        tiles.Clear();

        // Crea 1..10
        for (int n = 1; n <= 10; n++)
        {
            var t = Instantiate(tilePrefab, grid);
            t.Init(n, tileNormal, tileCorrect, tileError, OnTileClicked);
            tiles.Add(t);
        }
    }

    void ApplyFixedOrder()
    {
        // Ordena visualmente los hijos del grid según fixedOrder
        // (No cambia el value de cada tile; solo su posición)
        // Primero: mapa número -> tile
        var map = new Dictionary<int, NumberTile>(tiles.Count);
        foreach (var t in tiles) map[t.value] = t;

        // Poner en el orden deseado (set sibling indices en secuencia)
        int sibling = 0;
        for (int i = 0; i < fixedOrder.Length; i++)
        {
            int num = fixedOrder[i];
            if (map.TryGetValue(num, out var tile))
            {
                tile.transform.SetSiblingIndex(sibling++);
            }
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

            // ¿Completado?
            if (nextExpected > 10)
            {
                if (statusText) statusText.text = "¡Completado! ✅";
                StartCoroutine(DisableAllAfter(0.4f));
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

        // Todos a rojo y deshabilitados
        foreach (var t in tiles)
        {
            t.SetError();
            t.SetInteractable(false);
        }

        if (statusText) statusText.text = "¡Orden incorrecto! Reiniciando...";
        yield return new WaitForSeconds(errorFlashSeconds);

        // Volver al orden fijo y resetear
        ApplyFixedOrder();
        ResetState();
    }

    IEnumerator DisableAllAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var t in tiles) t.SetInteractable(false);
        // Aquí puedes cerrar la tarea o notificar éxito
        // gameObject.SetActive(false);
    }
}

