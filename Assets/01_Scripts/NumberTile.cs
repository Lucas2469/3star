using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Button), typeof(Image))]
public class NumberTile : MonoBehaviour
{
    [Header("Runtime")]
    public int value;           // número que representa (1..10)
    public Image bg;            // imagen de fondo
    public TMP_Text label;      // texto con el número

    // Sprites de estado
    Sprite spriteNormal, spriteCorrect, spriteError;

    Action<NumberTile> onClicked;
    Button btn;

    void Awake()
    {
        if (!bg) bg = GetComponent<Image>();
        if (!label) label = GetComponentInChildren<TMP_Text>(true);
        btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();      // ← SOLO lo controla el manager
        btn.onClick.AddListener(HandleClick);
    }

    public void Init(
        int number,
        Sprite normal, Sprite correct, Sprite error,
        Action<NumberTile> clickCb)
    {
        value = number;
        spriteNormal = normal;
        spriteCorrect = correct;
        spriteError = error;
        onClicked = clickCb;

        if (label) label.text = number.ToString();
        SetNormal();
        SetInteractable(true);
    }

    public void SetNormal() { if (bg) bg.sprite = spriteNormal; }
    public void SetCorrect() { if (bg) bg.sprite = spriteCorrect; }
    public void SetError() { if (bg) bg.sprite = spriteError; }

    public void SetInteractable(bool v) { if (btn) btn.interactable = v; }

    void HandleClick()
    {
        onClicked?.Invoke(this);
    }
}
