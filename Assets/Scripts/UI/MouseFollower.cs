using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        private Canvas m_canvas;
        private Image m_item;

        public void Awake()
        {
            m_canvas = transform.root.GetComponent<Canvas>();
            m_item = GetComponentsInChildren<Image>()[1];
        }

        public void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               (RectTransform)m_canvas.transform,
                Input.mousePosition,
                m_canvas.worldCamera,
                out position
                );
            transform.position = m_canvas.transform.TransformPoint(position);
        }

        public void SetImage(Sprite sprite)
        {
            if (m_item == null)
            {
                m_item = GetComponentsInChildren<Image>()[1];
            }

            m_item.sprite = sprite;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
