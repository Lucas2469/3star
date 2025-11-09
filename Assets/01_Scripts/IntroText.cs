using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroText : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI titleText;       // Título "BLACK'S CUBE"
    public TextMeshProUGUI storyText;       // Texto de la historia
    public FadeController fadeController;

    [Header("Historia")]
    [TextArea(5, 10)]
    public string fullText;
    public float letterDelay = 0.05f;       // tiempo entre letras

    [Header("Audio")]
    public AudioSource bgMusic;
    public AudioSource typeSound;

    void Start()
    {
        storyText.gameObject.SetActive(false); // ocultar historia al inicio
        titleText.gameObject.SetActive(true);  // mostrar título

        if (bgMusic != null)
            bgMusic.Play();

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Fade in desde negro
        if (fadeController != null)
            yield return StartCoroutine(fadeController.FadeIn());

        // Mostrar título BLACK'S CUBE
        yield return StartCoroutine(ShowTitle());

        // Mostrar historia con efecto de escritura
        yield return StartCoroutine(ShowStory());

        // Fade out y cargar escena principal
        if (fadeController != null)
            yield return StartCoroutine(fadeController.FadeOut());

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowTitle()
    {
        titleText.gameObject.SetActive(true);
        Color titleColor = titleText.color;
        titleColor.a = 0;
        titleText.color = titleColor;

        // Fade in del título
        while (titleText.color.a < 1)
        {
            titleColor.a += Time.deltaTime;
            titleText.color = titleColor;
            yield return null;
        }

        yield return new WaitForSeconds(2f); // mantener visible

        // Fade out del título
        while (titleText.color.a > 0)
        {
            titleColor.a -= Time.deltaTime;
            titleText.color = titleColor;
            yield return null;
        }

        titleText.gameObject.SetActive(false);
    }

    IEnumerator ShowStory()
{
    storyText.gameObject.SetActive(true);
    storyText.text = "";

    Color storyColor = storyText.color;
    storyColor.a = 1f;
    storyText.color = storyColor;

    for (int i = 0; i < fullText.Length; i++)
    {
        storyText.text = fullText.Substring(0, i + 1);

        if (typeSound != null && fullText[i] != ' ')
            typeSound.Play();

        yield return new WaitForSeconds(letterDelay);
    }

    // Esperar unos segundos antes del fade final
    yield return new WaitForSeconds(5f);
}

}