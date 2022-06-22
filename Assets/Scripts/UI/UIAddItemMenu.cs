using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAddItemMenu : MonoBehaviour
{

    public delegate void OnAddItemDelegate(Item item);
    public OnAddItemDelegate OnAddItem;

    [SerializeField]
    private GameObject addButtonPrefab;
    [SerializeField]
    private ItemFactory itemFactory;

    private void Awake()
    {
        itemFactory.OnDatabaseBuilt += BuildAddItemsMenu;
    }

    private void BuildAddItemsMenu(List<Item> database)
    {
        foreach (Item item in database)
        {
            GameObject instance = Instantiate(addButtonPrefab);
            instance.transform.SetParent(gameObject.transform);

            Image image = instance.GetComponentsInChildren<Image>()[1];
            image.sprite = item.texture;

            var button = instance.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                OnAddItem(item);
            });
        }
    }
}
