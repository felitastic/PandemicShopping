using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Keeps track of all Items in scene, handles updates of the location of items and helps retrieve values from the item list
/// </summary>
public class ItemManager : Singleton<ItemManager>
{
    protected ItemManager() { }

    public Dictionary<Item, eItemLocation> AllItems { get; private set; }
    public Dictionary<eItemType, int> AllItemsByType { get; private set; }

    private void Awake()
    {
        AllItems = new Dictionary<Item, eItemLocation>();
        AllItemsByType = new Dictionary<eItemType, int>();
    }
    private void Start()
    {
        ItemSpawn.OnItemCreation += AddToDictionary;
        ItemSpawn.OnItemCreation += SortAllItemsByType;
        ItemCollider.ItemLocationChange += ChangeLocation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            string allitems = "All Items in Scene:\n";

            foreach (KeyValuePair<eItemType, int> pair in AllItemsByType)
            {
                allitems += pair.Value + "x " + pair.Key+"\n";
            }
            if (AllItemsByType.Count == 0)
                allitems += "None found :(";

            print(allitems);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            string allitems = "All Items in Cart:\n";

            List<Item> cart = AllItemsInCart();

            foreach(Item item in cart)
            {
                allitems += item.Name + "\n";
            }
            if (cart.Count == 0)
                allitems = "Cart is empty :(";

            print(allitems);
        }
    }

    private void AddToDictionary(Item newItem)
    {
        AllItems.Add(newItem, eItemLocation.shelf);
    }

    public eItemLocation curLocation(Item item)
    {
        return AllItems.ContainsKey(item) ? AllItems[item] : eItemLocation.shelf;            
    }

    public void ChangeLocation(Item item, eItemLocation newLocation)
    {
        AllItems[item] = newLocation;
    }

    public void SortAllItemsByType(Item item)
    {
        if (AllItemsByType.ContainsKey(item.Type))
        {
            AllItemsByType[item.Type] += 1;
            return;
        }

        AllItemsByType.Add(item.Type, 1);
    }

    public eItemType GetRandomFromAllItems()
    {
        int rand = Random.Range(0, AllItemsByType.Count);
        return AllItemsByType.ElementAt(rand).Key;
    }

    public Dictionary<eItemType, int> SortByItemType(List<Item> itemsToSort)
    {
        Dictionary<eItemType, int> sortedItems = new Dictionary<eItemType, int>();
        foreach (Item item in itemsToSort)
        {
            if (sortedItems.ContainsKey(item.Type))
            {
                sortedItems[item.Type] += 1;
            }
            else
            {
                sortedItems.Add(item.Type, 1);
            }
        }
        return sortedItems;
    }

    public List<Item> AllItemsInCart()
    {
        List<Item> cartItems = (from i in AllItems
                                where i.Value == eItemLocation.cart
                                select i.Key).ToList();
        return cartItems;
    }

    public List<Item> AllItemsInShelf(int shelfID)
    {
        List<Item> shelfItems = (from i in AllItems
                                 where i.Value == eItemLocation.shelf && i.Key.ShelfID == shelfID
                                 select i.Key).ToList();
        return shelfItems;
    }
}
