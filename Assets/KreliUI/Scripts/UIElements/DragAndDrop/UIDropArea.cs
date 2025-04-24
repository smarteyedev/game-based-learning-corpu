using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace KreliStudio
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class UIDropArea : Selectable, IDropHandler
    {

        public UIDraggable item
        {
            get
            {
                return GetComponentInChildren<UIDraggable>();
            }
        }

        public UnityEvent onChanged = new UnityEvent();



        public virtual void OnDrop(PointerEventData eventData)
        {
            if (UIDraggable.draggedObject && interactable)
            {
                if (!item)
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDropArea", "OnDrop", "Dropped element to area.");
                    // call OnChange to UIDropArea from which UIDragable was pull out
                    UIDraggable.draggedObject.transform.parent.SendMessage("OnChange", SendMessageOptions.DontRequireReceiver);
                    // call OnChange to UIDragable
                    UIDraggable.draggedObject.OnChanged();
                    // call OnChange to this UIDropArea
                    OnChange();

                    // drop to UIDropArea
                    UIDraggable.draggedObject.transform.SetParent(transform);
                }
                else
                if (item.replaceable && item.interactable)
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDropArea", "OnDrop", "Dropped element to area and replace with " + item.name);
                    // call OnChange to UIDropArea from which UIDragable was pull out
                    UIDraggable.draggedObject.transform.parent.SendMessage("OnChange", SendMessageOptions.DontRequireReceiver);
                    // call OnChange to UIDragable
                    UIDraggable.draggedObject.OnChanged();
                    // call OnChange to this UIDropArea
                    OnChange();

                    // take item from this UIDropArea and replace it
                    item.transform.SetParent(UIDraggable.draggedObject.transform.parent);
                    // drop to UIDropArea
                    UIDraggable.draggedObject.transform.SetParent(transform);
                }
                else
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDropArea", "OnDrop", "Can not drop to this area because there is not interactable UIDraggable.");
            }
            else
            {
                if (!interactable)
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDropArea", "OnDrop", "Can not drop to this area because is not interactable.");
                if (UIDraggable.draggedObject == null)
                    UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDropArea", "OnDrop", "Can not drop to this area because dropped element do not have UIDraggable script.");
            }
        }

        public void OnChange()
        {
            try
            {
                UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIDropArea", "On Change", "Event invoke.");
                onChanged.Invoke();
            }
            catch (System.Exception exception)
            {
                Debug.LogError("Couldn't invoke event OnChanged in " + name + ". Error: " + exception.Message);
            }

        }

        }
}