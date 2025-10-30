using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button), typeof(Image))]
public class AlertTile : MonoBehaviour
{
    public int clicksNeeded = 3;
    public float scaleFactor = 0.67f; // se reduce ~a 2/3 por click

    int clicks = 0;
    Button btn;
    RectTransform rt;

    public Action<AlertTile> onVanished; // callback al manager

    void Awake()
    {
        btn = GetComponent<Button>();
        rt = GetComponent<RectTransform>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        clicks++;
        if (clicks < clicksNeeded)
        {
            // achica
            rt.localScale *= scaleFactor;
        }
        else
        {
            // desaparece y avisa
            gameObject.SetActive(false);
            onVanished?.Invoke(this);
        }
    }

    public void ResetTile()
    {
        clicks = 0;
        rt.localScale = Vector3.one;
        gameObject.SetActive(true);
    }
}
