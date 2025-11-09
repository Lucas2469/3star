using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;


public class ShieldTaskManagerWithReturn : MonoBehaviour
{
    [Header("Referencia")]
    public RectTransform board;
    public HexTile tilePrefab;

    [Header("Layout")]
    public float tileSize = 120f;
    public float spacing = 6f;
    public int rings = 2;

    [Header("Inicio aleatorio")]
    [Range(0f, 1f)] public float startActiveProbability = 0.55f;

    [Header("UI de estado")]
    public TextMeshProUGUI statusText;

    [Header("Mapa")]
    public string escenaMapa = "Mapa";
    public float tiempoMensaje = 2f;

    List<HexTile> tiles = new();

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GenerateHexBoard();
        RandomizeStates();
        UpdateStatus();
    }

    void GenerateHexBoard()
    {
        float w = tileSize;
        float h = Mathf.Sqrt(3f) / 2f * w;
        float xStep = w * 0.75f;
        float yStep = h;

        CreateTile(Vector2.zero);

        for (int r = 1; r <= rings; r++)
        {
            int q = r, s = -r;
            Vector2Int[] dirs = new[]
            {
                new Vector2Int(0,1), 
                new Vector2Int(-1,1),
                new Vector2Int(-1,0),
                new Vector2Int(0,-1),
                new Vector2Int(1,-1),
                new Vector2Int(1,0)
            };

            for (int side = 0; side < 6; side++)
            {
                for (int step = 0; step < r; step++)
                {
                    Vector2 pos = AxialToXY(q, s, w, h, xStep, yStep);
                    CreateTile(pos);
                    q += dirs[side].x;
                    s += dirs[side].y;
                }
            }
        }

        Vector2 AxialToXY(int q, int r2, float W, float H, float Xs, float Ys)
        {
            float x = (q * Xs);
            float y = (r2 * Ys) + (q * Ys * 0.5f);
            return new Vector2(x, y);
        }
    }

    void CreateTile(Vector2 localPos)
    {
        var t = Instantiate(tilePrefab, board);
        var rt = t.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(tileSize, tileSize);
        rt.anchoredPosition = localPos;
        t.OnBecameInactive += OnTileInactive;
        tiles.Add(t);
    }

    void RandomizeStates()
    {
        foreach (var t in tiles)
        {
            bool active = Random.value < startActiveProbability;
            t.SetState(active);
        }

        if (tiles.All(x => !x.IsActive))
        {
            tiles[Random.Range(0, tiles.Count)].SetState(true);
        }
    }

    void OnTileInactive(HexTile t)
    {
        if (tiles.All(x => !x.IsActive))
        {
            foreach (var tile in tiles)
                tile.Hide();

            if (statusText) statusText.text = "¡Tarea completada! ✅";

            StartCoroutine(VolverAMapaConDelay());
        }
        else
        {
            UpdateStatus();
        }
    }

    void UpdateStatus()
    {
        if (!statusText) return;
        int restantes = tiles.Count(x => x.IsActive);
        statusText.text = $"Encendidos: {restantes}";
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
