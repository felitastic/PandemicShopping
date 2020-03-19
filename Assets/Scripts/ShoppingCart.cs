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
    int minItemsToShop = 2;
    int maxItemsToShop = 6;
    int maxCartCapacity = 8;
    bool noFixedItemAmount { get { return GameManager.Instance.NoCountInShoppingList; } }
    public int curEntries { get; private set; }

    Dictionary<eItemType, int> requiredItems;
    Dictionary<eItemType, int> itemsInCart;

    public event Action<string[]> CreateShoppingList = delegate { };
    public event Action<Dictionary<int, bool>> StrikeItems = delegate { };

    private void Awake()
    {
        requiredItems = new Dictionary<eItemType, int>();
        itemsInCart = new Dictionary<eItemType, int>();
    }

    private void Start()
    {
        ItemCollider.ItemLocationChange += UpdateItemsInCart;
        ItemSpawn.ItemSpawnFinished += SetShoppingList;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            string allitems = "All Items in shopping list:\n";

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
        int maxItems = noFixedItemAmount ? maxItemsToShop + 1 : ItemManager.Instance.AllItemsByType.Count;        
        int itemsToShop = UnityEngine.Random.Range(minItemsToShop, maxItems);

        while (itemsToShop > 0)
        {
            eItemType newKey = ItemManager.Instance.GetRandomFromAllItems();
            int amount = noFixedItemAmount ? 1 : UnityEngine.Random.Range(1, 4);

            if (requiredItems.Count < maxItemsToShop)
            {
                if (requiredItems.ContainsKey(newKey))
                    requiredItems[newKey] += noFixedItemAmount ? 0 : amount;
                else
                    requiredItems.Add(newKey, amount);
                
                itemsToShop -= amount;
            }
            else
            {
                if (requiredItems.ContainsKey(newKey) && !noFixedItemAmount)
                {
                    requiredItems[newKey] += 0;
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
        if (itemsInCart.Equals(_tempItemsInCart))
            return;
        else
        {
            itemsInCart = _tempItemsInCart;
            CompareCartToShoppingList();
        }
    }

    public string[] WriteShoppingList()
    {
        string[] _lines = new string[curEntries];

        for (int i = 0; i < requiredItems.Count; i++)
        {
            eItemType item = requiredItems.ElementAt(i).Key;
            int count = requiredItems.ElementAt(i).Value;
            _lines[i] = noFixedItemAmount ? item.ToString() : count.ToString() + "x " + item.ToString();
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
                gotItem = itemsInCart[searchKey] >= requiredItems[searchKey] ? true : false; 
                    
            rowsToTickOff.Add(i, gotItem);
            //print("no " + i + " is " + gotItem);
        }
        StrikeItems(rowsToTickOff);
    }
}
