using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private const string VolumeKey = "MusicVolume";

    private void Awake()
    {
        // Singleton (una sola instancia)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Cargar volumen guardado o usar 0.5 por defecto
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat(VolumeKey, volume);
            PlayerPrefs.Save(); // Guarda inmediatamente
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 0.5f;
    }
}
