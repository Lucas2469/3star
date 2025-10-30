using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DownloadTaskUI : MonoBehaviour
{
    [Header("UI")]
    public Button btnDownload;
    public GameObject progressBG;
    public Image progressFill;      // Image (Filled)
    public TMP_Text statusText;

    [Header("Config")]
    public float totalSeconds = 5f;

    Coroutine running;

    void Awake()
    {
        // Estado inicial
        progressBG.SetActive(false);
        if (progressFill) progressFill.fillAmount = 0f;
        if (statusText) statusText.text = "";
        btnDownload.onClick.RemoveAllListeners();
        btnDownload.onClick.AddListener(StartDownload);
    }

    void StartDownload()
    {
        if (running != null) return;
        running = StartCoroutine(CoDownload());
    }

    IEnumerator CoDownload()
    {
        btnDownload.interactable = false;
        progressBG.SetActive(true);
        if (statusText) statusText.text = "Downloading...";

        float t = 0f;
        while (t < totalSeconds)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / totalSeconds);
            if (progressFill) progressFill.fillAmount = p;
            yield return null;
        }

        if (statusText) statusText.text = "Download complete!";
        // Aquí puedes notificar al sistema de tareas y cerrar el panel:
        // yield return new WaitForSeconds(0.5f);
        // gameObject.SetActive(false);

        running = null;
    }

    // Si quieres reiniciar manualmente desde el editor
    [ContextMenu("Reset UI")]
    public void ResetUI()
    {
        if (running != null) StopCoroutine(running);
        running = null;
        if (statusText) statusText.text = "";
        if (progressFill) progressFill.fillAmount = 0f;
        progressBG.SetActive(false);
        btnDownload.interactable = true;
    }
}
