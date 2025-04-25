
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipController : MonoBehaviour, IPointerEnterHandler
{
    public static TooltipController instance;

    public TextMeshProUGUI tooltipText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
            transform.position = Input.mousePosition;
    }

    public void SetAndShowTooltip(string message)
    {
        gameObject.SetActive(true);
        tooltipText.text = message;
        transform.position = Input.mousePosition;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
        transform.position = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        Debug.Log($"pointer enter..");
    }
}
