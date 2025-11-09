using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class FuseTaskControllerWithReturn : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject rightOff;
    public GameObject rightOn;
    public Image fuseImage;
    public Sprite fuseOffSprite;
    public Sprite fuseOnSprite;
    public TMP_Text statusText;  // texto para mensaje completado

    [Header("Animación (opcional)")]
    public float crossfadeTime = 0.35f;

    [Header("Mapa")]
    public string escenaMapa = "Mapa";
    public float tiempoMensaje = 2f;

    bool isPowered = false;
    float tAnim = 0f;
    bool completed = false;

    CanvasGroup offCg, onCg;

    void Awake()
    {
        offCg = rightOff.GetComponent<CanvasGroup>();
        if (!offCg) offCg = rightOff.AddComponent<CanvasGroup>();
        onCg = rightOn.GetComponent<CanvasGroup>();
        if (!onCg) onCg = rightOn.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        // Mostrar cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetState(false, instant: true);
    }

    public void ToggleFuse()
    {
        if (completed) return;

        SetState(!isPowered, instant: false);

        if (isPowered && !completed)
        {
            completed = true;
            if (statusText) 
            {
                statusText.text = "¡Tarea completada!";
            }
            StartCoroutine(VolverAMapaConDelay());
        }
    }

    void SetState(bool powered, bool instant)
    {
        isPowered = powered;

        fuseImage.sprite = powered ? fuseOnSprite : fuseOffSprite;

        rightOn.SetActive(true);
        rightOff.SetActive(true);

        if (instant || crossfadeTime <= 0f)
        {
            onCg.alpha = powered ? 1f : 0f;
            offCg.alpha = powered ? 0f : 1f;
            rightOn.SetActive(powered);
            rightOff.SetActive(!powered);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(Crossfade(powered));
    }

    IEnumerator Crossfade(bool toPowered)
    {
        tAnim = 0f;
        float startOn = onCg.alpha;
        float startOff = offCg.alpha;
        float endOn = toPowered ? 1f : 0f;
        float endOff = toPowered ? 0f : 1f;

        while (tAnim < crossfadeTime)
        {
            tAnim += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(tAnim / crossfadeTime);
            onCg.alpha = Mathf.Lerp(startOn, endOn, k);
            offCg.alpha = Mathf.Lerp(startOff, endOff, k);
            yield return null;
        }

        onCg.alpha = endOn;
        offCg.alpha = endOff;

        rightOn.SetActive(toPowered);
        rightOff.SetActive(!toPowered);
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
