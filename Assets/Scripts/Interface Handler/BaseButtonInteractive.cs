using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButtonInteractive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Config")]
    [SerializeField] protected Sprite defalutSprite;
    [SerializeField] protected Color textDefalutColor;
    [SerializeField] protected Sprite hoverSprite;
    [SerializeField] protected Color textHoverColor;

    [Header("Component References")]
    [SerializeField] protected Image buttonImage;
    [SerializeField] protected TextMeshProUGUI[] buttonsText;

    [Header("Custom")]
    [SerializeField] private GameObject iconNotification;
    [SerializeField] private JournalController journalController;

    [Header("Unity Event")]
    public UnityEvent OnMouseDown;
    [Space(3f)]
    public UnityEvent OnHoverEnter;
    [Space(3f)]
    public UnityEvent OnHoverExit;

    private void Start()
    {
        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        iconNotification.SetActive(journalController.IsHasUnreadNotes());
    }

    private void SetVisualImage(bool isHover)
    {
        if (isHover)
        {
            buttonImage.sprite = hoverSprite;

            if (buttonsText.Length != 0)
            {
                foreach (var item in buttonsText)
                {
                    item.color = textHoverColor;
                }
            }
        }
        else
        {
            buttonImage.sprite = defalutSprite;

            if (buttonsText.Length != 0)
            {
                foreach (var item in buttonsText)
                {
                    item.color = textDefalutColor;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown?.Invoke();

        iconNotification.SetActive(journalController.IsHasUnreadNotes());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke();
    }
}
