using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class MouseFollower : MonoBehaviour
    {
        private Canvas canvas;
        private UIItem item;

        void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            item = GetComponentInChildren<UIItem>();
        }

        private void Update()
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
            item.SetItemImage(sprite);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
