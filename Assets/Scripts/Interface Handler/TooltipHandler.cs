using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHandler : MonoBehaviour
{
    [Header("Tooltip Handler")]
    public string tooltipMessage;

    public void OnMouseEnter()
    {
        TooltipController.instance.SetAndShowTooltip(tooltipMessage);
    }

    public void OnMouseExit()
    {
        TooltipController.instance.HideTooltip();
    }
}
