using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonStageHandler : Selectable
{
    [Space(5f)]
    [Header("Unity Event")]

    // Event Unity untuk hover
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    // Event Unity untuk klik
    public UnityEvent onClickCustom;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        onHoverEnter.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        onHoverExit.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        onClickCustom.Invoke();
    }
}
