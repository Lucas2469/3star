using UnityEngine;
using UnityEngine.UI;

public class FuseTaskController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject rightOff;       // GameObject (Image) del circuito apagado
    public GameObject rightOn;        // GameObject (Image) del circuito encendido
    public Image fuseImage;           // Image del botón del fusible
    public Sprite fuseOffSprite;      // sprite apagado
    public Sprite fuseOnSprite;       // sprite encendido

    [Header("Animación (opcional)")]
    public float crossfadeTime = 0.35f;

    bool isPowered = false;
    float tAnim = 0f;

    CanvasGroup offCg, onCg;

    void Awake()
    {
        // CanvasGroup para crossfade sin materiales
        offCg = rightOff.GetComponent<CanvasGroup>();
        if (!offCg) offCg = rightOff.AddComponent<CanvasGroup>();
        onCg = rightOn.GetComponent<CanvasGroup>();
        if (!onCg) onCg = rightOn.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        SetState(false, instant: true);
    }

    public void ToggleFuse()
    {
        SetState(!isPowered, instant: false);
    }

    void SetState(bool powered, bool instant)
    {
        isPowered = powered;

        fuseImage.sprite = powered ? fuseOnSprite : fuseOffSprite;

        rightOn.SetActive(true);  // lo activamos para poder animar alpha
        rightOff.SetActive(true);

        if (instant || crossfadeTime <= 0f)
        {
            onCg.alpha = powered ? 1f : 0f;
            offCg.alpha = powered ? 0f : 1f;
            rightOn.SetActive(powered);
            rightOff.SetActive(!powered);
            return;
        }

        // arrancar animación
        StopAllCoroutines();
        StartCoroutine(Crossfade(powered));
    }

    System.Collections.IEnumerator Crossfade(bool toPowered)
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
}
