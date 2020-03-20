using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Handles the shopping list items, creation of a shopping list, updating of cart list and shopping list
/// </summary>
public class ShoppingCart : MonoBehaviour
{
    int minItemsToShop = 1;
    int maxItemsPossibleOnList = 6;
    int maxCartCapacity = 8;
    int totalItemPrefabs {  get { return GameManager.Instance.AvailablePrefabCount; } }
    bool FixedItemAmount { get { return GameManager.Instance.FixedAmountInShopList; } }
    public int curEntries { get; private set; }

    Dictionary<eItemType, int> requiredItems;
    Dictionary<eItemType, int> itemsInCart;

    public event Action<string[]> CreateShoppingList = delegate { };
    public event Action<Dictionary<int, bool>> StrikeItems = delegate { };

    private void Awake()
    {
        requiredItems = new Dictionary<eItemType, int>();
        itemsInCart = new Dictionary<eItemType, int>();
        minItemsToShop = UnityEngine.Random.Range(1, 5);
    }

    private void Start()
    {
        print("Count in list: " + FixedItemAmount);
        ItemCollider.ItemLocationChange += UpdateItemsInCart;
        ItemSpawn.ItemSpawnFinished += SetShoppingList;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            string allitems = "All Items on List:\n";

            foreach (KeyValuePair<eItemType, int> pair in requiredItems)
            {
                allitems += pair.Value + "x " + pair.Key + "\n";
            }
            if (requiredItems.Count == 0)
                allitems += "None found :(";

            print(allitems);
        }
    }

    void SetShoppingList()
    {
        int maxItemsOnList = !FixedItemAmount ? maxItemsPossibleOnList : maxCartCapacity + 1;        
        //no of items in TOTAL to shop (including doubles if !noFixedAmount)
        int itemsToShop = UnityEngine.Random.Range(minItemsToShop, maxItemsOnList);
        itemsToShop = itemsToShop > totalItemPrefabs ? totalItemPrefabs - 1 : itemsToShop;

        while (itemsToShop > 0)
        {
            eItemType newKey = ItemManager.Instance.GetRandomFromAllItems();
            int amount = !FixedItemAmount ? 1 : UnityEngine.Random.Range(1, 4);

            //is there still room in the shopping list
            if (requiredItems.Count < maxItemsPossibleOnList)
            {
                if (requiredItems.ContainsKey(newKey))
                    requiredItems[newKey] += !FixedItemAmount ? 0 : amount;
                else
                    requiredItems.Add(newKey, amount);
                
                itemsToShop -= amount;
            }
            else
            {
                if (requiredItems.ContainsKey(newKey) && FixedItemAmount)
                {
                    requiredItems[newKey] += amount;
                    itemsToShop -= amount;
                }
            }
        }
        curEntries = requiredItems.Count;
        CreateShoppingList(WriteShoppingList());
    }

    void UpdateItemsInCart(Item i, eItemLocation l)
    {
        Dictionary<eItemType,int> _tempItemsInCart = ItemManager.Instance.SortByItemType(ItemManager.Instance.AllItemsInCart());
        print(_tempItemsInCart.Count + " Items in temp Cart");
        itemsInCart = _tempItemsInCart;
        print(itemsInCart.Count + " Items in Cart");
        CompareCartToShoppingList();        
    }

    public string[] WriteShoppingList()
    {
        string[] _lines = new string[curEntries];

        for (int i = 0; i < requiredItems.Count; i++)
        {
            eItemType item = requiredItems.ElementAt(i).Key;
            int count = requiredItems.ElementAt(i).Value;
            _lines[i] = !FixedItemAmount ? item.ToString() : count.ToString() + "x " + item.ToString();
        }
        return _lines;
    }

    public void CompareCartToShoppingList()
    {
        Dictionary<int, bool> rowsToTickOff = new Dictionary<int, bool>();

        for (int i = 0; i < curEntries; i++)
        {
            var searchKey = requiredItems.ElementAt(i).Key;
            bool gotItem = false;

            if (itemsInCart.ContainsKey(searchKey))
                if (FixedItemAmount)
                    gotItem = true;
                else
                    gotItem = itemsInCart[searchKey] >= requiredItems[searchKey] ? true : false;
                    
            rowsToTickOff.Add(i, gotItem);
            //print("no " + i + " is " + gotItem);
        }
        StrikeItems(rowsToTickOff);
    }
}
