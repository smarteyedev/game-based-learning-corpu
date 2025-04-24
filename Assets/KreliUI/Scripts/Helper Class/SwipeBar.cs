using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KreliStudio
{
    [RequireComponent(typeof(Image))]
    public class SwipeBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public UISidePanel sidePanel;

        private Vector2 offsetDrag; // offset from center of swipeBar to begin drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "SwipeBar", "OnBeginDrag", "Begin drag swipe area.");
            offsetDrag = GetComponent<RectTransform>().anchoredPosition - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 dragVector=Vector2.zero;
            switch (sidePanel.side)
            {
                case 0: // Left
                    dragVector.x = eventData.position.x + offsetDrag.x;
                    if (!sidePanel.isElastic)
                    {
                        if (dragVector.x > sidePanel.panel.rect.width)
                            dragVector.x = sidePanel.panel.rect.width;
                        if (dragVector.x < 0)
                            dragVector.x = 0;
                    }
                        break;
                case 1: // Top
                    dragVector.y = eventData.position.y + offsetDrag.y;
                    if (!sidePanel.isElastic)
                    {
                        if (dragVector.y < -sidePanel.panel.rect.height)
                            dragVector.y = -sidePanel.panel.rect.height;
                        if (dragVector.y > 0)
                            dragVector.y = 0;
                    }
                    break;
                case 2: // Right
                    dragVector.x = eventData.position.x + offsetDrag.x;
                    if (!sidePanel.isElastic)
                    {
                        if (dragVector.x < -sidePanel.panel.rect.width)
                            dragVector.x = -sidePanel.panel.rect.width;
                        if (dragVector.x > 0)
                            dragVector.x = 0;
                    }
                    break;
                case 3: // Bottom
                    dragVector.y = eventData.position.y + offsetDrag.y;
                    if (!sidePanel.isElastic)
                    {
                        if (dragVector.y > sidePanel.panel.rect.height)
                            dragVector.y = sidePanel.panel.rect.height;
                        if (dragVector.y < 0)
                            dragVector.y = 0;
                    }
                    break;
            }
            
            GetComponent<RectTransform>().anchoredPosition = dragVector;
            sidePanel.panel.anchoredPosition = dragVector;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UIDebug.PrintDebug(UIDebug.DebugType.UI, transform, "SwipeBar", "OnEndDrag", "End drag swipe area.");
            sidePanel.OnEndDragSwipeBar();
        }
    }

}