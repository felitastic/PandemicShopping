﻿using System;
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
    bool gotEverything;
    bool noChange;
    int totalItemPrefabs { get { return GameManager.Instance.AvailablePrefabCount; } }
    bool FixedItemAmount { get { return GameManager.Instance.FixedAmountInShopList; } }
    public int curEntries { get; private set; }

    Dictionary<eItemType, int> requiredItems;
    Dictionary<eItemType, int> itemsInCart;
    Dictionary<eItemType, int> shopListGUI;

    public event Action<string[]> CreateShoppingList = delegate { };
    public event Action<int, bool> UpdateShoppingList = delegate { };

    private void Awake()
    {
        gotEverything = false;
        noChange = false;
        requiredItems = new Dictionary<eItemType, int>();
        itemsInCart = new Dictionary<eItemType, int>();
        shopListGUI = new Dictionary<eItemType, int>();
        minItemsToShop = UnityEngine.Random.Range(1, 5);
    }

    private void Start()
    {
        print("Count in list: " + FixedItemAmount);
        ItemManager.CartContentChanged += ItemContentInCartChanged;
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

    string[] WriteShoppingList()
    {
        string[] _lines = new string[curEntries];

        for (int i = 0; i < requiredItems.Count; i++)
        {
            eItemType item = requiredItems.ElementAt(i).Key;
            int count = requiredItems.ElementAt(i).Value;
            _lines[i] = !FixedItemAmount ? item.ToString() : count.ToString() + "x " + item.ToString();
            shopListGUI.Add(item, i);
        }
        return _lines;
    }

    void ItemContentInCartChanged(Item item, bool wasAdded)
    {
        if (wasAdded && gotEverything)
            return;

        //Check if it is on the required list 
        if (IsItemOnShoppingList(item.Type))
        {
            if (wasAdded)
            {
                print("yay, we gained an item");
                AddToCart(item.Type);

                if (!CanItemBeStrikedFromList(item.Type))
                    return;

                UpdateShoppingList(shopListGUI[item.Type], true);
                gotEverything = GotAllItems();

            }
            else if (!wasAdded)
            {
                print("Oh no, we lost an item");

                //remove from cart
                RemoveFromCart(item.Type);

                //check if amount in cart is still ok, otherwise change ui
                if (!CanItemBeStrikedFromList(item.Type))
                {
                    UpdateShoppingList(shopListGUI[item.Type], false);
                    gotEverything = false;
                }
            }
        }
    }

    bool GotAllItems()
    {
        foreach(var pair in requiredItems)
        {
            if (!CanItemBeStrikedFromList(pair.Key))
                return false;
        }
        return true;
    }

    bool CanItemBeStrikedFromList(eItemType item)
    {
        if (!itemsInCart.ContainsKey(item))
            return false;

        return itemsInCart[item] == requiredItems[item] ? true : false;
    }

    bool IsItemOnShoppingList(eItemType item)
    {
        return requiredItems.ContainsKey(item);
    }

    void AddToCart(eItemType newItem)
    {
        if (!itemsInCart.ContainsKey(newItem))
            itemsInCart.Add(newItem, 1);
        else
            itemsInCart[newItem] += 1;
    }

    void RemoveFromCart(eItemType itemToRemove)
    {
        if (!itemsInCart.ContainsKey(itemToRemove))
            return;
        else if (itemsInCart[itemToRemove] > 1)
            itemsInCart[itemToRemove] -= 1;
        else
            itemsInCart.Remove(itemToRemove);
    }

    //IEnumerator CompareCartToShoppingList()
    //{
    //    if (gotEverything)
    //        yield break;

    //    //Wait to make sure all items are really in cart
    //    yield return new WaitForSeconds(0.1f);

    //    //Set to false if any key is not in the cart
    //    gotEverything = true;
    //    //Set to false if any value is changed
    //    noChange = true;

    //    for (int i = 0; i < curEntries; i++)
    //    {
    //        var searchKey = requiredItems.ElementAt(i).Key;
    //        bool gotItem = false;

    //        if (itemsInCart.ContainsKey(searchKey))
    //            if (!FixedItemAmount)
    //                gotItem = true;
    //            else
    //                gotItem = itemsInCart[searchKey] >= requiredItems[searchKey] ? true : false;

    //        if (shopListGUI[i] == gotItem)
    //            noChange = false;
    //        else
    //            shopListGUI[i] = gotItem;

    //        if (!gotItem)
    //            gotEverything = false;
    //    }

    //    if (!noChange)
    //        StrikeItems(shopListGUI);
    //}
}
