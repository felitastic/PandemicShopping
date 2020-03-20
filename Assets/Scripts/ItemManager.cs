using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Keeps track of all Items in scene, handles updates of the location of items and helps retrieve values from the item list
/// </summary>
public class ItemManager : Singleton<ItemManager>
{
    protected ItemManager() { }

    public Dictionary<Item, eItemLocation> AllItems { get; private set; }
    public Dictionary<eItemType, int> AllItemsByType { get; private set; }

    public HashSet<Item> AllMovingItems { get; private set; }

    public static event Action<bool> ItemAddedToCart = delegate { };

    private void Awake()
    {
        AllItems = new Dictionary<Item, eItemLocation>();
        AllItemsByType = new Dictionary<eItemType, int>();
        AllMovingItems = new HashSet<Item>();
    }
    private void Start()
    {
        ItemSpawn.OnItemCreation += AddToDictionary;
        ItemSpawn.OnItemCreation += AddItemToAllItemsByType;
        ItemCollider.ItemLocationChange += ChangeLocation;
        Shelf.ItemsPushed += MovingItemsList;
        Item.ItemStoppedMoving += RemoveFromMovingList;
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

    void MovingItemsList(HashSet<Item> itemsPushed)
    {
        foreach(Item i in itemsPushed)
        {
            AllMovingItems.Add(i);
        }
    }

    void RemoveFromMovingList(Item item)
    {
        AllMovingItems.Remove(item);
        item.ChangeIsMoving(false);
    }

    void AddToMovingItemsList(Item item)
    {
        AllMovingItems.Add(item);
        item.ChangeIsMoving(true);
    }

    void ChangeLocation(Item item, eItemLocation newL)
    {
        eItemLocation oldL = AllItems[item];
        if (!LocationChanged(newL, oldL))
            return;

        AllItems[item] = newL;

        if (AddedToCart(newL, oldL))
        {
            RemoveFromMovingList(item);
            ItemAddedToCart(true);
            //StartCoroutine(ReallyAddedToCart(item));
        }
        else if (FellFromCart(newL, oldL))
        {
            AddToMovingItemsList(item);
            ItemAddedToCart(false);
        }
        else if (FellOnGround(newL, oldL))
        {
            RemoveFromMovingList(item);
        }
    }

    //IEnumerator ReallyAddedToCart(Item item)
    //{
    //    int wait = 0;

    //    while (wait < 5)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        if ()
    //    }

    //    RemoveFromMovingList(item);
    //    ItemAddedToCart(true);
    //}

    bool LocationChanged(eItemLocation newL, eItemLocation oldL)
    {
        return newL == oldL ? false : true;
    }
    bool AddedToCart(eItemLocation newL, eItemLocation oldL)
    {
        if (newL == eItemLocation.cart && oldL != eItemLocation.cart)
            return true;

        return false;
    }
    bool FellFromCart(eItemLocation newL, eItemLocation oldL)
    {
        if (newL != eItemLocation.cart && oldL == eItemLocation.cart)
            return true;

        return false;
    }
    bool FellOnGround(eItemLocation newL, eItemLocation oldL)
    {
        return newL == eItemLocation.ground ? true : false;
    }
    
    public void AddItemToAllItemsByType(Item item)
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
        int rand = UnityEngine.Random.Range(0, AllItemsByType.Count);
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
