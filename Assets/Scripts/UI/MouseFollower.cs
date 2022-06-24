using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        private Canvas canvas;
        private Image item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            item = GetComponentsInChildren<Image>()[1];
        }

        public void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
                );
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void SetImage(Sprite sprite)
        {
            if (item == null)
            {
                item = GetComponentsInChildren<Image>()[1];
            }

            item.sprite = sprite;
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
