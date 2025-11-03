using UnityEngine;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            // Sincronizar el valor del slider con el volumen real
            float currentVolume = MusicManager.Instance.GetVolume();
            volumeSlider.value = currentVolume;

            // Suscribirse a cambios del slider
            volumeSlider.onValueChanged.AddListener(MusicManager.Instance.SetVolume);
        }
    }

    private void OnDestroy()
    {
        if (volumeSlider != null && MusicManager.Instance != null)
        {
            volumeSlider.onValueChanged.RemoveListener(MusicManager.Instance.SetVolume);
        }
    }
}
