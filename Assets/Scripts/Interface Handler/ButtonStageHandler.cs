using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class ButtonStageHandler : Selectable
{
    [Header("Button Config")]
    [SerializeField] private int targetNextScene = 0;
    public bool isPopupOpen = false;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite nonactiveSprite;

    [Header("Tooltip Handler")]
    [SerializeField] private bool isTooltipActive;
    [SerializeField] private string tooltipMessage;

    [Header("Component References")]
    [SerializeField] private Image btnImage;
    [SerializeField] private GameObject popupStageDetail;
    [SerializeField] private Button btnChangeScene;

    [Space(2f)]
    [SerializeField] private GameObject iconPlay;
    [SerializeField] private GameObject plankPlay;


    [Space(5f)]
    [Header("Unity Event")]

    // Event Unity untuk hover
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    // Event Unity untuk klik
    public UnityEvent onClickCustom;

    private new void Start()
    {
        if (btnImage == null) GetComponent<Image>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        onHoverEnter.Invoke();

        if (!isPopupOpen)
        {
            if (isTooltipActive) { TooltipController.instance.SetAndShowTooltip(tooltipMessage); }
        }

        if (interactable)
        {
            iconPlay.SetActive(true);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        onHoverExit.Invoke();

        if (isTooltipActive) { TooltipController.instance.HideTooltip(); }

        if (interactable)
        {
            iconPlay.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        onClickCustom.Invoke();

        if (isTooltipActive) { TooltipController.instance.HideTooltip(); }

        if (interactable)
        {
            iconPlay.SetActive(false);
        }
    }

    public void SetActiveButton(bool isActive)
    {
        if (isActive)
        {
            btnImage.sprite = activeSprite;
        }
        else
        {
            btnImage.sprite = nonactiveSprite;
            // interactable = false;
        }
    }

    public void OpenPopupDetail(bool isActive)
    {
        if (interactable)
        {
            // iconPlay.SetActive(true);
            isPopupOpen = isActive;
            popupStageDetail.SetActive(isActive);
        }
    }

    public void SetupBtnChangeScene(bool isActive, UnityAction<int> changeSceneFunc = null)
    {
        if (isActive)
        {
            btnChangeScene.interactable = true;
            // btnChangeScene.GetComponentInChildren<TextMeshProUGUI>().text = $"Kerjakan";
            if (changeSceneFunc != null)
                btnChangeScene.onClick.AddListener(() => changeSceneFunc(targetNextScene));

            plankPlay.SetActive(true);
        }
        else
        {
            btnChangeScene.gameObject.SetActive(false);
        }
    }
}
