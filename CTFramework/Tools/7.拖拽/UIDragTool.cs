using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CT.Tools
{
    public class UIDragTool : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTrans;
        private Image image;
        private CanvasGroup cg;

        private void Start()
        {
            rectTrans = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            cg = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = false;
            Debug.Log("Begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTrans.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = true;
            Debug.Log("End");
        }
    }
}

