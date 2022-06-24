using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIItemTooltip : MonoBehaviour
    {
        private Canvas canvas;
        [SerializeField]
        private UIItem item;
        [SerializeField]
        private GameObject nameLabel;
        [SerializeField]
        private GameObject weightLabel;
        [SerializeField]
        private GameObject marketValueLabel;
        [SerializeField]
        private GameObject deteriorationLevelLabel;


        private void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                Vector2 position;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        (RectTransform)canvas.transform,
                        Input.mousePosition,
                        canvas.worldCamera,
                        out position
                    );
                transform.position = canvas.transform.TransformPoint(position + new Vector2(10, 0));
            }
        }

        /// <summary>
        /// Updates the tooltip with the correspondent info
        /// </summary>
        /// <param name="sprite">Item sprite</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="marketValue">Market Value of the item</param>
        public void SetInfo(Sprite sprite, string name, float weight, float marketValue) 
        {
            if (gameObject.activeSelf)
            {
                item.SetItemImage(sprite);
                nameLabel.GetComponent<TMPro.TextMeshProUGUI>().text = name;
                SetStatLabel(weightLabel, weight.ToString());
                SetStatLabel(marketValueLabel, marketValue.ToString());

                deteriorationLevelLabel.SetActive(false);
            }
        }

        public void SetDeteriorationInfo(int deteriorationLevel)
        {
            deteriorationLevelLabel.SetActive(true);
            SetStatLabel(deteriorationLevelLabel, deteriorationLevel.ToString());
        }

        private void SetStatLabel(GameObject label, string value)
        {
            label.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = value;
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
