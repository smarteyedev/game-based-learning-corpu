using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Smarteye.Manager.taufiq;
using System.Runtime.CompilerServices;
using TMPro;
using System.Linq;

public class ButtonStageHandler : Selectable
{
    [Header("Button Config")]
    [SerializeField] private int targetNextScene = 0;
    public bool isPopupOpen = false;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite nonactiveSprite;

    [Header("Tooltip Handler")]
    [SerializeField] private bool isTooltipActive;
    [SerializeField] private GameStage isButtonForStage;

    [Header("Component References")]
    [SerializeField] private Image btnImage;
    [SerializeField] private Button btnChangeScene;
    [SerializeField] private GameObject popupStageDetail;

    [Space(15f)]
    [SerializeField] private GameObject popupIVCAData;
    [SerializeField] private TextMeshProUGUI textIVCATitle;
    [SerializeField] private TextMeshProUGUI textIVCAResult;

    [Space(5f)]
    [SerializeField] private JournalController journalController;

    [Space(5f)]
    [SerializeField] private GameObject iconPlay;
    [SerializeField] private GameObject iconInspect;
    [SerializeField] private GameObject plankPlay;

    private GameManager m_gameManager;


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

        m_gameManager = GameManager.instance;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        onHoverEnter.Invoke();

        if (!isPopupOpen)
        {
            if (isTooltipActive) { TooltipController.instance.SetAndShowTooltip(m_gameManager.GenerateGameStageName(isButtonForStage)); }
        }

        if (interactable)
        {
            if (plankPlay.activeSelf)
                iconPlay.SetActive(true);
            else iconInspect.SetActive(true);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        onHoverExit.Invoke();

        if (isTooltipActive) { TooltipController.instance.HideTooltip(); }

        if (interactable)
        {
            if (plankPlay.activeSelf)
                iconPlay.SetActive(false);
            else iconInspect.SetActive(false);
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
        if (isActive)
        {
            isPopupOpen = true;

            if ((int)isButtonForStage < (int)m_gameManager.currentGameStage)
            {
                string[] ivcaData = m_gameManager.playerData.GetIVCAData();
                if (isButtonForStage == GameStage.IVCA)
                {
                    if (ivcaData == null)
                    {
                        popupStageDetail.SetActive(true);
                    }
                    else
                    {
                        popupIVCAData.SetActive(isActive);
                        textIVCATitle.text = ivcaData[0];
                        textIVCAResult.text = ivcaData[1];
                    }
                }
                else
                {
                    journalController.ShowSceneJournalPanelByStage(isButtonForStage);
                }
            }
            else
            {
                popupStageDetail.SetActive(true);
            }
        }
        else
        {
            isPopupOpen = false;

            if ((int)isButtonForStage < (int)m_gameManager.currentGameStage)
            {
                string[] ivcaData = m_gameManager.playerData.GetIVCAData();
                if (isButtonForStage == GameStage.IVCA)
                {
                    if (ivcaData == null)
                    {
                        popupStageDetail.SetActive(false);
                    }
                    else
                    {
                        popupIVCAData.SetActive(false);
                        textIVCATitle.text = ivcaData[0];
                        textIVCAResult.text = ivcaData[1];
                    }
                }
                else
                {
                    journalController.OnClickCloseJournalPanel();
                }
            }
            else
            {
                popupStageDetail.SetActive(false);
            }
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
