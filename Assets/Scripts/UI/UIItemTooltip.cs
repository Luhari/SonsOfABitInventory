using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIItemTooltip : MonoBehaviour
    {
        [SerializeField]
        private UIItem m_item;
        [SerializeField]
        private GameObject m_nameLabel;
        [SerializeField]
        private GameObject m_weightLabel;
        [SerializeField]
        private GameObject m_marketValueLabel;
        [SerializeField]
        private GameObject m_deteriorationLevelLabel;
        [SerializeField]
        private GameObject m_dpsLabel;

        private Canvas m_canvas;

        private void Awake()
        {
            m_canvas = transform.root.GetComponent<Canvas>();
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                Vector2 position;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        (RectTransform)m_canvas.transform,
                        Input.mousePosition,
                        m_canvas.worldCamera,
                        out position
                    );
                transform.position = m_canvas.transform.TransformPoint(position + new Vector2(10, 0));
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
                m_item.SetItemImage(sprite);
                m_nameLabel.GetComponent<TMPro.TextMeshProUGUI>().text = name;
                SetStatLabel(m_weightLabel, weight.ToString());
                SetStatLabel(m_marketValueLabel, marketValue.ToString());

                m_deteriorationLevelLabel.SetActive(false);
            }
        }

        public void SetDeteriorationInfo(int deteriorationLevel)
        {
            m_deteriorationLevelLabel.SetActive(true);
            SetStatLabel(m_deteriorationLevelLabel, deteriorationLevel.ToString());
        }

        public void SetDPSInfo(float dps)
        {
            m_dpsLabel.SetActive(true);
            SetStatLabel(m_dpsLabel, dps.ToString());
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
            m_deteriorationLevelLabel.SetActive(false);
            m_dpsLabel.SetActive(false);
        }
    }
}
