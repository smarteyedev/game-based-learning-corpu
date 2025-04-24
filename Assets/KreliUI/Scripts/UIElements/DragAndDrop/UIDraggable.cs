using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace KreliStudio
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDraggable : Selectable, IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        // dragged object - actual in hand
        public static UIDraggable draggedObject;


        // UIDragable properties
        public bool replaceable = false;
        public UnityEvent onDragged = new UnityEvent();
        public UnityEvent onDropped = new UnityEvent();
        public UnityEvent onChanged = new UnityEvent();


        // position before drag
        private Vector3 startPosition;
        private Transform startParent;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (interactable)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDraggable", "OnBeginDrag", "Begin drag.");
                GetComponent<Canvas>().overrideSorting = true;
                GetComponent<Canvas>().sortingOrder++;
                draggedObject = this;
                startPosition = transform.position;
                startParent = transform.parent;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                if (onDragged != null)
                {
                    try
                    {
                        UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIDraggable", "On Dragged", "Event invoke.");
                        onDragged.Invoke();
                    }
                    catch (System.Exception exception)
                    {
                        Debug.LogError("Couldn't invoke event OnDragged in " + name + ". Error: " + exception.Message);
                    }
                }
            }
            else
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDraggable", "OnBeginDrag", "Can not begin drag because element is not interactable.");
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (interactable)
            {
                transform.position = eventData.position;
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (interactable)
            {
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDraggable", "OnEndDrag", "End drag.");
                GetComponent<Canvas>().overrideSorting = false;
                GetComponent<Canvas>().sortingOrder--;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                draggedObject = null;
                if (transform.parent == startParent)
                {
                    transform.position = startPosition;
                }

                if (onDropped != null)
                {
                    try
                    {
                        UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIDraggable", "On Dropped", "Event invoke.");
                        onDropped.Invoke();
                    }
                    catch (System.Exception exception)
                    {
                        Debug.LogError("Couldn't invoke event OnDropped in " + name + ". Error: " + exception.Message);
                    }
                }
            }
            else
                UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "UIDraggable", "OnEndDrag", "Can not end drag because element is not interactable.");
        }

        public void OnChanged()
        {
            if (onChanged != null)
            {
                try
                {
                    UIDebug.PrintDebug(UIDebug.DebugType.EVENT, transform, "UIDraggable", "On Changed", "Event invoke.");
                    onChanged.Invoke();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError("Couldn't invoke event OnChanged in " + name + ". Error: " + exception.Message);
                }
            }
        }
    }

}