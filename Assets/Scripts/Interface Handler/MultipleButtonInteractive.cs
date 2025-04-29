using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class MultipleButtonInteractive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Config")]
    public Sprite defalutSprite;
    public Color textDefalutColor;
    public Sprite hoverSprite;
    public Color textHoverColor;

    [Header("Component References")]
    public Image buttonImage;
    public TextMeshProUGUI[] buttonsText;

    [Header("Unity Event")]
    public UnityEvent OnDown;
    public UnityEvent OnHover;
    public UnityEvent OnExit;

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetVisualImage(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetVisualImage(false);
    }

    private void Start()
    {
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        if (buttonsText.Length == 0)
        {
            TextMeshProUGUI[] b = GetComponentsInChildren<TextMeshProUGUI>();

            buttonsText = new TextMeshProUGUI[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                buttonsText[i] = b[i];
            }
        }

        SetVisualImage(false);
    }

    private void SetVisualImage(bool isHover)
    {
        if (isHover)
        {
            buttonImage.sprite = hoverSprite;

            foreach (var item in buttonsText)
            {
                item.color = textHoverColor;
            }
        }
        else
        {
            buttonImage.sprite = defalutSprite;
            foreach (var item in buttonsText)
            {
                item.color = textDefalutColor;
            }
        }
    }
}
