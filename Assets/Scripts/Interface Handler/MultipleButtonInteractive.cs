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
    [SerializeField] private Sprite defalutSprite;
    [SerializeField] private Color textDefalutColor;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Color textHoverColor;

    [Header("Component References")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI[] buttonsText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Unity Event")]
    public UnityEvent OnMouseDown;
    public UnityEvent OnHoverEnter;
    public UnityEvent OnHoverExit;

    private bool m_isSelected = false;

    private void OnEnable()
    {
        SetVisualImage(false);
        m_isSelected = false;
        SetAlpha(1);
    }

    private void OnDisable()
    {
        OnMouseDown.RemoveAllListeners();
        m_isSelected = false;
        SetAlpha(1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_isSelected && canvasGroup.alpha == 1)
        {
            OnMouseDown?.Invoke();
            m_isSelected = true;
            SetVisualImage(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_isSelected && canvasGroup.alpha == 1)
        {
            OnHoverEnter?.Invoke();
            SetVisualImage(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_isSelected && canvasGroup.alpha == 1)
        {
            OnHoverExit?.Invoke();
            SetVisualImage(false);
        }
    }

    private void Start()
    {
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }

        /* if (buttonsText.Length == 0)
        {
            TextMeshProUGUI[] b = GetComponentsInChildren<TextMeshProUGUI>();

            buttonsText = new TextMeshProUGUI[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                buttonsText[i] = b[i];
            }
        } */

        SetVisualImage(false);
        m_isSelected = false;
        SetAlpha(1);
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

    public void SetOptionText(string _txt)
    {
        TextMeshProUGUI tComp = buttonsText.First((x) => x.gameObject.name == "OptionText");
        // TextMeshProUGUI tComp = buttonsText[1];
        tComp.text = _txt;

        // Debug.Log($"{tComp.gameObject.name} : {_txt}");
    }

    public void SetAlpha(float _val)
    {
        canvasGroup.alpha = _val;
    }
}
