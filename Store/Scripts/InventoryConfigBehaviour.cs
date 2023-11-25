using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryConfigBehaviour : MonoBehaviour
{
    public UnityEvent buttonEvent;
    public InventoryData inventoryDataObj;
    public InventoryUIButtonBehaviour inventoryUIPrefab;
    public StoreUIButtonBehaviour storeUIPrefab;

    private void Start()
    {
        buttonEvent.Invoke();
    }
    
    private void AddItemsToUI<T>(List<T> items)
    {

        foreach (var item in items)
        {
            GameObject element = null;

            if (item is IInventoryItem { UsedOrPurchase: true })
            {
                element = Instantiate(inventoryUIPrefab.gameObject, transform);
            }

            if (item is IStoreItem { UsedOrPurchase: false } )
            {
                element = Instantiate(storeUIPrefab.gameObject, transform);
                var storeButton = element.GetComponent<StoreUIButtonBehaviour>();
                if (storeButton != null)
                {
                    storeButton.inventoryConfigBehaviour = this;
                    storeButton.inventoryDataObj = inventoryDataObj;
                }
            }

            ConfigureElement(element, item);

           
        }
    }
    
    private void ConfigureElement<T>(GameObject element, T item)
    {
        Vector3 toggelScaleFactor = Vector3.one * 4;
        Vector3 lableMoveFactor = new Vector3(0, -1.56f, -.1f);
        Vector3 toggelMoveFactor = new Vector3(2, .5f, 0);
        Vector3 buttonMoveFactor = new Vector3(0, 0, -.1f);

        if (item is IInventoryItem inventoryItem)
        {
            var elementData = element.GetComponent<InventoryUIButtonBehaviour>();
            if (elementData == null) return;
            elementData.ButtonObj.image.sprite = inventoryItem.PreviewArt;
            elementData.Label.text = inventoryItem.ThisName;
            elementData.ButtonObj.interactable = inventoryItem.UsedOrPurchase;
            elementData.InventoryItemObj = inventoryItem as InventoryItem;
            if(inventoryItem.GameActionObj != null)
                elementData.ButtonObj.onClick.AddListener(inventoryItem.Raise);
            else
            {
                elementData.ButtonObj.interactable = true;
            }

            elementData.Label.transform.position += lableMoveFactor;
            elementData.ButtonObj.transform.position += buttonMoveFactor;
        }

        if (item is not IStoreItem storeItem) return;
        {
            var elementData = element.GetComponent<StoreUIButtonBehaviour>();
            if (elementData == null) return;
            elementData.ButtonObj.image.sprite = storeItem.PreviewArt;
            elementData.Label.text = storeItem.ThisName;
            elementData.ButtonObj.interactable = !storeItem.UsedOrPurchase;
            elementData.StoreItemObj = storeItem;
            elementData.ToggleObj.isOn = storeItem.UsedOrPurchase;
            elementData.PriceLabel.text = $"${storeItem.Price}";
            elementData.cash = inventoryDataObj.cash;

            elementData.ToggleObj.transform.localScale = toggelScaleFactor;
            elementData.ToggleObj.transform.position += toggelMoveFactor;
            elementData.Label.transform.position += lableMoveFactor;
            elementData.ButtonObj.transform.position += buttonMoveFactor;
        }
    }
    
    public void AddAllInventoryItemsToUI()
    {
        foreach (var item in inventoryDataObj.inventoryDataObjList)
        {
            if (item is not { UsedOrPurchase: true }) continue;
            var element = Instantiate(inventoryUIPrefab.gameObject, transform);
            var elementData = element.GetComponent<InventoryUIButtonBehaviour>();
            elementData.ConfigButton(item);
        }
    }
    

    public void AddAllStoreInventoryItemsToUI()
    {
        foreach (var item in inventoryDataObj.storeDataObjList)
        {
            var element = Instantiate(inventoryUIPrefab.gameObject, transform);
            var elementData = element.GetComponent<StoreUIButtonBehaviour>();
            elementData.ConfigButton(item);
        }
    }

    public void AddPurchasedInventoryItemsPrefabsToScene()
    {
        var i = 0;
        foreach (var item in inventoryDataObj.storeDataObjList)
        {
            if (!item.UsedOrPurchase || item is not IInventoryItem storeItem ) continue;
            if (storeItem.GameActionObj == null || storeItem.GameArt == null);
            var element = Instantiate(storeItem.GameArt, transform);
            var elementData = element.GetComponent<InventoryPrefabItemBehaviour>();
            elementData.ConfigureGameObject(storeItem, i++);
        }
    }
}