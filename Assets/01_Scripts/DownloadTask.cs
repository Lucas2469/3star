using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AmongUsDownload
{
    public class DownloadTask : MonoBehaviour
    {
        [Header("UI References")]
        public Button downloadButton;
        public Image progressFill;
        public GameObject transferBox;
        public Text percentText;
        public Text hintText;
        public GameObject doneBox;

        [Header("Settings")]
        public float durationSeconds = 6.5f;
        public bool autoStart = false;

        public UnityEvent onDownloadStarted;
        public UnityEvent onDownloadFinished;

        float t;
        bool running;

        void Start()
        {
            ResetUI();
            if (autoStart) StartDownload();
        }

        void ResetUI()
        {
            t = 0f;
            running = false;
            if (progressFill) progressFill.fillAmount = 0f;
            if (percentText) percentText.text = "0%";
            if (transferBox) transferBox.SetActive(false);
            if (doneBox) doneBox.SetActive(false);

            if (downloadButton)
            {
                downloadButton.interactable = true;
                downloadButton.onClick.RemoveAllListeners();
                downloadButton.onClick.AddListener(StartDownload);
            }
        }

        public void StartDownload()
        {
            if (running) return;
            running = true;
            t = 0f;
            if (transferBox) transferBox.SetActive(true);
            if (doneBox) doneBox.SetActive(false);
            if (downloadButton) downloadButton.interactable = false;
            if (hintText) hintText.text = "Transfiriendo datos...";
            onDownloadStarted?.Invoke();
        }

        void Update()
        {
            if (!running) return;
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / Mathf.Max(0.01f, durationSeconds));
            if (progressFill) progressFill.fillAmount = p;
            if (percentText) percentText.text = Mathf.FloorToInt(p * 100f) + "%";
            if (p >= 1f) Finish();
        }

        void Finish()
        {
            running = false;
            if (doneBox) doneBox.SetActive(true);
            if (downloadButton) downloadButton.interactable = true;
            onDownloadFinished?.Invoke();
        }
    }
}
