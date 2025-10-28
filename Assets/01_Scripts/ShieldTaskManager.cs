using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShieldTaskManager : MonoBehaviour
{
    [Header("Referencia")]
    public RectTransform board;          // PanelEscudos
    public HexTile tilePrefab;

    [Header("Layout")]
    public float tileSize = 120f;
    public float spacing = 6f;
    public int rings = 2;                // 0 = solo centro; 1 = centro + 1 anillo; 2 = centro + 2 anillos…

    [Header("Inicio aleatorio")]
    [Range(0f, 1f)] public float startActiveProbability = 0.55f; // % de hex que inician rojos

    [Header("UI de estado")]
    public Text statusText; // opcional

    List<HexTile> tiles = new();

    void Start()
    {
        GenerateHexBoard();
        RandomizeStates();
        UpdateStatus();
    }

    void GenerateHexBoard()
    {
        // Disposición hexagonal (offset coordinates “pointy top”)
        float w = tileSize;
        float h = Mathf.Sqrt(3f) / 2f * w; // altura efectiva de un hex pointy
        float xStep = w * 0.75f;           // solape horizontal
        float yStep = h;                   // vertical

        // Centro (q=0,r=0)
        CreateTile(Vector2.zero);

        // Anillos alrededor del centro
        for (int r = 1; r <= rings; r++)
        {
            // Empezamos en la “esquina” de la derecha
            int q = r, s = -r, t = 0;
            // 6 lados del anillo
            Vector2Int[] dirs = new[]
            {
                new Vector2Int(0,1),   // subir
                new Vector2Int(-1,1),  // arriba-izq
                new Vector2Int(-1,0),  // izq
                new Vector2Int(0,-1),  // abajo
                new Vector2Int(1,-1),  // abajo-der
                new Vector2Int(1,0)    // der
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
            // Pointy-top axial to pixel
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

        // Evitar el caso trivial: todos apagados al inicio
        if (tiles.All(x => !x.IsActive))
        {
            tiles[Random.Range(0, tiles.Count)].SetState(true);
        }
    }

    void OnTileInactive(HexTile t)
    {
        if (tiles.All(x => !x.IsActive))
        {
            // ¡Completado!
            foreach (var tile in tiles)
                tile.Hide();              // <- los vuelves invisibles

            if (statusText) statusText.text = "Completado ✅";

            // Si quieres, también puedes desactivar todo el tablero:
            // board.gameObject.SetActive(false);
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
        statusText.text = $"Encedidos: {restantes}";
    }
}
